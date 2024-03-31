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

    // 이메일로 회원가입
    public void SignUpWithEmail(string email, string password, Action<bool, bool> onCompletion)
    {
        FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                print("회원가입 실패: " + task.Exception);
                // 회원가입 실패, onCompletion의 첫 번째 인자는 회원가입 성공 여부, 두 번째 인자는 이메일 인증 전송 여부
                onCompletion(false, false);
            }
            else
            {
                print("회원가입 성공");
                // 여기에서 이메일 인증을 요청합니다.
                SendEmailVerification(emailVerificationSent =>
                {
                    // StartCoroutine(GameManager.Instance.uiManager.ResendEmailCooldown());
                    onCompletion(true, emailVerificationSent); // onCompletion의 첫 번째 인자는 회원가입 성공 여부, 두 번째 인자는 이메일 인증 전송 성공 여부
                });
            }
        });
    }

    // 이메일로 로그인
    public void SignInWithEmail(string email, string password, Action<bool> onCompletion)
    {
        GameManager.Instance.firebaseManager.auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                print("로그인 실패: " + task.Exception);
                onCompletion(false);
            }
            else
            {
                onCompletion(true); // 로그인 성공
            }
        });
    }

    // 인증 메일 전송
    public void SendEmailVerification(Action<bool> onCompletion)
    {
        var user = FirebaseAuth.DefaultInstance.CurrentUser;
        if (user != null)
        {
            user.SendEmailVerificationAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                {
                    print("이메일 인증 전송 실패: " + task.Exception);
                    onCompletion(false);
                }
                else
                {
                    print("이메일 인증 링크 전송 성공");
                    onCompletion(true);
                }
            });
        }
        else
        {
            print("사용자 로그인이 필요합니다");
            onCompletion(false);
        }
    }

    // 인증 여부 확인
    public async void CheckEmailVerification(Action<bool> onCompletion)
    {
        print($"{FirebaseAuth.DefaultInstance.CurrentUser}");
        var user = FirebaseAuth.DefaultInstance.CurrentUser;
        if (user != null)
        {
            await user.ReloadAsync().ContinueWithOnMainThread(async reloadTask =>
            {
                if (reloadTask.IsCanceled || reloadTask.IsFaulted)
                {
                    print("사용자 정보 새로고침 실패: " + reloadTask.Exception);
                    onCompletion(false);
                }
                else
                {
                    // 사용자의 이메일 인증 상태를 확인합니다.
                    if (user.IsEmailVerified)
                    {
                        print("이메일 인증됨");

                        // 이메일 인증 상태 업데이트
                        GameManager.Instance.SetIsEmailAuthentication(true);

                        await GameManager.Instance.firebaseManager.InitializeUserData(user.UserId, user.Email, success =>
                        {
                            if (success)
                            {
                                onCompletion(true);
                            }
                            else
                            {
                                onCompletion(false);
                            }
                            
                        });
                    }
                    else
                    {
                        print("이메일 미인증");
                        onCompletion(false);
                    }
                }
            });
        }
        else
        {
            print("사용자 로그인이 필요합니다.");
            onCompletion(false);
        }
    }
}
