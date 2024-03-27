using Firebase.Auth;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectedUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> characterSelectedArea;
    [SerializeField] private GameObject characterButtonPrefab;
    [SerializeField] private List<GameObject> characterButton;

    private int serverNumber;

    private void Awake()
    {
        GameManager.Instance.uiManager.mainSceneUI.chracterSelectedUI = this;
    }

    private async void Start()
    {
        await CharacterSelecteAreaController(0);
    }

    /*** Page �� Area�� Active ��Ʈ�ѷ� �޼��� ***/

    // ĳ���� ���� ���� ��Ʈ�ѷ� �޼���
    private async Task CharacterSelecteAreaController(int serverNum)
    {
        var user = FirebaseAuth.DefaultInstance.CurrentUser;

        for (int i = 0; i < characterSelectedArea.Count; i++)
        {
            if (characterSelectedArea[i].activeInHierarchy)
            {
                characterSelectedArea[i].SetActive(false);
            }
        }

        switch (serverNum)
        {
            case 0:
                characterSelectedArea[0].SetActive(true);
                await GameManager.Instance.firebaseManager.LoadCharacter(user.UserId, OnCharacterLoaded);
                break;
            case 1:
                characterSelectedArea[1].SetActive(true);
                break;
            case 2:
                characterSelectedArea[2].SetActive(true);
                break;

        }
    }

    /*** ��ư �޼��� ***/

    // ���� ��ư Ŭ�� �޼���
    public async void OnServerButtonClick(int serverNum)
    {
        serverNumber = serverNum;
        await CharacterSelecteAreaController(serverNum);
        serverNumber = -1;
    }

    // ĳ���� ��ư Ŭ�� �޼���
    public void OnCharacterButtonClick()
    {

    }

    // ĳ���� ���� ��ư Ŭ�� �޼���
    public void OnCreateButtonClick()
    {
        GameManager.Instance.uiManager.mainSceneUI.characterCreateUI.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    // ĳ���� �ε� �Ϸ� �� ȣ��� �޼���
    private void OnCharacterLoaded(List<Dictionary<string, object>> charactersData)
    {
        if (charactersData == null || charactersData.Count == 0)
        {
            print("ĳ���� �ε� �����Ͽ��ų�, �ε��� ĳ���Ͱ� �����ϴ�.");
            return; // ĳ���Ͱ� ������ ������ �� �̻� �������� �ʽ��ϴ�.
        }

        // ������ characterSelectedArea�� �ִ� �ڽ� ������Ʈ���� ��� ����
        foreach (Transform child in characterSelectedArea[serverNumber].transform)
        {
            Destroy(child.gameObject);
        }

        // ���ο� ĳ���� ���� ������ �ν��Ͻ��� �����ϰ� characterSelectedArea�� �߰�
        foreach (var characterData in charactersData)
        {
            if (characterData.TryGetValue("name", out var characterName))
            {
                // ������ �ν��Ͻ�ȭ
                var instance = Instantiate(characterButtonPrefab, characterSelectedArea[serverNumber].transform);

                // �ν��Ͻ�ȭ�� �����տ� ĳ���� ���� ����
                // ��: ������ ���� Text ������Ʈ ã�Ƽ� ĳ���� �̸� ����
                var textComponent = instance.GetComponentInChildren<TextMeshProUGUI>();
                if (textComponent != null)
                {
                    textComponent.text = characterName.ToString();
                }
            }
        }
    }
}
