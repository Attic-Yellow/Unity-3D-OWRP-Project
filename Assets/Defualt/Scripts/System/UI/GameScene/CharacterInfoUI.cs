using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterInfoUI : MonoBehaviour
{
    private string[] characterInfo = { "name", "level", "job" };
    private string[] abilityNames = { "maxHp", "str", "int", "dex", "spi", "vit", "luk", "crt", "dh", "det", "def", "mdf", "pap", "map", "sks", "mhp", "sps", "ten", "pie" };

    [SerializeField] private GameObject characterInfoUI;

    [Header("Character Info")]
    [SerializeField] private List<TextMeshProUGUI> characterInfoText;
    [SerializeField] private List<TextMeshProUGUI> abilitesText;

    private void Awake()
    {
        GameManager.Instance.uiManager.gameSceneUI.characterInfoUI = this;
    }

    private void Start()
    {
        var characterData = GameManager.Instance.dataManager.characterData.characterData;

        if (characterInfoUI != null)
        {
            characterInfoUI.SetActive(false);
        }

        if (characterInfoText.Count > 0)
        {
            for (int i = 0; i < characterInfo.Length; i++)
            {
                characterInfoText[i].text = characterData.ContainsKey(characterInfo[i]) ? characterData[characterInfo[i]].ToString() : "null";
            }
        }

        if (abilitesText.Count > 0)
        {
            for (int i = 0; i < abilityNames.Length; i++)
            {
                abilitesText[i].text = characterData.ContainsKey(abilityNames[i]) ? characterData[abilityNames[i]].ToString() : "null";
            }
        }
    }

    public void CharacterInfoUIController()
    {
        if (characterInfoUI != null)
        {
            characterInfoUI.SetActive(!characterInfoUI.activeInHierarchy);
        }
    }
}
