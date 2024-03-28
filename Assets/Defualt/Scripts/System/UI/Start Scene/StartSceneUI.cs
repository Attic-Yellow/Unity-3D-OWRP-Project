using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartSceneUI : MonoBehaviour
{
    public AccountUI accountUI;

    [SerializeField] private GameObject accountUIPage;

    public TextMeshProUGUI testText;

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
    }

    // 로그인 및 회원가입 버튼 
    public void OnAccountButtonClick()
    {
        if (accountUIPage != null)
        {
            accountUIPage.SetActive(!accountUIPage.activeInHierarchy);
            accountUI.InputFieldUIPageController();
            accountUI.SigninUIPageController();
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
