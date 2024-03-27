using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.sceneLoadManager = this;
    }

    // 씬 로드 메서드
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
