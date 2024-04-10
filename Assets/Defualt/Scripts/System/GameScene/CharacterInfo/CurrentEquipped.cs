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
        int index = (int)equipment.equipment; // EquipmentType�� �ش��ϴ� �ε����� ������ ��ȯ

        // �ش� ��ġ�� ��� �̹� ������, ���� ��� ������ ��ġ�� �߰�
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

        // �� ��� �ش� ��ġ�� ����
        currentEquippeds[index] = equipment;

        // ��� ���� �˸�
        onChangeEquipp?.Invoke();

        return true;
    }

    public void RemoveEquipped(Equipment equipment)
    {
        int index = (int)equipment.equipment; // EquipmentType�� �ش��ϴ� �ε����� ������ ��ȯ

        // ��� �����ϰ�, ��� ���� �˸�
        currentEquippeds[index] = null;
        onChangeEquipp?.Invoke();
    }
}
