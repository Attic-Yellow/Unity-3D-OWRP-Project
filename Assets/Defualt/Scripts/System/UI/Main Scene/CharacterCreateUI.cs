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

    private float lastClickTime = 0f; // ������ Ŭ�� �ð��� ����
    private const float doubleClickThreshold = 0.25f; // ���� Ŭ������ ���ֵǴ� �ð�(�� ����)
    private int currentAreaIndex = 0;

    [Header("���� �Է� ������")]
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

    /*** ���� ��Ʈ�ѷ� �޼��� ***/

    // �ٸ� ���� ȣ�� �޼���
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

    // ���� ���� ���� ��Ʈ�ѷ� �޼���
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

    // ���� ���� ���� �޼���
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

    // ���� ���� ���� �޼���
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

    // �޽��� Ȱ��ȭ/��Ȱ��ȭ ��Ʈ�ѷ� �޼���
    public void OnCheckMessageController()
    {
        checkMessage.SetActive(!checkMessage.activeInHierarchy);
    }

    /*** ��ư �޼��� ***/

    // ���� ���� ��ư �޼���
    public void OnJobButtonClick(int jobNum)
    {
        // ���� �ð��� ������ Ŭ�� �ð��� ���̸� ����մϴ�.
        float timeSinceLastClick = Time.time - lastClickTime;
        lastClickTime = Time.time;

        // ���� ���� ���� �Ǵ� ���� ���� ó��
        if (timeSinceLastClick <= doubleClickThreshold) // ���� Ŭ������ ���ֵǴ� ���
        {
            job = ((Job)jobNum).ToString(); // ������ ������ ���ڿ��� ����
            // print(job);
            currentAreaIndex++;
            CreationAreasController(currentAreaIndex);
        }
        else // ���� Ŭ������ ���ֵǴ� ���
        {
            // JobsAreaController(jobNum);
            JobExplanationController(jobNum);
        }
    }

    // ���� ���� ��ư �޼���
    public void OnTribeButtonClick(int tribeNum)
    {
        float timeSinceLastClick = Time.time - lastClickTime;
        lastClickTime = Time.time;

        if (timeSinceLastClick <= doubleClickThreshold) // ���� Ŭ������ ���ֵǴ� ���
        {
            tribe = ((Tribe)tribeNum).ToString(); // ������ ������ ���ڿ��� ����
            // print(tribe);
            currentAreaIndex++;
            CreationAreasController(currentAreaIndex);
        }
        else // ���� Ŭ������ ���ֵǴ� ���
        {
            TribeExplanationController(tribeNum);
        }
    }

    // �̸� �Է� �Ϸ� ��ư �޼���
    public void OnNameInputFieldEndEdit()
    {
        characterName = nameInputField.text;
        // print(characterName);
        OnCheckMessageController();
        currentAreaIndex++;
        CreationAreasController(currentAreaIndex);
    }

    // ���� ���� ��ư �޼���
    public void OnServerButtonClick(int serverNum)
    {
        float timeSinceLastClick = Time.time - lastClickTime;
        lastClickTime = Time.time;

        if (timeSinceLastClick <= doubleClickThreshold) // ���� Ŭ������ ���ֵǴ� ���
        {
            server = ((Server)serverNum).ToString(); // ������ ������ ���ڿ��� ����
            // print(server);
            CreateCharacterCallBack(serverNum);
        }
    }

    // �ڷ� ���� ��ư �޼���
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

    // ĳ���� ���� �޼���
    private async void CreateCharacterCallBack(int serverNum)
    {
        var user = FirebaseAuth.DefaultInstance.CurrentUser;

        bool isCharacterCreated = await GameManager.Instance.firebaseManager.CreateCharacter(user.UserId, user.Email, job, tribe, server, characterName);

        if (isCharacterCreated)
        {
            print("ĳ���� ������ �����Ͽ����ϴ�");
            GameManager.Instance.uiManager.mainSceneUI.MainSceneInit();
            await GameManager.Instance.uiManager.mainSceneUI.characterSelectedUI.CharacterSelecteAreaController(serverNum);
        }
        else
        {
            print("ĳ���� ������ �����Ͽ����ϴ�");
        }
    }
}
