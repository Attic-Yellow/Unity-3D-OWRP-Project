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
        // ���콺 Ŭ�� ��ġ�� UI ����� �߽� ��ġ�� ���̸� ����Ͽ� �������� ����
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out offset);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // �巡�� ���� ��ġ�� UI ��Ҹ� �̵���Ű��, �������� �����Ͽ� ���콺 Ŀ������ ����� ��ġ�� ����
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform.parent as RectTransform, eventData.position, eventData.pressEventCamera, out Vector2 globalMousePos))
        {
            rectTransform.localPosition = globalMousePos - offset;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        rectTransform.SetAsLastSibling(); // �巡�� ���� UI ��Ҹ� �ֻ����� ��ġ
    }
}
