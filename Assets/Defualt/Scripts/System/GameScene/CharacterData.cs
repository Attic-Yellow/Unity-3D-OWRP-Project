using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    public Dictionary<string, object> characterData { get; private set; } // 캐릭터 데이터 저장

    private void Awake()
    {
        GameManager.Instance.dataManager.characterData = this; // 데이터 매니저에 캐릭터 데이터 설정
    }

    public void SetCharacterData(Dictionary<string, object> characterData)
    {
        this.characterData = characterData; // 캐릭터 데이터 설정
    }
}
