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

    // 종료 메서드
    public void OnExitGame()
    {
        Application.Quit();
    }
}
