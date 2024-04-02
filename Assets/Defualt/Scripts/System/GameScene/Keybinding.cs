using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Keybinding : MonoBehaviour
{
    public InputActionAsset actionAsset;
    private InputActionMap playerActionMap;

    void Start()
    {
        playerActionMap = actionAsset.FindActionMap("PlayerKey");

        // 기존 바인딩 로드 (예시)
        string rebinds = PlayerPrefs.GetString("rebinds", string.Empty);
        if (!string.IsNullOrEmpty(rebinds))
        {
            actionAsset.LoadBindingOverridesFromJson(rebinds);
        }
    }

    public void RebindKey(InputAction action)
    {
        action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse") // 마우스 제외
            .OnMatchWaitForAnother(0.1f) // 실수로 두 번 입력하는 것 방지
            .OnComplete(operation => SaveRebinds()) // 리바인드 완료 후 저장
            .Start();
    }

    void SaveRebinds()
    {
        // 변경 사항 저장
        string rebinds = actionAsset.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
        PlayerPrefs.Save();
    }
}
