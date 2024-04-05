using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    private static SceneLoadManager Instance;
    public SceneLoadingUIController loadingUIController;

    [SerializeField] private GameObject loadingUIPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        GameManager.Instance.sceneLoadManager = this;
    }

    // 씬 로드 메서드
    public void LoadScene(string sceneName)
    {
        var target = GameObject.Find("LoadingScene Target");
        var loadingScene = Instantiate(loadingUIPrefab, target.transform);
        loadingUIController = loadingScene.GetComponent<SceneLoadingUIController>();
        loadingUIController.Loading(sceneName);
    }

    public void JoiningServer(int progress)
    {
        var target = GameObject.Find("LoadingScene Target");
        var loadingScene = Instantiate(loadingUIPrefab, target.transform);
        loadingUIController = loadingScene.GetComponent<SceneLoadingUIController>();
        loadingUIController.UpdataLoadingProgress(progress);
    }
}
