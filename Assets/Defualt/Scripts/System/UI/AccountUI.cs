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

    /*** UI Area 활성화 비활성화 ***/

    // 계정 UI 활성화/비활성화 컨트롤러 메서드
    public void InputFieldUIPageController()
    {
        if (inputFieldUIArea != null)
        {
            inputFieldUIArea.SetActive(!inputFieldUIArea.activeInHierarchy);
        }
    }

    // 로그인 UI 활성화/비활성화 컨트롤러 메서드
    public void SigninUIPageController()
    {
        if (signinUIArea != null)
        {
            signinUIArea.SetActive(!signinUIArea.activeInHierarchy);
        }
    }

    // 회원 가입 UI 활성화/비활성화 컨트롤러 메서드
    public void SignupUIPageController()
    {
        if (signupUIArea != null)
        {
            signupUIArea.SetActive(!signupUIArea.activeInHierarchy);
        }
    }

    // 이메일 인증 UI 활성화/비활성화 컨트롤러 메서드
    public void EmailVerificationPageController()
    {
        if (emailVerificationArea != null)
        {
            emailVerificationArea.SetActive(!emailVerificationArea.activeInHierarchy);
        }
    }

    // 모든 UI를 비활성화 하는 메서드
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

    /*** 버튼 클릭 시 기능 ***/

    // 로그인 버튼 
    public void OnSigninButtonClick()
    {
        AccountSystem accountSystem = FindAnyObjectByType<AccountSystem>();
        accountSystem.OnSigninButtonCallBack();
    }

    // 회원 가입 버튼
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
            EmailVerificationPageController(); // 이메일 인증 안내 UI 활성화
            InputFieldUIPageController();
            SignupUIPageController(); // 회원가입 UI 비활성화
        }
    }

    // 비밀 번호 찾기 버튼
    public void OnForgottenPWButtonClick()
    {

    }

    // 인증 완료 버튼
    public void OnCompleteEVButtonClick()
    {
        AccountSystem accountSystem = FindAnyObjectByType<AccountSystem>();
        accountSystem.OnCompleteEVButtonCallBack();
        GameManager.Instance.uiManager.startSceneUI.OnAccountButtonClick();
    }
}
