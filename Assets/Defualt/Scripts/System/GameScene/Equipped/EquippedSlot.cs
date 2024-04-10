using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class EquippedSlot : Slot, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public Equipment equipment;
    private GameObject dragVisual;
    private Equipment tempEquipment; // �ӽ÷� ������ ��� ������

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

    public void OnDrag(PointerEventData eventData)
    {
        if (dragVisual != null)
        {
            dragVisual.transform.position = Input.mousePosition; // ���콺 ��ġ�� �ð��� ǥ�� �̵�
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragVisual != null)
        {
            Destroy(dragVisual);
        }
        // ���콺 ������ �Ʒ��� "Slot" �±׸� ���� ������Ʈ�� �˻�
        List<RaycastResult> hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, hits);
        RaycastResult? hit = hits.FirstOrDefault(h => h.gameObject.CompareTag("Slot"));

        if (hit.HasValue && hit.Value.gameObject != null)
        {
            // ��� ��ġ�� ���� ó��
            Slot slot = hit.Value.gameObject.GetComponent<Slot>();
            if (slot != null && equipmentType == slot.equipmentType)
            {
                // ��� ����: �������� �� ���Կ� �Ҵ�
                slot.AssignEquipment(tempEquipment);
                slot.UpdateSlotUI();
            }
            else
            {
                equipment = tempEquipment;
                UpdateSlotUI();
            }
        }
        else
        {
            // ��� ����: ���� ���Կ� �������� �ٽ� �Ҵ�
            equipment = tempEquipment;
            UpdateSlotUI();
        }

        // �ӽ� ������ �ʱ�ȭ
        tempEquipment = null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (equipment != null)
        {
            tempEquipment = equipment;
            RemoveSlot(equipment); // ��� ����
            ClearSlot(); // ���� Ŭ����

            // �ð��� ǥ�� ����
            dragVisual = new GameObject("Drag Visual");
            dragVisual.transform.SetParent(GameObject.FindObjectOfType<Canvas>().transform); // Canvas�� �θ�� ����
            Image visualImage = dragVisual.AddComponent<Image>();
            visualImage.sprite = itemIcon.sprite; // ���� ������ ������ �̹��� ���
            visualImage.rectTransform.sizeDelta = new Vector2(50, 50); // ũ�� ����
            visualImage.raycastTarget = false; // �̺�Ʈ ����ĳ��Ʈ ����
        }
    }

    public override void AssignEquipment(Equipment newEquipment)
    {
        this.equipment = newEquipment; // ���ο� ��� �Ҵ�
        UpdateSlotUI(); // ������ UI�� ������Ʈ

        switch (newEquipment.equipment)
        {
            case EquipmentType.Weapon:
                Equipped.Instance.weapon.Add(newEquipment);
                break;
            case EquipmentType.Head:
                Equipped.Instance.head.Add(newEquipment);
                break;
            case EquipmentType.Body:
                Equipped.Instance.body.Add(newEquipment);
                break;
            case EquipmentType.Hands:
                Equipped.Instance.hands.Add(newEquipment);
                break;
            case EquipmentType.Legs:
                Equipped.Instance.legs.Add(newEquipment);
                break;
            case EquipmentType.Feet:
                Equipped.Instance.feet.Add(newEquipment);
                break;
            case EquipmentType.Auxiliary:
                Equipped.Instance.auxiliary.Add(newEquipment);
                break;
            case EquipmentType.Earring:
                Equipped.Instance.earring.Add(newEquipment);
                break;
            case EquipmentType.Necklace:
                Equipped.Instance.necklace.Add(newEquipment);
                break;
            case EquipmentType.Bracelet:
                Equipped.Instance.bracelet.Add(newEquipment);
                break;
            case EquipmentType.Ring:
                Equipped.Instance.ring.Add(newEquipment);
                break;
        }
    }

    private void RemoveSlot(Equipment newEquipment)
    {
        switch (newEquipment.equipment)
        {
            case EquipmentType.Weapon:
                Equipped.Instance.RemoveWeapon(newEquipment);
                break;
            case EquipmentType.Head:
                Equipped.Instance.RemoveHead(newEquipment);
                break;
            case EquipmentType.Body:
                Equipped.Instance.RemoveBody(newEquipment);
                break;
            case EquipmentType.Hands:
                Equipped.Instance.RemoveHands(newEquipment);
                break;
            case EquipmentType.Legs:
                Equipped.Instance.RemoveLegs(newEquipment);
                break;
            case EquipmentType.Feet:
                Equipped.Instance.RemoveFeet(newEquipment);
                break;
            case EquipmentType.Auxiliary:
                Equipped.Instance.RemoveAuxiliary(newEquipment);
                break;
            case EquipmentType.Earring:
                Equipped.Instance.RemoveEarring(newEquipment);
                break;
            case EquipmentType.Necklace:
                Equipped.Instance.RemoveNecklace(newEquipment);
                break;
            case EquipmentType.Bracelet:
                Equipped.Instance.RemoveBracelet(newEquipment);
                break;
            case EquipmentType.Ring:
                Equipped.Instance.RemoveRing(newEquipment);
                break;
        }
    }
}
