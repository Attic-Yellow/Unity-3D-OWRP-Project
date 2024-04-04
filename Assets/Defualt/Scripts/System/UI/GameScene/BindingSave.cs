using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BindingSave : MonoBehaviour
{
    public InputActionAsset actionAsset;

    private void Awake()
    {
        LoadBindings();
    }

    public void SaveBindings()
    {
        foreach (var map in actionAsset.actionMaps)
        {
            foreach (var binding in map.bindings)
            {
                if (!string.IsNullOrEmpty(binding.id.ToString()))
                {
                    PlayerPrefs.SetString(binding.id.ToString(), binding.overridePath);
                }
            }
        }
        PlayerPrefs.Save();
    }

    public void LoadBindings()
    {
        foreach (var map in actionAsset.actionMaps)
        {
            foreach (var binding in map.bindings)
            {
                if (PlayerPrefs.HasKey(binding.id.ToString()))
                {
                    var overridePath = PlayerPrefs.GetString(binding.id.ToString());

                    map.ApplyBindingOverride(new InputBinding { id = binding.id, overridePath = overridePath });
                }
            }
        }
    }
}
