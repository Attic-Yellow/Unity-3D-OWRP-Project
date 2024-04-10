using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class DraggableUI : MonoBehaviour, IDragHandler, IPointerDownHandler, IEndDragHandler
{
    [SerializeField] private RectTransform windowRectTransform;
    [SerializeField] private RectTransform dragAreaRectTransform;
    private Vector2 offset;
    private bool isDragging = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        windowRectTransform.SetAsLastSibling(); // 클릭 시 해당 창을 최상위로 이동

        if (RectTransformUtility.RectangleContainsScreenPoint(dragAreaRectTransform, eventData.position, eventData.pressEventCamera))
        {
            Vector2 mousePosition = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, eventData.position);
            offset = (Vector2)windowRectTransform.position - mousePosition;
            isDragging = true; 
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging) 
        {
            Vector2 newPosition = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, eventData.position) + offset;
            windowRectTransform.position = newPosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }
}
