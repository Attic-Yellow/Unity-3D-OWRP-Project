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

    /***��ư �ݺ� �޼���***/

    // �α��� ��ư �ݹ�
    public void OnSigninButtonCallBack()
    {
        if (!Regex.IsMatch(idInputField.text, @"[!@#$%^&*(),.?"":{}|<>]") || idInputField.text.Length < 8)
        {
            Debug.LogError("Ư������ �����ؼ� 8�ڸ� �̻� ���� ��"); // Ư�����ڰ� ���Ե��� ���� ��� ���� �޽��� ���
            return; // ���⼭ �Լ� ������ �ߴ�
        }

        GameManager.Instance.uiManager.startSceneUI.LoadingAreaController(); // �ε� ȭ�� Ȱ��ȭ
        StartCoroutine(SigninCoroutine(idInputField.text, passwordInputField.text));
    }

    // ȸ�� ���� ��ư �ݹ�
    public void OnSignupButtonCallBack()
    {
        GameManager.Instance.authManager.SignUpWithEmail(idInputField.text, passwordInputField.text, (signUpSuccess, emailSent) =>
        {
            if (signUpSuccess && emailSent)
            {
                email = idInputField.text;
                password = passwordInputField.text;
                Debug.Log("ȸ������ ���� �� �̸��� ���� ��ũ ���� �Ϸ�");
                InitInputField();
                GameManager.Instance.uiManager.startSceneUI.LoadingAreaController(); // �ε� ȭ�� ��Ȱ��ȭ
                return;
            }
            else if (signUpSuccess)
            {
                print("ȸ�������� ���������� �̸��� ���� ��ũ ���ۿ� �����߽��ϴ�.");
                return;
            }
            else
            {
                print("ȸ������ ����");
                return;
            }
        });
    }

    // �̸��� ���� Ȯ�� ��ư �ݹ�
    public void OnCompleteEVButtonCallBack()
    {
        GameManager.Instance.uiManager.startSceneUI.LoadingAreaController(); // �ε� ȭ�� Ȱ��ȭ
        GameManager.Instance.authManager.CheckEmailVerification((isVerified) =>
        {
            if (isVerified)
            {
                Debug.Log("�̸��� ���� ����");
                StartCoroutine(SigninCoroutine(email, password));
            }
            else
            {
                print("�̸��� ������ ���� �Ϸ���� �ʾҽ��ϴ�. �̸����� Ȯ�����ּ���.");
                // GameManager.Instance.authManager.SendEmailVerification((reSandVerified) => { });
            }
        });
    }

    /*** �ڷ�ƾ ***/

    // �α��� �񵿱� ����
    public IEnumerator SigninCoroutine(string email, string password)
    {
        var signInTask = FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(email, password); // �α��εǾ� ���� ���� ���, �̸��ϰ� ��й�ȣ�� ����Ͽ� �α��� �õ�
        yield return new WaitUntil(() => signInTask.IsCompleted);

        if (signInTask.Exception != null)
        {
            print("�α��� ����: " + signInTask.Exception);
        }
        else
        {
            var user = FirebaseAuth.DefaultInstance.CurrentUser;
            print(user.UserId);
            GameManager.Instance.OnLoginSuccess();
            yield return new WaitUntil(() => GameManager.Instance.GetIsSignInSuccess());
            InitInputField();
            GameManager.Instance.uiManager.startSceneUI.OnAccountButtonClick(true);
            GameManager.Instance.uiManager.startSceneUI.LoadingAreaController(); // �ε� ȭ�� ��Ȱ��ȭ
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
                idInputField.OnPointerClick(new PointerEventData(EventSystem.current)); // ��Ŀ���� �Բ� Ŭ�� �̺�Ʈ�� �߻�
                currentIndex++;
                break;
            case 1:
                EventSystem.current.SetSelectedGameObject(passwordInputField.gameObject, null);
                passwordInputField.OnPointerClick(new PointerEventData(EventSystem.current)); // ��Ŀ���� �Բ� Ŭ�� �̺�Ʈ�� �߻�
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
