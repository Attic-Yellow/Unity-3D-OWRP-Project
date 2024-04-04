using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Keybinding : MonoBehaviour
{
    public InputActionAsset actionAsset;
    [SerializeField] private string playerActionMapName = "Player";
    [SerializeField] private string bindNaem;
    [SerializeField] private TMP_Text bindingButtonText;
    [SerializeField] private int bindingsIndex;

    public void StartRebindingProcess()
    {
        if (GameManager.Instance.GetIsRebinding())
        {
            return;
        }

        GameManager.Instance.SetIsRebinding(true);
        // 특정 InputAction 찾기
        InputAction actionToRebind = actionAsset.FindAction(bindNaem, true);
        if (actionToRebind != null)
        {
            RebindKey(actionToRebind);
        }
        else
        {
            print("Action to rebind not found.");
        }
    }

    public void RebindKey(InputAction action)
    {
        action.Disable();

        // 이미 존재하는 바인딩 인덱스인지 확인하고, 맞다면 해당 바인딩 업데이트
        if (action.bindings.Count > 1 + bindingsIndex)
        {
            action.RemoveBindingOverride(bindingsIndex); // 기존 바인딩 제거
        }

        var rebindOperation = action.PerformInteractiveRebinding(bindingsIndex)
            .WithControlsExcluding("Mouse") // 마우스 입력은 제외 (예시)
            .OnMatchWaitForAnother(0.2f)
            .OnComplete(operation => OnRebindComplete(operation, action)) // 재바인딩 완료 콜백
            .Start(); // 재바인딩 시작
    }

    private void OnRebindComplete(InputActionRebindingExtensions.RebindingOperation operation, InputAction action)
    {
        operation.Dispose(); // 재바인딩 작업 리소스 해제

        action.Enable(); // 재바인딩이 완료되면, 액션을 다시 활성화
        bindingButtonText.text = action.bindings[bindingsIndex].ToDisplayString(); // 버튼 텍스트 업데이트
        GameManager.Instance.SetIsRebinding(false); // 게임 매니저 상태 업데이트
    }

    public void UpdateBindingButtonText()
    {
        if (string.IsNullOrEmpty(bindNaem))
        {
            return; // bindNaem이 비어 있으면 여기서 함수 실행을 중단
        }

        InputAction action = actionAsset.FindAction(bindNaem, throwIfNotFound: false);
        if (action != null)
        {
            bindingButtonText.text = action.bindings[bindingsIndex].ToDisplayString();
        }
        else
        {
            bindingButtonText.text = "";
        }
    }
}
