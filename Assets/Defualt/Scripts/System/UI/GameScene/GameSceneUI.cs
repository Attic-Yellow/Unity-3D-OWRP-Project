using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneUI : MonoBehaviour
{
    public CharacterInfoUI characterInfoUI;

    private void Awake()
    {
        GameManager.Instance.uiManager.gameSceneUI = this;
    }
}
