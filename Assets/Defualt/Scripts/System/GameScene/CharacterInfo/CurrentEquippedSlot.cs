using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentEquippedSlot : Slot
{
    public Equipment equipment;

    public override void UpdateSlotUI()
    {
        itemIcon.sprite = equipment.itemImage;
        itemIcon.gameObject.SetActive(true);
    }

    public override void ClearSlot()
    {
        equipment = null;
        itemIcon.sprite = null;
        itemIcon.gameObject.SetActive(false);
    }
}
