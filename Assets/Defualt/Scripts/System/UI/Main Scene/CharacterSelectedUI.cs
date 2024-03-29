using Firebase.Auth;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectedUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> characterSelectedArea;
    [SerializeField] private GameObject characterButtonPrefab;
    [SerializeField] private List<GameObject> characterButton;

    private int serverNumber = 0;

    private void Awake()
    {
        GameManager.Instance.uiManager.mainSceneUI.characterSelectedUI = this;
    }

    private async void Start()
    {
        await CharacterSelecteAreaController(0);
    }

    /*** Page 및 Area의 Active 컨트롤러 메서드 ***/

    // 캐릭터 선택 영역 컨트롤러 메서드
    public async Task CharacterSelecteAreaController(int serverNum)
    {
        var user = FirebaseAuth.DefaultInstance.CurrentUser;

        for (int i = 0; i < characterSelectedArea.Count; i++)
        {
            if (characterSelectedArea[i].activeInHierarchy)
            {
                characterSelectedArea[i].SetActive(false);
            }
        }

        switch (serverNum)
        {
            case 0:
                characterSelectedArea[0].SetActive(true);
                await GameManager.Instance.firebaseManager.LoadCharacter(user.UserId, "server1", OnCharacterLoaded);
                break;
            case 1:
                characterSelectedArea[1].SetActive(true);
                await GameManager.Instance.firebaseManager.LoadCharacter(user.UserId, "server2", OnCharacterLoaded);
                break;
            case 2:
                characterSelectedArea[2].SetActive(true);
                await GameManager.Instance.firebaseManager.LoadCharacter(user.UserId, "server3", OnCharacterLoaded);
                break;

        }
    }

    /*** 버튼 메서드 ***/

    // 서버 버튼 클릭 메서드
    public async void OnServerButtonClick(int serverNum)
    {
        serverNumber = serverNum;
        await CharacterSelecteAreaController(serverNum);
    }

    // 캐릭터 버튼 클릭 메서드
    public void OnCharacterButtonClick()
    {

    }

    // 캐릭터 생성 버튼 클릭 메서드
    public void OnCreateButtonClick()
    {
        GameManager.Instance.uiManager.mainSceneUI.MainScenePageController();
    }

    // 캐릭터 로드 완료 시 호출될 메서드
    private void OnCharacterLoaded(List<Dictionary<string, object>> charactersData)
    {
        if (charactersData == null || charactersData.Count == 0)
        {
            print("캐릭터 로드 실패하였거나, 로드할 캐릭터가 없습니다.");
            return; // 캐릭터가 없으면 로직을 더 이상 진행하지 않습니다.
        }

        // 기존에 characterSelectedArea에 있는 자식 오브젝트들을 모두 삭제
        foreach (Transform child in characterSelectedArea[serverNumber].transform)
        {
            Destroy(child.gameObject);
        }

        // 새로운 캐릭터 정보 프리팹 인스턴스를 생성하고 characterSelectedArea에 추가
        foreach (var characterData in charactersData)
        {
            var instance = Instantiate(characterButtonPrefab, characterSelectedArea[serverNumber].transform); // 프리팹 인스턴스화

            var characterButtonScript = instance.GetComponent<CharacterButton>(); // CharacterButton 스크립트 참조

            if (characterButtonScript != null)
            {
                characterButtonScript.SetCharacterData(characterData); // 캐릭터 데이터 설정
            }
        }
    }
}
