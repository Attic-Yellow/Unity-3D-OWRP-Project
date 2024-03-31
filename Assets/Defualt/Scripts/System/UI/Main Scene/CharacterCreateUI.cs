using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Auth;
using Unity.VisualScripting;
using System.Threading.Tasks;

enum Job
{
    Warrior,
    Dragoon,
    Bard,
    WhiteMage,
    BlackMage
}

enum Tribe
{
    Human,
    Elf,
    Dwarf,
    Orc
}

enum Server
{
    server1,
    server2,
    server3
}

public class CharacterCreateUI : MonoBehaviour
{


    [SerializeField] private List<GameObject> characterCreationAreas;
    [SerializeField] private List<GameObject> jobExplanations;
    [SerializeField] private List<GameObject> jobsArea;
    [SerializeField] private List<GameObject> tribeExplanations;
    [SerializeField] private GameObject checkMessage;
    [SerializeField] private TMP_InputField nameInputField;

    private float lastClickTime = 0f; // 마지막 클릭 시간을 추적
    private const float doubleClickThreshold = 0.25f; // 더블 클릭으로 간주되는 시간(초 단위)
    private int currentAreaIndex = 0;

    [Header("유저 입력 데이터")]
    [SerializeField] private string job;
    [SerializeField] private string tribe;
    [SerializeField] private string characterName;
    [SerializeField] private string server;

    private void Awake()
    {
        GameManager.Instance.uiManager.mainSceneUI.characterCreateUI = this;
    }

    private void Start()
    {
        CreationAreasController(0);

        if (checkMessage != null)
        {
            checkMessage.SetActive(false);
        }
    }

    /*** 영역 컨트롤러 메서드 ***/

    // 다른 영역 호출 메서드
    public void CreationAreasController(int areaNum)
    {
        if (characterCreationAreas.Count > 0)
        {
            for (int i = 0; i < characterCreationAreas.Count; i++)
            {
                characterCreationAreas[i].SetActive(areaNum == i);
            }
        }
    }

    // 직업 선택 영역 컨트롤러 메서드
    public void JobsAreaController(int jobNum)
    {
        if (jobsArea.Count > 0)
        {
            for (int i = 0; i < jobsArea.Count; i++)
            {
                jobsArea[i].SetActive(jobNum == i);
            }
        }
    }

    // 직업 설명 변경 메서드
    public void JobExplanationController(int jobNum)
    {
        if (jobExplanations.Count > 0)
        {
            for (int i = 0; i < jobExplanations.Count; i++)
            {
                jobExplanations[i].SetActive(jobNum == i);
            }
        }
    }

    // 종족 설명 변경 메서드
    public void TribeExplanationController(int tribeNum)
    {
        if (tribeExplanations.Count > 0)
        {
            for (int i = 0; i < tribeExplanations.Count; i++)
            {
                tribeExplanations[i].SetActive(tribeNum == i);
            }
        }
    }

    // 메시지 활설화/비활성화 컨트롤러 메서드
    public void OnCheckMessageController()
    {
        checkMessage.SetActive(!checkMessage.activeInHierarchy);
    }

    /*** 버튼 메서드 ***/

    // 직업 선택 버튼 메서드
    public void OnJobButtonClick(int jobNum)
    {
        // 현재 시간과 마지막 클릭 시간의 차이를 계산합니다.
        float timeSinceLastClick = Time.time - lastClickTime;
        lastClickTime = Time.time;

        // 직업 설명 변경 또는 직업 선택 처리
        if (timeSinceLastClick <= doubleClickThreshold) // 더블 클릭으로 간주되는 경우
        {
            job = ((Job)jobNum).ToString(); // 선택한 직업을 문자열로 저장
            // print(job);
            currentAreaIndex++;
            CreationAreasController(currentAreaIndex);
        }
        else // 단일 클릭으로 간주되는 경우
        {
            // JobsAreaController(jobNum);
            JobExplanationController(jobNum);
        }
    }

    // 종족 선택 버튼 메서드
    public void OnTribeButtonClick(int tribeNum)
    {
        float timeSinceLastClick = Time.time - lastClickTime;
        lastClickTime = Time.time;

        if (timeSinceLastClick <= doubleClickThreshold) // 더블 클릭으로 간주되는 경우
        {
            tribe = ((Tribe)tribeNum).ToString(); // 선택한 종족을 문자열로 저장
            // print(tribe);
            currentAreaIndex++;
            CreationAreasController(currentAreaIndex);
        }
        else // 단일 클릭으로 간주되는 경우
        {
            TribeExplanationController(tribeNum);
        }
    }

    // 이름 입력 완료 버튼 메서드
    public void OnNameInputFieldEndEdit()
    {
        characterName = nameInputField.text;
        // print(characterName);
        OnCheckMessageController();
        currentAreaIndex++;
        CreationAreasController(currentAreaIndex);
    }

    // 서버 선택 버튼 메서드
    public void OnServerButtonClick(int serverNum)
    {
        float timeSinceLastClick = Time.time - lastClickTime;
        lastClickTime = Time.time;

        if (timeSinceLastClick <= doubleClickThreshold) // 더블 클릭으로 간주되는 경우
        {
            server = ((Server)serverNum).ToString(); // 선택한 서버를 문자열로 저장
            // print(server);
            CreateCharacterCallBack(serverNum);
        }
    }

    // 뒤로 가기 버튼 메서드
    public void OnBackButtonClick()
    {
        if (currentAreaIndex > 0)
        {
            currentAreaIndex--;
            CreationAreasController(currentAreaIndex);
        }
        else
        {            
            GameManager.Instance.uiManager.mainSceneUI.MainSceneInit();
        }
    }

    // 캐릭터 생성 메서드
    private async void CreateCharacterCallBack(int serverNum)
    {
        var user = FirebaseAuth.DefaultInstance.CurrentUser;

        bool isCharacterCreated = await GameManager.Instance.firebaseManager.CreateCharacter(user.UserId, user.Email, job, tribe, server, characterName);

        if (isCharacterCreated)
        {
            print("캐릭터 생성에 성공하였습니다");
            GameManager.Instance.uiManager.mainSceneUI.MainSceneInit();
            await GameManager.Instance.uiManager.mainSceneUI.characterSelectedUI.CharacterSelecteAreaController(serverNum);
        }
        else
        {
            print("캐릭터 생성에 실패하였습니다");
        }
    }
}
