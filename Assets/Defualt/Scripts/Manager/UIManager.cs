using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public StartSceneUI startSceneUI;
    public MainSceneUI mainSceneUI;

    private void Awake()
    {
        GameManager.Instance.uiManager = this;
    }

    // ���� �޼���
    public void OnExitGame()
    {
        Application.Quit();
    }
}
