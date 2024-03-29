using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    public Dictionary<string, object> characterData { get; private set; } // ĳ���� ������ ����

    private void Awake()
    {
        GameManager.Instance.dataManager.characterData = this; // ������ �Ŵ����� ĳ���� ������ ����
    }

    public void SetCharacterData(Dictionary<string, object> characterData)
    {
        this.characterData = characterData; // ĳ���� ������ ����
    }
}
