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
    private Equipment tempEquipment; // 임시로 저장할 장비 데이터

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
            dragVisual.transform.position = Input.mousePosition; // 마우스 위치로 시각적 표현 이동
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragVisual != null)
        {
            Destroy(dragVisual);
        }
        // 마우스 포인터 아래의 "Slot" 태그를 가진 오브젝트만 검사
        List<RaycastResult> hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, hits);
        RaycastResult? hit = hits.FirstOrDefault(h => h.gameObject.CompareTag("Slot"));

        if (hit.HasValue && hit.Value.gameObject != null)
        {
            // 드랍 위치의 슬롯 처리
            Slot slot = hit.Value.gameObject.GetComponent<Slot>();
            if (slot != null && equipmentType == slot.equipmentType)
            {
                // 드랍 성공: 아이템을 새 슬롯에 할당
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
            // 드랍 실패: 원래 슬롯에 아이템을 다시 할당
            equipment = tempEquipment;
            UpdateSlotUI();
        }

        // 임시 데이터 초기화
        tempEquipment = null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (equipment != null)
        {
            tempEquipment = equipment;
            CurrentEquipped.Instance.RemoveEquipped(equipment);
            ClearSlot(); // 슬롯 클리어

            // 시각적 표현 생성
            dragVisual = new GameObject("Drag Visual");
            dragVisual.transform.SetParent(GameObject.FindObjectOfType<Canvas>().transform); // Canvas를 부모로 설정
            Image visualImage = dragVisual.AddComponent<Image>();
            visualImage.sprite = itemIcon.sprite; // 현재 슬롯의 아이템 이미지 사용
            visualImage.rectTransform.sizeDelta = new Vector2(50, 50); // 크기 조절
            visualImage.raycastTarget = false; // 이벤트 레이캐스트 무시
        }
    }

    public override void AssignEquipment(Equipment newEquipment)
    {
        equipment = newEquipment; // 새로운 장비를 할당
        CurrentEquipped.Instance.IsEquipped(newEquipment);
        UpdateSlotUI(); // 슬롯의 UI를 업데이트
    }
}
