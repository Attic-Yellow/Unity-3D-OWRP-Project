using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using System;
using Firebase.Extensions;
using Unity.VisualScripting;

public class AuthManager : MonoBehaviour
{
    private static AuthManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        GameManager.Instance.authManager = this;
    }

    // �̸��Ϸ� ȸ������
    public void SignUpWithEmail(string email, string password, Action<bool, bool> onCompletion)
    {
        FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                print("ȸ������ ����: " + task.Exception);
                // ȸ������ ����, onCompletion�� ù ��° ���ڴ� ȸ������ ���� ����, �� ��° ���ڴ� �̸��� ���� ���� ����
                onCompletion(false, false);
            }
            else
            {
                print("ȸ������ ����");
                // ���⿡�� �̸��� ������ ��û�մϴ�.
                SendEmailVerification(emailVerificationSent =>
                {
                    // StartCoroutine(GameManager.Instance.uiManager.ResendEmailCooldown());
                    onCompletion(true, emailVerificationSent); // onCompletion�� ù ��° ���ڴ� ȸ������ ���� ����, �� ��° ���ڴ� �̸��� ���� ���� ���� ����
                });
            }
        });
    }

    // �̸��Ϸ� �α���
    public void SignInWithEmail(string email, string password, Action<bool> onCompletion)
    {
        GameManager.Instance.firebaseManager.auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                print("�α��� ����: " + task.Exception);
                onCompletion(false);
            }
            else
            {
                onCompletion(true); // �α��� ����
            }
        });
    }

    // ���� ���� ����
    public void SendEmailVerification(Action<bool> onCompletion)
    {
        var user = FirebaseAuth.DefaultInstance.CurrentUser;
        if (user != null)
        {
            user.SendEmailVerificationAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                {
                    print("�̸��� ���� ���� ����: " + task.Exception);
                    onCompletion(false);
                }
                else
                {
                    print("�̸��� ���� ��ũ ���� ����");
                    onCompletion(true);
                }
            });
        }
        else
        {
            print("����� �α����� �ʿ��մϴ�");
            onCompletion(false);
        }
    }

    // ���� ���� Ȯ��
    public void CheckEmailVerification(Action<bool> onCompletion)
    {
        print($"{FirebaseAuth.DefaultInstance.CurrentUser}");
        var user = FirebaseAuth.DefaultInstance.CurrentUser;
        if (user != null)
        {
            user.ReloadAsync().ContinueWithOnMainThread(reloadTask =>
            {
                if (reloadTask.IsCanceled || reloadTask.IsFaulted)
                {
                    print("����� ���� ���ΰ�ħ ����: " + reloadTask.Exception);
                    onCompletion(false);
                }
                else
                {
                    // ������� �̸��� ���� ���¸� Ȯ���մϴ�.
                    if (user.IsEmailVerified)
                    {
                        print("�̸��� ������");

                        // �̸��� ���� ���� ������Ʈ
                        GameManager.Instance.SetIsEmailAuthentication(true);
                    }
                    else
                    {
                        print("�̸��� ������");
                        onCompletion(false);
                    }
                }
            });
        }
        else
        {
            print("����� �α����� �ʿ��մϴ�.");
            onCompletion(false);
        }
    }
}
