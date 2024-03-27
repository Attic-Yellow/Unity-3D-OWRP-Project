using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public StartSceneUI startSceneUI;

    private void Awake()
    {
        GameManager.Instance.uiManager = this;
    }


}
