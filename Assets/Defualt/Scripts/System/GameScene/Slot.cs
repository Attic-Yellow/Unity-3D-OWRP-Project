using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Image itemIcon;
    public EquipmentType equipmentType;

    public virtual void UpdateSlotUI()
    {

    }

    public virtual void ClearSlot()
    {

    }

    public virtual void AssignItem(Item newItem)
    {

    }

    public virtual void AssignEquipment(Equipment newEquipment)
    {

    }

}
