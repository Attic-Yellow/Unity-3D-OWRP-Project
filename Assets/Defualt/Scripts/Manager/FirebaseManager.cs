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
    // ��ŷ �����͸� ���� Ŭ����
    [System.Serializable]
    public class RankingEntry
    {
        public string userID;
        public string nickname;
        public int score;
    }

    public FirebaseAuth auth { get; private set; }

    FirebaseFirestore db;
    public bool IsFirebaseInitialized = false; // �ʱ�ȭ ���� �÷���

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
                IsFirebaseInitialized = true; // �ʱ�ȭ �Ϸ�
                print("Firebase�� ����� �غ� ��");
            }
            else
            {
                print($"��� Firebase ���Ӽ��� �ذ��� �� ����: {dependencyStatus}");
            }
        });
    }

    // �Խ�Ʈ �α���
    public void SignInAnonymously(Action<bool> onCompletion)
    {
        auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                print("�Խ�Ʈ �α��� ����: " + task.Exception);
                onCompletion(false);
            }
            else
            {
                FirebaseUser newUser = task.Result.User; // �Խ�Ʈ �α��� ����
                InitializeUserData(newUser.UserId, success =>
                {
                    if (success)
                    {
                        GameManager.Instance.firebaseManager.UpdateGuestStatus(newUser.UserId, true, guestUpdated => { });
                        GameManager.Instance.firebaseManager.UpdateChangedToEmailAccount(newUser.UserId, false, guestUpdated => { });
                    }
                    else
                    {
                        print("�Խ�Ʈ ����� ������ �ʱ�ȭ ����");
                    }
                    onCompletion(true);
                });
            }
        });
    }

    // ����� ������ �ε�
    public async void LoadUserData(string userId, Action<Dictionary<string, object>> onCompletion)
    {
        var docRef = db.Collection("users").Document(userId);
        try
        {
            var snapshot = await docRef.GetSnapshotAsync();
            if (!snapshot.Exists)
            {
                print("���� ������ �ε� ����");
                onCompletion(null);
                return;
            }

            var userData = snapshot.ToDictionary();
            onCompletion(userData);
        }
        catch (Exception ex)
        {
            print($"���� ������ �ε� �� ���� �߻�: {ex.Message}");
            onCompletion(null);
        }
    }

    // ����� ������ �ʱ�ȭ
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
                print("����� ������ �ʱ�ȭ ����");
                onCompletion?.Invoke(false);
            }
            else
            {
                onCompletion?.Invoke(true);
            }
        });
    }

    // ���� �ʵ� �� ���� ���� Ȯ��
    public void CheckFieldValueExists(string document, string userValue, System.Action<bool> onResult)
    {
        if (!IsFirebaseInitialized)
        {
            print("Firebase�� ���� �ʱ�ȭ���� �ʾ���");
            onResult(false);
            return;
        }

        DocumentReference docRef = db.Collection("answer").Document(document);
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                print("���� ��ȸ �� ���� �߻�");
                onResult(false);
                return;
            }

            var documentSnapshot = task.Result;
            if (!documentSnapshot.Exists)
            {
                print("korean ������ �������� �ʽ��ϴ�.");
                onResult(false);
                return;
            }


            if (documentSnapshot.ContainsField(userValue)) // ����� �Է� ���� �ʵ�� �����ϴ��� Ȯ��
            {
                onResult(true); // print($"�ʵ� '{userValue}'�� ������");
            }
            else
            {
                onResult(false); // print($"�ʵ� '{userValue}'�� �������� ����);
            }
        });
    }

    // �Խ�Ʈ ���� ���� ������Ʈ
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
                print("Guest ���� ������Ʈ ����: " + task.Exception);
                onCompletion(false);
            }
            else
            {
                print("Guest ���� ������Ʈ ����");
                onCompletion(true);
            }
        });
    }

    // �Խ�Ʈ ���� ȸ�� ��ȯ ���� ������Ʈ
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
                // print("ȸ�� ��ȯ ���� ������Ʈ ����: " + task.Exception);
                onCompletion(false);
            }
            else
            {
                // print("ȸ�� ��ȯ ���� ������Ʈ ����");
                onCompletion(true);
            }
        });
    }

    // �̸��� ���� ���� ������Ʈ
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
                print("�̸��� ���� ���� ������Ʈ ����: " + task.Exception);
                onCompletion(false);
            }
            else
            {
                print("�̸��� ���� ���� ������Ʈ ����");
                onCompletion(true);
            }
        });
    }

    //  �α׾ƿ�
    public void SignOut()
    {
        if (auth.CurrentUser != null)
        {
            auth.SignOut();
            Debug.Log("�α׾ƿ� ����");
        }
    }
}
