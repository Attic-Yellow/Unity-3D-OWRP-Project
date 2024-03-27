using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountUI : MonoBehaviour
{
    [SerializeField] private GameObject inputFieldUIArea;
    [SerializeField] private GameObject signinUIArea;
    [SerializeField] private GameObject signupUIArea;
    [SerializeField] private GameObject emailVerificationArea;

    private void Awake()
    {
        GameManager.Instance.uiManager.startSceneUI.accountUI = this;
    }

    private void Start()
    {
        AllAreaClose();
    }

    /*** UI Area Ȱ��ȭ ��Ȱ��ȭ ***/

    // ���� UI Ȱ��ȭ/��Ȱ��ȭ ��Ʈ�ѷ� �޼���
    public void InputFieldUIPageController()
    {
        if (inputFieldUIArea != null)
        {
            inputFieldUIArea.SetActive(!inputFieldUIArea.activeInHierarchy);
        }
    }

    // �α��� UI Ȱ��ȭ/��Ȱ��ȭ ��Ʈ�ѷ� �޼���
    public void SigninUIPageController()
    {
        if (signinUIArea != null)
        {
            signinUIArea.SetActive(!signinUIArea.activeInHierarchy);
        }
    }

    // ȸ�� ���� UI Ȱ��ȭ/��Ȱ��ȭ ��Ʈ�ѷ� �޼���
    public void SignupUIPageController()
    {
        if (signupUIArea != null)
        {
            signupUIArea.SetActive(!signupUIArea.activeInHierarchy);
        }
    }

    // �̸��� ���� UI Ȱ��ȭ/��Ȱ��ȭ ��Ʈ�ѷ� �޼���
    public void EmailVerificationPageController()
    {
        if (emailVerificationArea != null)
        {
            emailVerificationArea.SetActive(!emailVerificationArea.activeInHierarchy);
        }
    }

    // ��� UI�� ��Ȱ��ȭ �ϴ� �޼���
    public void AllAreaClose()
    {
        if (inputFieldUIArea != null)
        {
            inputFieldUIArea.SetActive(false);
        }

        if (signinUIArea != null)
        {
            signinUIArea.SetActive(false);
        }

        if (signupUIArea != null)
        {
            signupUIArea.SetActive(false);
        }

        if (emailVerificationArea != null)
        {
            emailVerificationArea.SetActive(false);
        }
    }

    /*** ��ư Ŭ�� �� ��� ***/

    // �α��� ��ư 
    public void OnSigninButtonClick()
    {
        AccountSystem accountSystem = FindAnyObjectByType<AccountSystem>();
        accountSystem.OnSigninButtonCallBack();
    }

    // ȸ�� ���� ��ư
    public void OnSignupButtonClick()
    {
        if (signinUIArea.activeInHierarchy && !signupUIArea.activeInHierarchy)
        {
            SigninUIPageController();
            SignupUIPageController();
        }
        else
        {
            AccountSystem accountSystem = FindAnyObjectByType<AccountSystem>();
            accountSystem.OnSignupButtonCallBack();
            EmailVerificationPageController(); // �̸��� ���� �ȳ� UI Ȱ��ȭ
            InputFieldUIPageController();
            SignupUIPageController(); // ȸ������ UI ��Ȱ��ȭ
        }
    }

    // ��� ��ȣ ã�� ��ư
    public void OnForgottenPWButtonClick()
    {

    }

    // ���� �Ϸ� ��ư
    public void OnCompleteEVButtonClick()
    {
        AccountSystem accountSystem = FindAnyObjectByType<AccountSystem>();
        accountSystem.OnCompleteEVButtonCallBack();
        GameManager.Instance.uiManager.startSceneUI.OnAccountButtonClick();
    }
}
