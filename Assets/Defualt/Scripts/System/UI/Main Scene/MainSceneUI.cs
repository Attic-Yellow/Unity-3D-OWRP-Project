using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneUI : MonoBehaviour
{
    public CharacterSelectedUI chracterSelectedUI;
    public CharacterCreateUI characterCreateUI;

    private void Awake()
    {
        GameManager.Instance.uiManager.mainSceneUI = this;
    }
}
