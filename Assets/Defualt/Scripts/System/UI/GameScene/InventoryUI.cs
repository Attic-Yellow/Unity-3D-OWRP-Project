using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private List<GameObject> inventorySlotAreas;
    [SerializeField] private List<InventorySlot> inventorySlots;

    [SerializeField] private Inventory inventory;

    private void Awake()
    {
        GameManager.Instance.uiManager.gameSceneUI.inventoryUI = this;
    }

    private void Start()
    {
        inventory = Inventory.Instance;
        inventory.onChangeItem += ReadrawSlotUI;
        for (int i = 0; i < inventorySlotAreas.Count; i++)
        {
            foreach (var slot in inventorySlotAreas[i].GetComponentsInChildren<InventorySlot>())
            {
                inventorySlots.Add(slot);
            }
        }

        if (inventoryUI != null)
        {
            inventoryUI.SetActive(false);
        }

        InventorySlotsAreasController(0);
    }

    public void InventoryController()
    {
        if (inventoryUI != null)
        {
            inventoryUI.SetActive(!inventoryUI.activeInHierarchy);

            if (inventoryUI.activeSelf)
            {
                inventoryUI.transform.SetAsLastSibling();
            }
        }
    }

    public void InventorySlotsAreasController(int index)
    {
        if (inventorySlotAreas.Count > 0)
        {
            for (int i = 0; i < inventorySlotAreas.Count; i++)
            {
                inventorySlotAreas[i].SetActive(i == index);
            }
        }
    }

    public void ReadrawSlotUI()
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            inventorySlots[i].ClearSlot();
        }
        for (int i = 0; i < inventory.items.Count; i++)
        {
            inventorySlots[i].item = inventory.items[i];
            inventorySlots[i].UpdateSlotUI();
        }
    }
}
