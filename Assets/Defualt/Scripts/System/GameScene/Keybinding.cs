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
        // Ư�� InputAction ã��
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

        // �̹� �����ϴ� ���ε� �ε������� Ȯ���ϰ�, �´ٸ� �ش� ���ε� ������Ʈ
        if (action.bindings.Count > 1 + bindingsIndex)
        {
            action.RemoveBindingOverride(bindingsIndex); // ���� ���ε� ����
        }

        var rebindOperation = action.PerformInteractiveRebinding(bindingsIndex)
            .WithControlsExcluding("Mouse") // ���콺 �Է��� ���� (����)
            .OnMatchWaitForAnother(0.2f)
            .OnComplete(operation => OnRebindComplete(operation, action)) // ����ε� �Ϸ� �ݹ�
            .Start(); // ����ε� ����
    }

    private void OnRebindComplete(InputActionRebindingExtensions.RebindingOperation operation, InputAction action)
    {
        operation.Dispose(); // ����ε� �۾� ���ҽ� ����

        action.Enable(); // ����ε��� �Ϸ�Ǹ�, �׼��� �ٽ� Ȱ��ȭ
        bindingButtonText.text = action.bindings[bindingsIndex].ToDisplayString(); // ��ư �ؽ�Ʈ ������Ʈ
        GameManager.Instance.SetIsRebinding(false); // ���� �Ŵ��� ���� ������Ʈ
    }

    public void UpdateBindingButtonText()
    {
        if (string.IsNullOrEmpty(bindNaem))
        {
            return; // bindNaem�� ��� ������ ���⼭ �Լ� ������ �ߴ�
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
