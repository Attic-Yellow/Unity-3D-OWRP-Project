using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> uiAreas;
    [SerializeField] private GameObject inputFieldUIArea;

    [SerializeField] private int currentAreaIndex = 0;

    private void Awake()
    {
        GameManager.Instance.uiManager.startSceneUI.accountUI = this;
    }

    private void Start()
    {
        if (inputFieldUIArea != null)
        {
            inputFieldUIArea.SetActive(false);
        }

        if (uiAreas.Count > 0)
        {
            Init();
        }
    }

    public void Init()
    {
        for (int i = 0; i < uiAreas.Count; i++)
        {
            uiAreas[i].SetActive(false);
        }
    }    

    /*** UI Area Ȱ��ȭ ��Ȱ��ȭ ***/

    // ���� UI Ȱ��ȭ/��Ȱ��ȭ ��Ʈ�ѷ� �޼���
    public void InputFieldUIController(int index)
    {
        if (index == 0 || index == 1)
        {
            inputFieldUIArea.SetActive(true);
        }
        else
        {
            inputFieldUIArea.SetActive(false);
        }
    }

    // ���� UI ���� ��Ʈ�ѷ�
    public void UIAreaIndexController(int index)
    {
        for (int i = 0; i < uiAreas.Count; i++)
        {
            uiAreas[i].SetActive(i == index);
        }
    }

    /*** ��ư Ŭ�� �� ��� ***/

    // �ڷΰ��� ��ư
    public void OnBackButtonClick()
    {
        if (currentAreaIndex == 0)
        {
            GameManager.Instance.uiManager.startSceneUI.accountUIPageController();
            GameManager.Instance.uiManager.startSceneUI.AccountButtonController();
            Init();
            return;
        }
        currentAreaIndex--;
        InputFieldUIController(currentAreaIndex);
        UIAreaIndexController(currentAreaIndex);
    }

    // �α��� ��ư 
    public void OnSigninButtonClick()
    {
        AccountSystem accountSystem = FindAnyObjectByType<AccountSystem>();
        accountSystem.OnSigninButtonCallBack();
    }

    // ȸ�� ���� ��ư
    public void OnSignupButtonClick(int index)
    {
        if (index == 2)
        {
            AccountSystem accountSystem = FindAnyObjectByType<AccountSystem>();
            accountSystem.OnSignupButtonCallBack();
        }
        currentAreaIndex = index;
        InputFieldUIController(index);
        UIAreaIndexController(index);
    }

    // ��� ��ȣ ã�� ��ư
    public void OnForgottenPWButtonClick()
    {

    }

    // ���� �Ϸ� ��ư
    public void OnCompleteEVButtonClick()
    {
        StartCoroutine(EVCallBackCoroutine());
        Init();
    }

    private IEnumerator EVCallBackCoroutine()
    {
        AccountSystem accountSystem = FindAnyObjectByType<AccountSystem>();
        accountSystem.OnCompleteEVButtonCallBack();
        yield return new WaitUntil(() => GameManager.Instance.GetIsSignInSuccess());
    }
}
