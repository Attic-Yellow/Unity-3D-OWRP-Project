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
        // ���콺 ���� ��ư Ŭ�� ����
        if (Input.GetMouseButtonDown(0))
        {
            // Ŭ���� UI ��� ������ �߻������� �޴� ��ư�� �ƴ� ���
            if (EventSystem.current.IsPointerOverGameObject() && !IsMenuButtonClicked())
            {
                MenuListController(4); // ��� ����Ʈ�� ��Ȱ��ȭ
            }
            else if (!EventSystem.current.IsPointerOverGameObject())
            {
                MenuListController(4);
            }
        }
    }

    // ���� Ŭ���� ������Ʈ�� �޴� ��ư���� Ȯ��
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
