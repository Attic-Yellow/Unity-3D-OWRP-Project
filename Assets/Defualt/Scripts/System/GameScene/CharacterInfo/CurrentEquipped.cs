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
        if (Input.GetKeyDown(KeyCode.X))
        {
            IsEquipped(ItemData.Instance.equip[1]);
        }
    }

    public bool IsEquipped(Equipment equipment)
    {
        int index = (int)equipment.equipment; // EquipmentType에 해당하는 인덱스를 정수로 변환

        // 해당 위치에 장비가 이미 있으면, 이전 장비를 적절한 위치에 추가
        if (currentEquippeds[index] != null)
        {
            switch (equipment.equipment)
            {
                case EquipmentType.Weapon:
                    Equipped.Instance.AddWeapon(currentEquippeds[index]);
                    break;
                case EquipmentType.Head:
                    Equipped.Instance.AddHead(currentEquippeds[index]);
                    break;
                case EquipmentType.Body:
                    Equipped.Instance.AddBody(currentEquippeds[index]);
                    break;
                case EquipmentType.Hands:
                    Equipped.Instance.AddHands(currentEquippeds[index]);
                    break;
                case EquipmentType.Legs:
                    Equipped.Instance.AddLegs(currentEquippeds[index]);
                    break;
                case EquipmentType.Feet:
                    Equipped.Instance.AddFeet(currentEquippeds[index]);
                    break;
                case EquipmentType.Auxiliary:
                    Equipped.Instance.AddAuxiliary(currentEquippeds[index]);
                    break;
                case EquipmentType.Earring:
                    Equipped.Instance.AddEarring(currentEquippeds[index]);
                    break;
                case EquipmentType.Necklace:
                    Equipped.Instance.AddNecklace(currentEquippeds[index]);
                    break;
                case EquipmentType.Bracelet:
                    Equipped.Instance.AddBracelet(currentEquippeds[index]);
                    break;
                case EquipmentType.Ring:
                    Equipped.Instance.AddRing(currentEquippeds[index]);
                    break;
            }
        }

        // 새 장비를 해당 위치에 설정
        currentEquippeds[index] = equipment;

        // 장비 변경 알림
        onChangeEquipp?.Invoke();

        return true;
    }

    public void RemoveEquipped(Equipment equipment)
    {
        int index = (int)equipment.equipment; // EquipmentType에 해당하는 인덱스를 정수로 변환

        // 장비를 제거하고, 장비 변경 알림
        currentEquippeds[index] = null;
        onChangeEquipp?.Invoke();
    }
}
