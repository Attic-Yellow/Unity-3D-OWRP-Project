using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

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

    /*** UI Area 활성화 비활성화 ***/

    // 계정 UI 활성화/비활성화 컨트롤러 메서드
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

    // 계정 UI 영역 컨트롤러
    public void UIAreaIndexController(int index)
    {
        for (int i = 0; i < uiAreas.Count; i++)
        {
            uiAreas[i].SetActive(i == index);
        }
    }

    /*** 버튼 클릭 시 기능 ***/

    // 뒤로가기 버튼
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

    // 로그인 버튼 
    public void OnSigninButtonClick()
    {
        AccountSystem accountSystem = FindAnyObjectByType<AccountSystem>();
        accountSystem.OnSigninButtonCallBack();
    }

    // 회원 가입 버튼
    public void OnSignupButtonClick(int index)
    {
        if (index == 2)
        {
            AccountSystem accountSystem = FindAnyObjectByType<AccountSystem>();
            GameManager.Instance.uiManager.startSceneUI.LoadingAreaController(); // 로딩 화면 활성화
            accountSystem.OnSignupButtonCallBack();
        }
        currentAreaIndex = index;
        InputFieldUIController(index);
        UIAreaIndexController(index);
    }

    // 비밀 번호 찾기 버튼
    public void OnForgottenPWButtonClick()
    {

    }

    // 인증 완료 버튼
    public void OnCompleteEVButtonClick()
    {
        StartCoroutine(EVCallBackCoroutine());
    }

    private IEnumerator EVCallBackCoroutine()
    {
        AccountSystem accountSystem = FindAnyObjectByType<AccountSystem>();
        accountSystem.OnCompleteEVButtonCallBack();
        yield return new WaitUntil(() => GameManager.Instance.GetIsSignInSuccess());

        Init();
    }

    public int GetCurrentAreaIndex()
    {
        return currentAreaIndex;
    }
}
