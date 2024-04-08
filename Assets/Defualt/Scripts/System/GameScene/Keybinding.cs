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

        // �̹� �����ϴ� ���ε� �ε������� Ȯ���ϰ�, �´ٸ� �ش� ���ε� ������Ʈ
        if (action.bindings.Count > bindingIndex)
        {
            action.RemoveBindingOverride(bindingIndex); // ���� ���ε� ����
        }

        action.ChangeBinding(bindingIndex).NextCompositeBinding();

        var rebindOperation = action.PerformInteractiveRebinding(bindingIndex)
         // �ʿ��� ���, Ư�� Ű ������ ���ܽ�ų �� �ֽ��ϴ�.
         .WithControlsExcluding("Mouse")
         .OnMatchWaitForAnother(0.1f) // ����ڰ� �Ǽ��� �� �� �Է��ϴ� ���� ����
         .OnComplete(operation => operation.Dispose());

        rebindOperation.Start();
    }

    private void OnRebindComplete(InputActionRebindingExtensions.RebindingOperation operation, InputAction action)
    {
        operation.Dispose(); // ����ε� �۾� ���ҽ� ����

        action.Enable(); // ����ε��� �Ϸ�Ǹ�, �׼��� �ٽ� Ȱ��ȭ
        bindingButtonText.text = action.GetBindingDisplayString(bindingIndex); // ��ư �ؽ�Ʈ ������Ʈ
        GameManager.Instance.SetIsRebinding(false); // ���� �Ŵ��� ���� ������Ʈ
    }

    public void UpdateBindingButtonText()
    {
        if (string.IsNullOrEmpty(bindName))
        {
            return; // bindNaem�� ��� ������ ���⼭ �Լ� ������ �ߴ�
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
                // �̹� �����ϴ� ���ε� �ε������� Ȯ���ϰ�, �´ٸ� �ش� ���ε� ������Ʈ
                if (action.bindings.Count > bindingIndex)
                {
                    action.RemoveBindingOverride(bindingIndex); // ���� ���ε� ����
                }

                var rebindOperation = action.PerformInteractiveRebinding(bindingIndex)
                    .WithTargetBinding(bindingIndex + 1)
                    .WithControlsExcluding("Mouse") // ���콺 �Է��� ����
                    .OnMatchWaitForAnother(0.2f)
                    .OnComplete(operation => OnRebindComplete(operation, action)) // ����ε� �Ϸ� �ݹ�
                    .Start(); // ����ε� ����

                yield break;
            }
        }
        
    }
}
