using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneUI : MonoBehaviour
{
    public AccountUI accountUI;

    [SerializeField] private GameObject accountUIPage;

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

    public void OnAccountButtonClick()
    {
        if (accountUIPage != null)
        {
            if (!accountUIPage.activeInHierarchy)
            {
                accountUIPage.SetActive(!accountUIPage.activeInHierarchy);
                accountUI.InputFieldUIPageController();
                accountUI.SigninUIPageController();
            }
            else
            {
                accountUI.AllAreaClose();
            }
        }
        
    }
}
