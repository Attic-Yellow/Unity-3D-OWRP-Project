using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BindingSave : MonoBehaviour
{
    public InputActionAsset actionAsset;

    private void Awake()
    {
        LoadBindingsAndUpdateUI();
    }

    public void SaveBindings()
    {
        var bindingsDictionary = new Dictionary<string, List<string>>();

        foreach (var actionMap in actionAsset.actionMaps)
        {
            foreach (var action in actionMap)
            {
                var bindingList = new List<string>();

                for (int i = 0; i < action.bindings.Count && i < 2; i++) // �� �׼Ǻ� �ִ� 2���� ���ε� ����
                {
                    if (!string.IsNullOrEmpty(action.bindings[i].overridePath))
                    {
                        bindingList.Add(action.bindings[i].overridePath);
                    }
                }

                if (bindingList.Count > 0)
                {
                    bindingsDictionary[action.id.ToString()] = bindingList;
                }
            }
        }

        string bindingsJson = JsonConvert.SerializeObject(bindingsDictionary);
        PlayerPrefs.SetString("actionAsset_bindings", bindingsJson);
        PlayerPrefs.Save();
        Debug.Log("�����");
    }

    public void LoadBindings()
    {
        string bindingsJson = PlayerPrefs.GetString("actionAsset_bindings", string.Empty);
        if (!string.IsNullOrEmpty(bindingsJson))
        {
            var bindingsDictionary = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(bindingsJson);
            foreach (var actionMap in actionAsset.actionMaps)
            {
                foreach (var action in actionMap)
                {
                    if (bindingsDictionary.TryGetValue(action.id.ToString(), out var bindingList))
                    {
                        for (int i = 0; i < bindingList.Count && i < action.bindings.Count; i++)
                        {
                                action.ApplyBindingOverride(bindingList[i]);
                        }
                    }
                }
            }
            Debug.Log("�ε��");
        }
        else
        {
            Debug.Log("�ε� ����");
        }
    }

    public void UpdateAllBindingButtonTexts()
    {
        // ��� Keybinding �ν��Ͻ��� ���� UpdateBindingButtonText ȣ��
        foreach (var keybinding in FindObjectsOfType<Keybinding>(true))
        {
            keybinding.UpdateBindingButtonText();
        }
    }

    public void LoadBindingsAndUpdateUI()
    {
        LoadBindings(); // ���ε� �ε�
        UpdateAllBindingButtonTexts(); // UI ������Ʈ
    }
}
