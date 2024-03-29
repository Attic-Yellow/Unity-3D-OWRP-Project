using Firebase.Auth;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("�Ŵ���")]
    public FirebaseManager firebaseManager;
    public AuthManager authManager;
    public PhotonManager photonManager;
    public DataManager dataManager;
    public UIManager uiManager;
    public SceneLoadManager sceneLoadManager;

    [Header("���� ������")]
    private bool isUserGuest = false;
    private bool isEmailAuthentication = false;
    private bool isChangedToEmailAccount = false;
    private bool isManager = false;

    [Header("���� ������")]
    private bool isDataLoaded;
    private bool isSignInSuccess;

    [Serializable]
    private class UserData
    {
        public bool guestUser { get; set; }
        public bool emailAuthentication { get; set; }
        public bool changedToEmailAccount { get; set; }
        public bool manager { get; set; }
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

    // �α��� ���� �� ȣ��Ǵ� �ݹ� �޼���
    public void OnLoginSuccess()
    {
        StartCoroutine(CheckAutoLoginWhenReady());
    }

    // ���̾� ���̽� �Ŵ��� �ʱ�ȭ Ȯ�� �޼���
    private IEnumerator CheckAutoLoginWhenReady()
    {
        yield return new WaitUntil(() => firebaseManager != null && firebaseManager.IsFirebaseInitialized); // FirebaseManager�� �ʱ�ȭ�� ������ ���

        if (firebaseManager.auth != null) // FirebaseAuth�� �غ�Ǿ����� Ȯ��
        {
            StartCoroutine(WaitForUserData());
        }
        else
        {

        }
    }

    // ����� ������ �ҷ�����
    public void LoadCurrentUserProfile()
    {

        if (firebaseManager.auth.CurrentUser != null)
        {
            var user = FirebaseAuth.DefaultInstance.CurrentUser;
            firebaseManager.LoadUserData(user.UserId, OnUserDataLoaded);
        }
        else
        {
            Debug.LogError("����ڰ� �α��εǾ� ���� ����");
        }
    }

    // ����� ������ �ε� �� ȣ��Ǵ� �ݹ� �޼���
    private void OnUserDataLoaded(Dictionary<string, object> userData)
    {
        if (userData != null)
        {
            try
            {
                string json = JsonConvert.SerializeObject(userData); // userData ������ �ٽ� JSON ���ڿ��� ��ȯ
                UserData deserializedUserData = JsonConvert.DeserializeObject<UserData>(json); // JSON ���ڿ��� UserData Ŭ������ ������ȭ

                if (deserializedUserData != null) // ������ȭ�� ��ü�� �Ӽ��� ���� ����
                {
                    isUserGuest = deserializedUserData.guestUser;
                    isEmailAuthentication = deserializedUserData.emailAuthentication;
                    isChangedToEmailAccount = deserializedUserData.changedToEmailAccount;
                    isManager = deserializedUserData.manager;

                    isDataLoaded = true;
                    isSignInSuccess = true;
                }
                else
                {
                    print("����� ������ ������ȭ ����");
                }
            }
            catch (Exception ex)
            {
                print($"����� ������ ó�� �� ���� �߻�: {ex.Message}");
            }
        }
        else
        {
            //ToDo : �α��� ��õ� ���� �ʿ�
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

    public bool GetIsManager()
    {
        return isManager;
    }
}
