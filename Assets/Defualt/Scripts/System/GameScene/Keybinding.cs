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

        action.ApplyBindingOverride(bindingsIndex, "<Keyboard>/space");
        //if (bindingsIndex != 0)
        //{
        //    // ���� ���ε��� �ƴ� �� ���ε��� �߰��ϴ� ���
        //    action.AddBinding("<Keyboard>/#(any)");
        //}

        //// ����ε� ����
        //var rebindOperation = action.PerformInteractiveRebinding(bindingsIndex)
        //    .WithControlsExcluding("Mouse") // ���콺 ����
        //    .OnMatchWaitForAnother(0.1f) // �Ǽ��� �� �� �Է��ϴ� �� ����
        //    .OnComplete(operation => OnRebindComplete(operation, action)) // ����ε� �Ϸ� �� ó��
        //    .Start();

        //// �� ���ε� �߰��� ���, ���ε� �ε��� ������Ʈ �ʿ�
        //if (bindingsIndex != 0)
        //{
        //    bindingsIndex = action.bindings.Count - 1;
        //}
    }

    private void OnRebindComplete(InputActionRebindingExtensions.RebindingOperation operation, InputAction action)
    {
        // ����ε��� �Ϸ�Ǹ�, �׼��� �ٽ� Ȱ��ȭ
        action.Enable();
        bindingButtonText.text = action.bindings[bindingsIndex].ToDisplayString();
        GameManager.Instance.SetIsRebinding(false);
    }
}
