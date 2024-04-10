using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using ExitGames.Client.Photon;

public class LastSibling : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnPointerUp(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        rectTransform.SetAsLastSibling(); // �巡�� ���� UI ��Ҹ� �ֻ����� ��ġ
    }

}
