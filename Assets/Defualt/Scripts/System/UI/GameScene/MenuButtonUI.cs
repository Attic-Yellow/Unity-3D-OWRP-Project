using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButtonUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> lists;

    private void Awake()
    {
        GameManager.Instance.uiManager.gameSceneUI.menuButtonsUI = this;
    }

    private void Start()
    {
        MenuListController(lists.Count);
    }

    private void Update()
    {
        // 마우스 왼쪽 버튼 클릭 감지
        if (Input.GetMouseButtonDown(0))
        {
            // 클릭이 UI 요소 위에서 발생했으나 메뉴 버튼이 아닐 경우
            if (EventSystem.current.IsPointerOverGameObject() && !IsMenuButtonClicked())
            {
                MenuListController(4); // 모든 리스트를 비활성화
            }
            else if (!EventSystem.current.IsPointerOverGameObject())
            {
                MenuListController(4);
            }
        }
    }

    // 현재 클릭된 오브젝트가 메뉴 버튼인지 확인
    private bool IsMenuButtonClicked()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            return EventSystem.current.currentSelectedGameObject.CompareTag("MenuButton");
        }
        return false;
    }

    public void MenuListController(int index)
    {
        if (lists.Count > 0)
        {
            for (int i = 0; i < lists.Count; i++)
            {
                lists[i].SetActive(i == index);
            }
        }
    }

    public void OnMenuButtonClick(int index)
    {
        MenuListController(index);
    }
}
