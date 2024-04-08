using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

public class Keybinding : MonoBehaviour
{
    public InputActionAsset actionAsset;
    [SerializeField] private InputAction actionToRebind;
    [SerializeField] private string bindName;
    [SerializeField] private TMP_Text bindingButtonText;
    [SerializeField] private int bindingIndex;

    private bool isComposite = false;

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
            StartCoroutine(RebindKey());
        }
        else
        {
            print("Action to rebind not found.");
        }
    }

    private IEnumerator RebindKey()
    {
        bool isComposite = false;
        string firstBinding = "";

        // ù ��° Ű ���ε�
        yield return StartCoroutine(RebindingCoroutine(actionToRebind, bindingIndex + 1, (binding) => {
            firstBinding = binding;
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftAlt))
            {
                isComposite = true;
            }
        }));

        if (isComposite)
        {
            // ���� Ű�� ���, �� ��° Ű ���ε� ����
            yield return StartCoroutine(RebindingCoroutine(actionToRebind, bindingIndex + 2, _ => { }));
        }
        else
        {
            // ���� Ű�� ���, ù ��° ���ε��� �� ��° Ű���� ����
            actionToRebind.ApplyBindingOverride(bindingIndex + 1, firstBinding);
            actionToRebind.ApplyBindingOverride(bindingIndex + 2, firstBinding);
        }

        print(actionToRebind.GetBindingDisplayString(bindingIndex + 1));
        print(actionToRebind.GetBindingDisplayString(bindingIndex + 2));

        GameManager.Instance.SetIsRebinding(false);
        UpdateBindingButtonText();
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
            if (action.GetBindingDisplayString(bindingIndex + 1) == action.GetBindingDisplayString(bindingIndex + 2))
            {
                bindingButtonText.text = action.GetBindingDisplayString(bindingIndex + 1);
            }
            else
            {
                bindingButtonText.text = $"{action.GetBindingDisplayString(bindingIndex + 1)} + {action.GetBindingDisplayString(bindingIndex + 2)}";
            }
        }
        else
        {
            bindingButtonText.text = "";
        }
    }

    private IEnumerator RebindingCoroutine(InputAction action, int targetBindingIndex, Action<string> onBindingComplete)
    {
        action.Disable();
        var rebindOperation = action.PerformInteractiveRebinding(targetBindingIndex)
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete((op) => {
                if (op.selectedControl != null)
                {
                    // ���õ� ��Ʈ���� ������, �ش� ��θ� ��ȯ
                    onBindingComplete(op.selectedControl.path);
                }
                else
                {
                    // ���õ� ��Ʈ���� ������, null�� ��ȯ�Ͽ� �̸� ó���� �� �ֵ��� ��
                    onBindingComplete(null);
                }
            })
            .Start();

        yield return new WaitUntil(() => rebindOperation.completed);

        rebindOperation.Dispose();
        action.Enable();
    }
}
