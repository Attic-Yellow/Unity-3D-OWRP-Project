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

    // �α��� �� ȸ������ ��ư 
    public void OnAccountButtonClick()
    {
        if (accountUIPage != null)
        {
            accountUIPage.SetActive(!accountUIPage.activeInHierarchy);
            accountUI.InputFieldUIPageController();
            accountUI.SigninUIPageController();
        }
    }

    // ���� ��ư
    public void OnStartButtonClick()
    {
        GameManager.Instance.sceneLoadManager.LoadScene("MainScene");
    }

    // ���� ��ư
    public void OnExitButtonClick()
    {
        GameManager.Instance.uiManager.OnExitGame();
    }

}
