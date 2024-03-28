using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class AccountSystem : MonoBehaviour
{
    [SerializeField] private TMP_InputField idInputField;
    [SerializeField] private TMP_InputField passwordInputField;

    [SerializeField] private string email;
    [SerializeField] private string password;

    /***��ư �ݺ� �޼���***/

    // �α��� ��ư �ݹ�
    public void OnSigninButtonCallBack()
    {
        if (!Regex.IsMatch(idInputField.text, @"[!@#$%^&*(),.?"":{}|<>]") || idInputField.text.Length < 8)
        {
            Debug.LogError("Ư������ �����ؼ� 8�ڸ� �̻� ���� ��"); // Ư�����ڰ� ���Ե��� ���� ��� ���� �޽��� ���
            return; // ���⼭ �Լ� ������ �ߴ�
        }
        StartCoroutine(SigninCoroutine(idInputField.text, passwordInputField.text));
    }

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
            }
            else if (signUpSuccess)
            {
                print("ȸ�������� ���������� �̸��� ���� ��ũ ���ۿ� �����߽��ϴ�.");
            }
            else
            {
                print("ȸ������ ����");
            }
        });
    }

    // �̸��� ���� Ȯ�� ��ư �ݹ�
    public void OnCompleteEVButtonCallBack()
    {
        GameManager.Instance.authManager.CheckEmailVerification((isVerified) =>
        {
            if (isVerified)
            {
                Debug.Log("�̸��� ���� ����");
                if (GameManager.Instance.GetIsChangedToEmailAccount())
                {
                    StartCoroutine(SigninCoroutine(email, password));
                }
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
            GameManager.Instance.SetIsSignInSuccess(true); // �α��� ���� �� ó��
            yield return new WaitUntil(() => GameManager.Instance.GetIsSignInSuccess());
            InitInputField();
            GameManager.Instance.uiManager.startSceneUI.OnAccountButtonClick(true);
        }
    }

    private void InitInputField()
    {
        idInputField.text = "";
        passwordInputField.text = "";
    }
}
