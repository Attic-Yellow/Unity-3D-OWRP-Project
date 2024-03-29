using Firebase.Auth;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterButton : MonoBehaviour
{
    [SerializeField] private GameObject characterButton;
    [SerializeField] private GameObject optionButton; 
    
    private float lastClickTime = 0f; // ������ Ŭ�� �ð��� ����
    private const float doubleClickThreshold = 0.25f; // ���� Ŭ������ ���ֵǴ� �ð�(�� ����)

    public Dictionary<string, object> CharacterData { get; private set; }

    private void Start()
    {
        characterButton.GetComponent<Button>().onClick.AddListener(() => OnCharacterButtonClick());
        CharacterNameUpdate();
    }

    private void OnCharacterButtonClick()
    {
        float timeSinceLastClick = Time.time - lastClickTime;
        lastClickTime = Time.time;

        // ���� ���� ���� �Ǵ� ���� ���� ó��
        if (timeSinceLastClick <= doubleClickThreshold) // ���� Ŭ������ ���ֵǴ� ���
        {
            if (CharacterData != null) 
            {
                GameManager.Instance.dataManager.characterData.SetCharacterData(CharacterData);
                string serverName = CharacterData.ContainsKey("server") ? CharacterData["server"].ToString() : "server1";
                string nickName = CharacterData.ContainsKey("name") ? CharacterData["name"].ToString() : string.Empty;
                GameManager.Instance.photonManager.CreateOrJoinRoom(serverName, nickName);
            }
                
        }
        else // ���� Ŭ������ ���ֵǴ� ���
        {

        }
    }

    public void SetCharacterData(Dictionary<string, object> characterData)
    {
        this.CharacterData = characterData;
    }

    private void CharacterNameUpdate()
    {
        characterButton.GetComponentInChildren<TextMeshProUGUI>().text = CharacterData["name"].ToString();
    }
}
