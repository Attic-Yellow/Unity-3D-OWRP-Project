using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AccountSystem : MonoBehaviour
{
    [SerializeField] private TMP_InputField idInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private Button signinButton;
    [SerializeField] private Button signupGoButton;
    [SerializeField] private Button signupButton;
    [SerializeField] private Button evButton;

    [SerializeField] private string email;
    [SerializeField] private string password;

    private int currentIndex = 0;

    /***버튼 콜벡 메서드***/

    // 로그인 버튼 콜백
    public void OnSigninButtonCallBack()
    {
        if (!Regex.IsMatch(idInputField.text, @"[!@#$%^&*(),.?"":{}|<>]") || idInputField.text.Length < 8)
        {
            Debug.LogError("특수문자 포함해서 8자리 이상 만들 것"); // 특수문자가 포함되지 않은 경우 에러 메시지 출력
            return; // 여기서 함수 실행을 중단
        }

        GameManager.Instance.uiManager.startSceneUI.LoadingAreaController(); // 로딩 화면 활성화
        StartCoroutine(SigninCoroutine(idInputField.text, passwordInputField.text));
    }

    // 회원 가입 버튼 콜백
    public void OnSignupButtonCallBack()
    {
        GameManager.Instance.authManager.SignUpWithEmail(idInputField.text, passwordInputField.text, (signUpSuccess, emailSent) =>
        {
            if (signUpSuccess && emailSent)
            {
                email = idInputField.text;
                password = passwordInputField.text;
                Debug.Log("회원가입 성공 및 이메일 인증 링크 전송 완료");
                InitInputField();
                GameManager.Instance.uiManager.startSceneUI.LoadingAreaController(); // 로딩 화면 비활성화
                return;
            }
            else if (signUpSuccess)
            {
                print("회원가입은 성공했지만 이메일 인증 링크 전송에 실패했습니다.");
                return;
            }
            else
            {
                print("회원가입 실패");
                return;
            }
        });
    }

    // 이메일 인증 확인 버튼 콜백
    public void OnCompleteEVButtonCallBack()
    {
        GameManager.Instance.uiManager.startSceneUI.LoadingAreaController(); // 로딩 화면 활성화
        GameManager.Instance.authManager.CheckEmailVerification((isVerified) =>
        {
            if (isVerified)
            {
                Debug.Log("이메일 인증 성공");
                StartCoroutine(SigninCoroutine(email, password));
            }
            else
            {
                print("이메일 인증이 아직 완료되지 않았습니다. 이메일을 확인해주세요.");
                // GameManager.Instance.authManager.SendEmailVerification((reSandVerified) => { });
            }
        });
    }

    /*** 코루틴 ***/

    // 로그인 비동기 로직
    public IEnumerator SigninCoroutine(string email, string password)
    {
        var signInTask = FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(email, password); // 로그인되어 있지 않은 경우, 이메일과 비밀번호를 사용하여 로그인 시도
        yield return new WaitUntil(() => signInTask.IsCompleted);

        if (signInTask.Exception != null)
        {
            print("로그인 실패: " + signInTask.Exception);
        }
        else
        {
            var user = FirebaseAuth.DefaultInstance.CurrentUser;
            print(user.UserId);
            GameManager.Instance.OnLoginSuccess();
            yield return new WaitUntil(() => GameManager.Instance.GetIsSignInSuccess());
            InitInputField();
            GameManager.Instance.uiManager.startSceneUI.OnAccountButtonClick(true);
            GameManager.Instance.uiManager.startSceneUI.LoadingAreaController(); // 로딩 화면 비활성화
        }
    }

    private void InitInputField()
    {
        idInputField.text = "";
        passwordInputField.text = "";
    }

    private void OnNext(InputValue value)
    {
        int index = GameManager.Instance.uiManager.startSceneUI.accountUI.GetCurrentAreaIndex();

        print(index);
        print(currentIndex);
        switch (currentIndex)
        {
            case 0:
                EventSystem.current.SetSelectedGameObject(idInputField.gameObject, null);
                idInputField.OnPointerClick(new PointerEventData(EventSystem.current)); // 포커스와 함께 클릭 이벤트도 발생
                currentIndex++;
                break;
            case 1:
                EventSystem.current.SetSelectedGameObject(passwordInputField.gameObject, null);
                passwordInputField.OnPointerClick(new PointerEventData(EventSystem.current)); // 포커스와 함께 클릭 이벤트도 발생
                currentIndex++;
                break;
            case 2:
                EventSystem.current.SetSelectedGameObject(signinButton.gameObject, null);
                currentIndex++;
                break;
            case 3:
                if (index == 0)
                {
                    EventSystem.current.SetSelectedGameObject(signupGoButton.gameObject, null);
                    currentIndex++;
                }
                else if (index == 1)
                {
                    EventSystem.current.SetSelectedGameObject(signupButton.gameObject, null);
                }
                break;
            case 4:
                EventSystem.current.SetSelectedGameObject(idInputField.gameObject, null);
                idInputField.OnPointerClick(new PointerEventData(EventSystem.current));
                currentIndex = 1;
                break;
        }
    }

    private void OnBack(InputValue value)
    {
        print(currentIndex);
        switch (currentIndex)
        {
            case 3:
                EventSystem.current.SetSelectedGameObject(signupGoButton.gameObject, null);
                currentIndex--;
                break;
            case 2:
                EventSystem.current.SetSelectedGameObject(passwordInputField.gameObject, null);
                passwordInputField.OnPointerClick(new PointerEventData(EventSystem.current));
                currentIndex--;
                break;
            case 1:
                EventSystem.current.SetSelectedGameObject(idInputField.gameObject, null);
                idInputField.OnPointerClick(new PointerEventData(EventSystem.current)); 
                currentIndex--;
                break;
        }    
    }

    private void OnEnter(InputValue value)
    {
        switch (currentIndex)
        {
            case 1:
                EventSystem.current.SetSelectedGameObject(passwordInputField.gameObject, null);
                passwordInputField.OnPointerClick(new PointerEventData(EventSystem.current));
                break;
            case 2:
                signinButton.OnPointerClick(new PointerEventData(EventSystem.current));
                break;
        }
    }
}
