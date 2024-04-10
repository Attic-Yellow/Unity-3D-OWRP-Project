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
        rectTransform.SetAsLastSibling(); // 드래그 중인 UI 요소를 최상위로 배치
    }

}
