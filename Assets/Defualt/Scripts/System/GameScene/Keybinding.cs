using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Keybinding : MonoBehaviour
{
    public InputActionAsset actionAsset;
    [SerializeField] private InputAction actionToRebind;
    [SerializeField] private string playerActionMapName = "Player";
    [SerializeField] private string bindName;
    [SerializeField] private TMP_Text bindingButtonText;
    [SerializeField] private int bindingIndex;

    private void Start()
    {
        actionToRebind = actionAsset.FindAction(bindName, true);
        bindingIndex = actionToRebind.GetBindingIndex();
    }

    public void StartRebindingProcess()
    {
        print(bindingIndex);
        if (GameManager.Instance.GetIsRebinding())
        {
            return;
        }

        GameManager.Instance.SetIsRebinding(true);

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
        if (action.bindings.Count > bindingIndex)
        {
            action.RemoveBindingOverride(bindingIndex); // 기존 바인딩 제거
        }

        action.ChangeBinding(bindingIndex).NextCompositeBinding();

        var rebindOperation = action.PerformInteractiveRebinding(bindingIndex)
         // 필요한 경우, 특정 키 조합을 제외시킬 수 있습니다.
         .WithControlsExcluding("Mouse")
         .OnMatchWaitForAnother(0.1f) // 사용자가 실수로 두 번 입력하는 것을 방지
         .OnComplete(operation => operation.Dispose());

        rebindOperation.Start();
    }

    private void OnRebindComplete(InputActionRebindingExtensions.RebindingOperation operation, InputAction action)
    {
        operation.Dispose(); // 재바인딩 작업 리소스 해제

        action.Enable(); // 재바인딩이 완료되면, 액션을 다시 활성화
        bindingButtonText.text = action.GetBindingDisplayString(bindingIndex); // 버튼 텍스트 업데이트
        GameManager.Instance.SetIsRebinding(false); // 게임 매니저 상태 업데이트
    }

    public void UpdateBindingButtonText()
    {
        if (string.IsNullOrEmpty(bindName))
        {
            return; // bindNaem이 비어 있으면 여기서 함수 실행을 중단
        }

        InputAction action = actionAsset.FindAction(bindName, throwIfNotFound: false);
        if (action != null)
        {
            bindingButtonText.text = action.GetBindingDisplayString(bindingIndex);
        }
        else
        {
            bindingButtonText.text = "";
        }
    }

    private IEnumerator Rebinding(InputAction action)
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.LeftAlt))
            {
                yield break;
            }
            else if (Input.anyKeyDown)
            {
                // 이미 존재하는 바인딩 인덱스인지 확인하고, 맞다면 해당 바인딩 업데이트
                if (action.bindings.Count > bindingIndex)
                {
                    action.RemoveBindingOverride(bindingIndex); // 기존 바인딩 제거
                }

                var rebindOperation = action.PerformInteractiveRebinding(bindingIndex)
                    .WithTargetBinding(bindingIndex + 1)
                    .WithControlsExcluding("Mouse") // 마우스 입력은 제외
                    .OnMatchWaitForAnother(0.2f)
                    .OnComplete(operation => OnRebindComplete(operation, action)) // 재바인딩 완료 콜백
                    .Start(); // 재바인딩 시작

                yield break;
            }
        }
        
    }
}
