using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CharacterInfoUI : MonoBehaviour
{
    

    private string[] characterInfo = { "name", "level", "job" };
    private string[] abilityNames = { "maxHp", "str", "int", "dex", "spi", "vit", "luk", "crt", "dh", "det", "def", "mdf", "pap", "map", "sks", "mhp", "sps", "ten", "pie" };

    [SerializeField] private GameObject characterInfoUI;
    [SerializeField] private Transform jobIconTransform;
    [SerializeField] private List<GameObject> jobIconPrefabs;

    [Header("Character Info")]
    [SerializeField] private List<TextMeshProUGUI> characterInfoText;
    [SerializeField] private List<TextMeshProUGUI> abilitesText;
    [SerializeField] private List<CurrentEquippedSlot> currentEquippedSlots;

    [SerializeField] private CurrentEquipped currentEquipped;

    private void Awake()
    {
        GameManager.Instance.uiManager.gameSceneUI.characterInfoUI = this;
    }

    private void Start()
    {
        currentEquipped = CurrentEquipped.Instance;
        currentEquipped.onChangeEquipp += ReadrawSlotUI;
        var characterData = GameManager.Instance.dataManager.characterData.characterData;

        if (characterInfoUI != null)
        {
            characterInfoUI.SetActive(false);
        }

        if (characterInfoText.Count > 0)
        {
            for (int i = 0; i < characterInfo.Length; i++)
            {
                if (i == 2)
                {
                    string job = characterData.ContainsKey(characterInfo[i]) ? characterData[characterInfo[i]].ToString() : "null";
                    switch (job)
                    {
                        case "Warrior":
                            characterInfoText[i].text = "전사";
                            break;
                        case "Drgoon":
                            characterInfoText[i].text = "용기사";
                            break;
                        case "Bard":
                            characterInfoText[i].text = "음유시인";
                            break;
                        case "WhiteMage":
                            characterInfoText[i].text = "백마도사";
                            break;
                        case "BlackMage":
                            characterInfoText[i].text = "흑마도사";
                            break;
                    }
                }
                else
                {
                    characterInfoText[i].text = characterData.ContainsKey(characterInfo[i]) ? characterData[characterInfo[i]].ToString() : "null";
                }
            }
        }

        if (abilitesText.Count > 0)
        {
            for (int i = 0; i < abilityNames.Length; i++)
            {
                abilitesText[i].text = characterData.ContainsKey(abilityNames[i]) ? characterData[abilityNames[i]].ToString() : "null";
            }
        }

        if (jobIconTransform != null)
        {
            string jobName = characterData.ContainsKey("job") ? characterData[characterInfo[2]].ToString() : "Warrior";
            Job job = (Job)Enum.Parse(typeof(Job), jobName);
            int jobNumber = (int)job;
            Instantiate(jobIconPrefabs[jobNumber], jobIconTransform.transform);
        }
    }

    public void CharacterInfoUIController()
    {
        if (characterInfoUI != null)
        {
            characterInfoUI.SetActive(!characterInfoUI.activeInHierarchy);

            if (characterInfoUI.activeSelf)
            {
                characterInfoUI.transform.SetAsLastSibling();
            }
        }
    }

    public void ReadrawSlotUI()
    {
        for (int i = 1; i < currentEquippedSlots.Count; i++)
        {
            currentEquippedSlots[i].ClearSlot();

            if (i < currentEquipped.currentEquippeds.Count && currentEquipped.currentEquippeds[i] != null)
            {
                currentEquippedSlots[i].equipment = currentEquipped.currentEquippeds[i];
                currentEquippedSlots[i].UpdateSlotUI();
            }
            else
            {
                currentEquippedSlots[i].ClearSlot();
            }
        }
    }
}
