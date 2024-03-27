using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneUI : MonoBehaviour
{
    public AccountUI accountUI;

    [SerializeField] private GameObject accountUIPage;
    [SerializeField] private GameObject acoountButton;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject quitButton;

    private void Awake()
    {
        GameManager.Instance.uiManager.startSceneUI = this;
    }

    private void Start()
    {
        if (accountUIPage != null)
        {
            accountUIPage.SetActive(false);
        }

        if (acoountButton != null)
        {
            acoountButton.SetActive(true);
        }

        if (startButton != null)
        {
            startButton.SetActive(false);
        }

        if (quitButton != null)
        {
            quitButton.SetActive(false);
        }
    }

    // 로그인 및 회원가입 버튼 
    public void OnAccountButtonClick()
    {
        if (accountUIPage != null)
        {
            if (!accountUIPage.activeInHierarchy) // 계정 UI가 활성화 되어 있지 않다면
            {
                accountUIPage.SetActive(!accountUIPage.activeInHierarchy);
                accountUI.InputFieldUIPageController();
                accountUI.SigninUIPageController();
            }
            else // 계정 UI가 활성화 되어 있다면
            {
                accountUIPage.SetActive(!accountUIPage.activeInHierarchy);
                accountUI.AllAreaClose();

                if (GameManager.Instance.GetIsSignInSuccess())
                {
                    acoountButton.SetActive(false);
                    startButton.SetActive(true);
                    quitButton.SetActive(true);
                }
            }
        }
    }

    // 시작 버튼
    public void OnStartButtonClick()
    {
        GameManager.Instance.sceneLoadManager.LoadScene("MainScene");
    }

    // 종료 버튼
    public void OnExitButtonClick()
    {
        GameManager.Instance.uiManager.OnExitGame();
    }

}
