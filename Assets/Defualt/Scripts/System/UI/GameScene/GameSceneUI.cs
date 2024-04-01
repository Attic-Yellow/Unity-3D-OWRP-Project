using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneUI : MonoBehaviour
{
    public CharacterInfoUI characterInfoUI;
    public CharacterGearUI characterGearUI;
    public InventoryUI inventoryUI;

    private void Awake()
    {
        GameManager.Instance.uiManager.gameSceneUI = this;
    }

    private void Update()
    {
        UIKeyController();
    }

    private void UIKeyController()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            characterInfoUI.CharacterInfoUIController();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            characterGearUI.GearUIController();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryUI.InventoryController();
        }
    }
}
