using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartSceneUI : MonoBehaviour
{
    public AccountUI accountUI;

    [SerializeField] private GameObject accountUIPage;
    [SerializeField] private GameObject loadingArea;

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

        if (loadingArea != null)
        {
            loadingArea.SetActive(false);
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

    // 계정 UI 페이지 활성화/비활성화 컨트롤러 메서드
    public void accountUIPageController()
    {
        if (accountUIPage != null)
        {
            accountUIPage.SetActive(!accountUIPage.activeInHierarchy);
        }
    }

    // 로그인 및 회원가입 버튼 활성화/비활성화 컨트롤러 메서드
    public void AccountButtonController()
    {
        if (acoountButton != null)
        {
            acoountButton.SetActive(!acoountButton.activeInHierarchy);
        }
    }

    // 로딩 화면 활성화/비활성화 컨트롤러 메서드
    public void LoadingAreaController()
    {
        if (loadingArea != null)
        {
            loadingArea.SetActive(!loadingArea.activeInHierarchy);
        }
    }

    // 로그인 및 회원가입 버튼 
    public void OnAccountButtonClick(bool success)
    {
        if (accountUIPage != null)
        {
            if (!success)
            {
                AccountButtonController();
                accountUIPageController();
                accountUI.UIAreaIndexController(0);
                accountUI.InputFieldUIController(0);
            }
            else
            {
                accountUIPageController();
                AccountButtonController();
                startButton.SetActive(!startButton.activeInHierarchy);
                quitButton.SetActive(!quitButton.activeInHierarchy);
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
