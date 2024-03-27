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

    // �α��� �� ȸ������ ��ư 
    public void OnAccountButtonClick()
    {
        if (accountUIPage != null)
        {
            if (!accountUIPage.activeInHierarchy) // ���� UI�� Ȱ��ȭ �Ǿ� ���� �ʴٸ�
            {
                accountUIPage.SetActive(!accountUIPage.activeInHierarchy);
                accountUI.InputFieldUIPageController();
                accountUI.SigninUIPageController();
            }
            else // ���� UI�� Ȱ��ȭ �Ǿ� �ִٸ�
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
