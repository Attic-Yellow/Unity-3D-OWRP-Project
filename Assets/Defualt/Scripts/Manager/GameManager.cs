using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("매니저")]
    public FirebaseManager firebaseManager;
    public AuthManager authManager;
    public UIManager uiManager;
    public SceneLoadManager sceneLoadManager;

    [Header("유저 데이터")]
    private bool isUserGuest = false;
    private bool isEmailAuthentication = false;
    private bool isChangedToEmailAccount = false;
    private string userId;

    [Header("게임 데이터")]
    private bool isDataLoaded;
    private bool isSignInSuccess;

    [Serializable]
    private class UserData
    {
        public bool guestUser { get; set; }
        public bool emailAuthentication { get; set; }
        public bool changedToEmailAccount { get; set; }
        public string nickname { get; set; }
        public int score { get; set; }
        public int coins { get; set; }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        
    }

    // 로그인 성공 시 호출되는 콜백 메서드
    public void OnLoginSuccess()
    {
        SetIsSignInSuccess(true);
    }

    // 파이어 베이스 매니저 초기화 확인 메서드
    private IEnumerator CheckAutoLoginWhenReady()
    {
        yield return new WaitUntil(() => firebaseManager != null && firebaseManager.IsFirebaseInitialized); // FirebaseManager가 초기화될 때까지 대기

        if (firebaseManager.auth != null) // FirebaseAuth가 준비되었는지 확인
        {

        }
        else
        {

        }
    }

    // 사용자 데이터 불러오기
    public void LoadCurrentUserProfile()
    {
        if (firebaseManager.auth.CurrentUser != null)
        {
            userId = firebaseManager.auth.CurrentUser.UserId;
            firebaseManager.LoadUserData(userId, OnUserDataLoaded);
        }
        else
        {
            Debug.LogError("사용자가 로그인되어 있지 않음");
        }
    }

    // 사용자 데이터 로드 후 호출되는 콜백 메서드
    private void OnUserDataLoaded(Dictionary<string, object> userData)
    {
        if (userData != null)
        {
            try
            {
                string json = JsonConvert.SerializeObject(userData); // userData 사전을 다시 JSON 문자열로 변환
                UserData deserializedUserData = JsonConvert.DeserializeObject<UserData>(json); // JSON 문자열을 UserData 클래스로 역직렬화

                if (deserializedUserData != null) // 역직렬화된 객체의 속성에 직접 접근
                {
                    isUserGuest = deserializedUserData.guestUser;
                    isEmailAuthentication = deserializedUserData.emailAuthentication;
                    isChangedToEmailAccount = deserializedUserData.changedToEmailAccount;
                    isDataLoaded = true;
                }
                else
                {
                    print("사용자 데이터 역직렬화 실패");
                }
            }
            catch (Exception ex)
            {
                print($"사용자 데이터 처리 중 오류 발생: {ex.Message}");
            }
        }
        else
        {
            //ToDo : 로그인 재시도 로직 필요
        }
    }

    private IEnumerator WaitForUserData()
    {
        isDataLoaded = false;
        LoadCurrentUserProfile();
        yield return new WaitUntil(() => isDataLoaded);

        if (isEmailAuthentication || isUserGuest)
        {
        }
        else
        {
            yield return new WaitUntil(() => isEmailAuthentication);
        }
    }

    public void LogOut()
    {
        firebaseManager.SignOut();
        // sceneManager.LoadSceneForLogin();
    }

    public void SetIsUserGuest(bool isUserGuest)
    {
        this.isUserGuest = isUserGuest;
    }

    public bool GetIsUserGuest()
    {
        return isUserGuest;
    }

    public void SetIsEmailAuthentication(bool isEmailAuthentication)
    {
        this.isEmailAuthentication = isEmailAuthentication;
    }

    public bool GetIsEmailAuthentication()
    {
        return isEmailAuthentication;
    }

    public void SetIsChangedToEmailAccount(bool isChangedToEmailAccount)
    {
        this.isChangedToEmailAccount = isChangedToEmailAccount;
    }

    public bool GetIsChangedToEmailAccount()
    {
        return isChangedToEmailAccount;
    }

    public void SetIsSignInSuccess(bool isSignInSuccess)
    {
        this.isSignInSuccess = isSignInSuccess;
    }

    public bool GetIsSignInSuccess()
    {
        return isSignInSuccess;
    }
}
