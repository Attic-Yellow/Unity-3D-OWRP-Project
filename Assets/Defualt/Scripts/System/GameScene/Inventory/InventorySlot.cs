using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : Slot
{
    public Item item;

    public override void UpdateSlotUI()
    {
        itemIcon.sprite = item.itemImage;
        itemIcon.gameObject.SetActive(true);
    }

    public override void ClearSlot()
    {
        item = null;
        itemIcon.sprite = null;
        itemIcon.gameObject.SetActive(false);
    }
}
