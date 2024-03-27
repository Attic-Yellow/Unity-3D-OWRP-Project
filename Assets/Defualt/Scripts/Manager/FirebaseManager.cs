using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using System;
using Firebase.Auth;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class FirebaseManager : MonoBehaviour
{
    // 랭킹 데이터를 위한 클래스
    [System.Serializable]
    public class RankingEntry
    {
        public string userID;
        public string nickname;
        public int score;
    }

    public FirebaseAuth auth { get; private set; }

    FirebaseFirestore db;
    public bool IsFirebaseInitialized = false; // 초기화 상태 플래그

    private void Awake()
    {
        GameManager.Instance.firebaseManager = this;

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                print("Could not resolve all Firebase dependencies: " + task.Exception);
                return;
            }

            var dependencyStatus = task.Result;

            if (dependencyStatus == DependencyStatus.Available)
            {
                db = FirebaseFirestore.DefaultInstance;
                auth = FirebaseAuth.DefaultInstance;
                IsFirebaseInitialized = true; // 초기화 완료
                print("Firebase를 사용할 준비가 됨");
            }
            else
            {
                print($"모든 Firebase 종속성을 해결할 수 없음: {dependencyStatus}");
            }
        });
    }

    // 게스트 로그인
    public void SignInAnonymously(Action<bool> onCompletion)
    {
        auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                print("게스트 로그인 실패: " + task.Exception);
                onCompletion(false);
            }
            else
            {
                FirebaseUser newUser = task.Result.User; // 게스트 로그인 성공
                InitializeUserData(newUser.UserId, success =>
                {
                    if (success)
                    {
                        GameManager.Instance.firebaseManager.UpdateGuestStatus(newUser.UserId, true, guestUpdated => { });
                        GameManager.Instance.firebaseManager.UpdateChangedToEmailAccount(newUser.UserId, false, guestUpdated => { });
                    }
                    else
                    {
                        print("게스트 사용자 데이터 초기화 실패");
                    }
                    onCompletion(true);
                });
            }
        });
    }

    // 사용자 데이터 로드
    public async void LoadUserData(string userId, Action<Dictionary<string, object>> onCompletion)
    {
        var docRef = db.Collection("users").Document(userId);
        try
        {
            var snapshot = await docRef.GetSnapshotAsync();
            if (!snapshot.Exists)
            {
                print("유저 데이터 로드 실패");
                onCompletion(null);
                return;
            }

            var userData = snapshot.ToDictionary();
            onCompletion(userData);
        }
        catch (Exception ex)
        {
            print($"유저 데이터 로드 중 오류 발생: {ex.Message}");
            onCompletion(null);
        }
    }

    // 사용자 데이터 초기화
    public void InitializeUserData(string userId, Action<bool> onCompletion)
    {
        var docRef = db.Collection("users").Document(userId);
        var user = new Dictionary<string, object>
    {
        { "Guest" , GameManager.Instance.GetIsUserGuest() },
        { "emailauthentication", GameManager.Instance.GetIsEmailAuthentication() },
        { "ChangedToEmailAccount", GameManager.Instance.GetIsChangedToEmailAccount() }
    };
        docRef.SetAsync(user).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                print("사용자 데이터 초기화 실패");
                onCompletion?.Invoke(false);
            }
            else
            {
                onCompletion?.Invoke(true);
            }
        });
    }

    // 문서 필드 값 존재 여부 확인
    public void CheckFieldValueExists(string document, string userValue, System.Action<bool> onResult)
    {
        if (!IsFirebaseInitialized)
        {
            print("Firebase가 아직 초기화되지 않았음");
            onResult(false);
            return;
        }

        DocumentReference docRef = db.Collection("answer").Document(document);
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                print("문서 조회 중 오류 발생");
                onResult(false);
                return;
            }

            var documentSnapshot = task.Result;
            if (!documentSnapshot.Exists)
            {
                print("korean 문서가 존재하지 않습니다.");
                onResult(false);
                return;
            }


            if (documentSnapshot.ContainsField(userValue)) // 사용자 입력 값이 필드로 존재하는지 확인
            {
                onResult(true); // print($"필드 '{userValue}'가 존재함");
            }
            else
            {
                onResult(false); // print($"필드 '{userValue}'가 존재하지 않음);
            }
        });
    }

    // 게스트 여부 상태 업데이트
    public void UpdateGuestStatus(string userId, bool isGuest, Action<bool> onCompletion)
    {
        var docRef = db.Collection("users").Document(userId);
        Dictionary<string, object> updates = new Dictionary<string, object>
    {
        { "Guest", isGuest }
    };
        docRef.UpdateAsync(updates).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                print("Guest 상태 업데이트 실패: " + task.Exception);
                onCompletion(false);
            }
            else
            {
                print("Guest 상태 업데이트 성공");
                onCompletion(true);
            }
        });
    }

    // 게스트 유저 회원 전환 상태 업데이트
    public void UpdateChangedToEmailAccount(string userId, bool isChangedToEmailAccount, Action<bool> onCompletion)
    {
        var docRef = db.Collection("users").Document(userId);
        Dictionary<string, object> updates = new Dictionary<string, object>
    {
        { "ChangedToEmailAccount", isChangedToEmailAccount }
    };
        docRef.UpdateAsync(updates).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                // print("회원 전환 정보 업데이트 실패: " + task.Exception);
                onCompletion(false);
            }
            else
            {
                // print("회원 전환 정보 업데이트 성공");
                onCompletion(true);
            }
        });
    }

    // 이메일 인증 상태 업데이트
    public void UpdateEmailAuthentication(string userId, bool isEmailAuthentication, Action<bool> onCompletion)
    {
        var docRef = db.Collection("users").Document(userId);
        Dictionary<string, object> updates = new Dictionary<string, object>
    {
        { "emailauthentication", isEmailAuthentication }
    };
        docRef.UpdateAsync(updates).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                print("이메일 인증 상태 업데이트 실패: " + task.Exception);
                onCompletion(false);
            }
            else
            {
                print("이메일 인증 상태 업데이트 성공");
                onCompletion(true);
            }
        });
    }

    //  로그아웃
    public void SignOut()
    {
        if (auth.CurrentUser != null)
        {
            auth.SignOut();
            Debug.Log("로그아웃 성공");
        }
    }
}
