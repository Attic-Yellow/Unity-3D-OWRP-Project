using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentEquipped : MonoBehaviour
{
    public static CurrentEquipped Instance;

    public delegate void OnEquippChanged();
    public OnEquippChanged onChangeEquipp;

    public List<Equipment> currentEquippeds = new List<Equipment>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            IsEquipped(ItemData.Instance.equip[0]);
        }
    }

    public bool IsEquipped(Equipment equipment)
    {
        switch (equipment.equipment)
        {
            case EquipmentType.Weapon:
                if (currentEquippeds[0] != null)
                {
                    Equipped.Instance.AddHead(currentEquippeds[0]);
                }
                currentEquippeds[0] = equipment;
                onChangeEquipp?.Invoke();
                return true;
            case EquipmentType.Head:
                if (currentEquippeds[1] != null)
                {
                    Equipped.Instance.AddHead(currentEquippeds[1]);
                }
                currentEquippeds[1] = equipment;
                onChangeEquipp?.Invoke();
                return true;
            case EquipmentType.Body:
                if (currentEquippeds[2] != null)
                {
                    Equipped.Instance.AddBody(currentEquippeds[2]);
                }
                currentEquippeds[2] = equipment;
                onChangeEquipp?.Invoke();
                return true;
            case EquipmentType.Hands:
                if (currentEquippeds[3] != null)
                {
                    Equipped.Instance.AddHands(currentEquippeds[3]);
                }
                currentEquippeds[3] = equipment;
                onChangeEquipp?.Invoke();
                return true;
            case EquipmentType.Legs:
                if (currentEquippeds[4] != null)
                {
                    Equipped.Instance.AddLegs(currentEquippeds[4]);
                }
                currentEquippeds[4] = equipment;
                onChangeEquipp?.Invoke();
                return true;
            case EquipmentType.Feet:
                if (currentEquippeds[5] != null)
                {
                    Equipped.Instance.AddFeet(currentEquippeds[5]);
                }
                currentEquippeds[5] = equipment;
                onChangeEquipp?.Invoke();
                return true;
            case EquipmentType.Auxiliary:
                if (currentEquippeds[6] != null)
                {
                    Equipped.Instance.AddAuxiliary(currentEquippeds[6]);
                }
                currentEquippeds[6] = equipment;
                onChangeEquipp?.Invoke();
                return true;
            case EquipmentType.Earring:
                if (currentEquippeds[7] != null)
                {
                    Equipped.Instance.AddEarring(currentEquippeds[7]);
                }
                currentEquippeds[7] = equipment;
                onChangeEquipp?.Invoke();
                return true;
            case EquipmentType.Necklace:
                if (currentEquippeds[8] != null)
                {
                    Equipped.Instance.AddNecklace(currentEquippeds[8]);
                }
                currentEquippeds[8] = equipment;
                onChangeEquipp?.Invoke();
                return true;
            case EquipmentType.Bracelet:
                if (currentEquippeds[9] != null)
                {
                    Equipped.Instance.AddBracelet(currentEquippeds[9]);
                }
                currentEquippeds[9] = equipment;
                onChangeEquipp?.Invoke();
                return true;
            case EquipmentType.Ring:
                if (currentEquippeds[10] != null)
                {
                    Equipped.Instance.AddRing(currentEquippeds[10]);
                }
                currentEquippeds[10] = equipment;
                onChangeEquipp?.Invoke();
                return true;
        }

        return false;
    }
}
