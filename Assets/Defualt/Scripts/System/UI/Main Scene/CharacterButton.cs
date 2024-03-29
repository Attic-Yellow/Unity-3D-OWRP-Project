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
    
    private float lastClickTime = 0f; // 마지막 클릭 시간을 추적
    private const float doubleClickThreshold = 0.25f; // 더블 클릭으로 간주되는 시간(초 단위)

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

        if (timeSinceLastClick <= doubleClickThreshold) // 더블 클릭으로 간주되는 경우
        {
            if (CharacterData != null) 
            {
                GameManager.Instance.dataManager.characterData.SetCharacterData(CharacterData);
                string serverName = CharacterData.ContainsKey("server") ? CharacterData["server"].ToString() : "server1";
                string nickName = CharacterData.ContainsKey("name") ? CharacterData["name"].ToString() : string.Empty;
                GameManager.Instance.photonManager.CreateOrJoinRoom(serverName, nickName);
            }
                
        }
        else // 단일 클릭으로 간주되는 경우
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
