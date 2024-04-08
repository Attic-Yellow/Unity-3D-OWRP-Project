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

                for (int i = 0; i < action.bindings.Count && i < 2; i++) // 각 액션별 최대 2개의 바인딩 저장
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
        Debug.Log("저장됨");
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
            print("로드됨");
        }
        else
        {
            print("로드 실패");
        }
    }

    public void UpdateAllBindingButtonTexts()
    {
        // 모든 Keybinding 인스턴스에 대해 UpdateBindingButtonText 호출
        foreach (var keybinding in FindObjectsOfType<Keybinding>(true))
        {
            keybinding.UpdateBindingButtonText();
        }
    }

    public void LoadBindingsAndUpdateUI()
    {
        LoadBindings(); // 바인딩 로드
        UpdateAllBindingButtonTexts(); // UI 업데이트
    }
}
