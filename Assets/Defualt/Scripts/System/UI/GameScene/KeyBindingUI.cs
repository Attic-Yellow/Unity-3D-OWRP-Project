using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBindingUI : MonoBehaviour
{
    [SerializeField] private GameObject keyBindingUI;
    [SerializeField] private List<GameObject> bindingAreas;

    private void Awake()
    {
        GameManager.Instance.uiManager.gameSceneUI.keyBindingUI = this;
    }

    private void Start()
    {
        if (keyBindingUI != null)
        {
            keyBindingUI.SetActive(false);
        }

        bindingAreaController(0);
    }

    public void keyBindingUIController()
    {
        if (keyBindingUI != null)
        {
            keyBindingUI.SetActive(!keyBindingUI.activeInHierarchy);
            
            if (keyBindingUI.activeSelf)
            {
                keyBindingUI.transform.SetAsLastSibling();
            }
        }
    }

    public void bindingAreaController(int index)
    {
        if (bindingAreas.Count > 0)
        {
            for (int i = 0; i < bindingAreas.Count; i++)
            {
                bindingAreas[i].SetActive(i == index);
            }
        }
    }
}
