using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreateUI : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.uiManager.mainSceneUI.characterCreateUI = this;
    }
}
