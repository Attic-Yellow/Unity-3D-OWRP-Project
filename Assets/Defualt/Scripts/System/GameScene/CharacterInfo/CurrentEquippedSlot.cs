using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class CurrentEquippedSlot : Slot, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public Equipment equipment;
    private GameObject dragVisual;
    private Equipment tempEquipment; // �ӽ÷� ������ ��� ������

    public override void UpdateSlotUI()
    {
        itemIcon.sprite = equipment.itemImage;

        if (itemIcon.sprite != null)
        {
            itemIcon.gameObject.SetActive(true);
        }
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
            CurrentEquipped.Instance.RemoveEquipped(equipment);
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
        equipment = newEquipment; // ���ο� ��� �Ҵ�
        CurrentEquipped.Instance.IsEquipped(newEquipment);
        UpdateSlotUI(); // ������ UI�� ������Ʈ
    }
}
