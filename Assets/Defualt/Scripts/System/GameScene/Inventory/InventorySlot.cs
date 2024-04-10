using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : Slot, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public Item item;
    private GameObject dragVisual;
    private Item tempitem; // �ӽ÷� ������ ������ ������

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
            if (slot != null)
            {
                // ��� ����: �������� �� ���Կ� �Ҵ�
                slot.AssignItem(tempitem);
                slot.UpdateSlotUI();
            }
            else
            {
                item = tempitem;
                UpdateSlotUI();
            }
        }
        else
        {
            // ��� ����: ���� ���Կ� �������� �ٽ� �Ҵ�
            item = tempitem;
            UpdateSlotUI();
        }

        // �ӽ� ������ �ʱ�ȭ
        item = null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            tempitem = item;
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
}
