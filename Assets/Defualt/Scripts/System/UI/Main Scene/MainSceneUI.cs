using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneUI : MonoBehaviour
{
    public CharacterSelectedUI characterSelectedUI;
    public CharacterCreateUI characterCreateUI;
    [SerializeField] private GameObject selectedPage;
    [SerializeField] private GameObject createPage;


    private void Awake()
    {
        GameManager.Instance.uiManager.mainSceneUI = this;
    }

    private void Start()
    {
        MainSceneInit();
    }

    public void MainSceneInit()
    {
        selectedPage.SetActive(true);
        createPage.SetActive(false);
    }

    public void MainScenePageController()
    {
        selectedPage.SetActive(false);
        createPage.SetActive(true);
    }
}
