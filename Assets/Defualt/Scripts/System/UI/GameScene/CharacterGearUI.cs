using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGearUI : MonoBehaviour
{
    [SerializeField] private GameObject characterGearUI;
    [SerializeField] private List<GameObject> characterGears;
    [SerializeField] private List<EquippedSlot> weaponSlot;
    [SerializeField] private List<EquippedSlot> headSlot;
    [SerializeField] private List<EquippedSlot> bodySlot;
    [SerializeField] private List<EquippedSlot> handsSlot;
    [SerializeField] private List<EquippedSlot> legsSlot;
    [SerializeField] private List<EquippedSlot> feetSlot;
    [SerializeField] private List<EquippedSlot> auxiliarySlot;
    [SerializeField] private List<EquippedSlot> earringSlot;
    [SerializeField] private List<EquippedSlot> necklaceSlot;
    [SerializeField] private List<EquippedSlot> braceletSlot;
    [SerializeField] private List<EquippedSlot> ringSlot;

    private void Awake()
    {
        GameManager.Instance.uiManager.gameSceneUI.characterGearUI = this;
    }

    private void Start()
    {
        if (characterGearUI != null)
        {
            characterGearUI.SetActive(false);
        }

        if (characterGears.Count > 0)
        {
            GearsController(0);
        }
    }

    public void GearUIController()
    {
        if (characterGearUI != null)
        {
            characterGearUI.SetActive(!characterGearUI.activeInHierarchy);

            if (characterGearUI.activeSelf)
            {
                characterGearUI.transform.SetAsLastSibling();
            }
        }
    }

    public void GearsController(int index)
    {
        if (characterGears.Count > 0)
        {
            for (int i = 0; i < characterGears.Count; i++)
            {
                characterGears[i].gameObject.SetActive(i == index);
            }
        }
    }
}
