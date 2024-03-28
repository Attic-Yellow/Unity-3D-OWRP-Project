using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    // ���� UI ������ Ȱ��ȭ/��Ȱ��ȭ ��Ʈ�ѷ� �޼���
    public void accountUIPageController()
    {
        if (accountUIPage != null)
        {
            accountUIPage.SetActive(!accountUIPage.activeInHierarchy);
        }
    }

    // �α��� �� ȸ������ ��ư Ȱ��ȭ/��Ȱ��ȭ ��Ʈ�ѷ� �޼���
    public void AccountButtonController()
    {
        if (acoountButton != null)
        {
            acoountButton.SetActive(!acoountButton.activeInHierarchy);
        }
    }

    // �α��� �� ȸ������ ��ư 
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
