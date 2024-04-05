using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class DraggableUI : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [SerializeField] private RectTransform rectTransform;
    private Vector2 offset;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnPointerUp(eventData);
        // 마우스 클릭 위치와 UI 요소의 중심 위치의 차이를 계산하여 오프셋을 설정
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out offset);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 드래그 중인 위치로 UI 요소를 이동시키되, 오프셋을 적용하여 마우스 커서와의 상대적 위치를 유지
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform.parent as RectTransform, eventData.position, eventData.pressEventCamera, out Vector2 globalMousePos))
        {
            rectTransform.localPosition = globalMousePos - offset;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        rectTransform.SetAsLastSibling(); // 드래그 중인 UI 요소를 최상위로 배치
    }
}
