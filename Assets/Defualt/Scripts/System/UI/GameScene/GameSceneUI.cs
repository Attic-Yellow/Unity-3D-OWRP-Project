using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameSceneUI : MonoBehaviour
{
    public CharacterInfoUI characterInfoUI;
    public CharacterGearUI characterGearUI;
    public InventoryUI inventoryUI;
    public MenuButtonUI menuButtonsUI;
    public KeyBindingUI keyBindingUI;

    private void Awake()
    {
        GameManager.Instance.uiManager.gameSceneUI = this;
    }
}
