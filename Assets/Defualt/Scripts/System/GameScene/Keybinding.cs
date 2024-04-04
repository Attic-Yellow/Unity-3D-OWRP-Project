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

        action.ApplyBindingOverride(bindingsIndex, "<Keyboard>/space");
        //if (bindingsIndex != 0)
        //{
        //    // 기존 바인딩이 아닌 새 바인딩을 추가하는 경우
        //    action.AddBinding("<Keyboard>/#(any)");
        //}

        //// 재바인딩 수행
        //var rebindOperation = action.PerformInteractiveRebinding(bindingsIndex)
        //    .WithControlsExcluding("Mouse") // 마우스 제외
        //    .OnMatchWaitForAnother(0.1f) // 실수로 두 번 입력하는 것 방지
        //    .OnComplete(operation => OnRebindComplete(operation, action)) // 재바인딩 완료 후 처리
        //    .Start();

        //// 새 바인딩 추가의 경우, 바인딩 인덱스 업데이트 필요
        //if (bindingsIndex != 0)
        //{
        //    bindingsIndex = action.bindings.Count - 1;
        //}
    }

    private void OnRebindComplete(InputActionRebindingExtensions.RebindingOperation operation, InputAction action)
    {
        // 재바인딩이 완료되면, 액션을 다시 활성화
        action.Enable();
        bindingButtonText.text = action.bindings[bindingsIndex].ToDisplayString();
        GameManager.Instance.SetIsRebinding(false);
    }
}
