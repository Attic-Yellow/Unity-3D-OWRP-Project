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

        // ���� ���ε� �ε� (����)
        string rebinds = PlayerPrefs.GetString("rebinds", string.Empty);
        if (!string.IsNullOrEmpty(rebinds))
        {
            actionAsset.LoadBindingOverridesFromJson(rebinds);
        }
    }

    public void RebindKey(InputAction action)
    {
        action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse") // ���콺 ����
            .OnMatchWaitForAnother(0.1f) // �Ǽ��� �� �� �Է��ϴ� �� ����
            .OnComplete(operation => SaveRebinds()) // �����ε� �Ϸ� �� ����
            .Start();
    }

    void SaveRebinds()
    {
        // ���� ���� ����
        string rebinds = actionAsset.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
        PlayerPrefs.Save();
    }
}
