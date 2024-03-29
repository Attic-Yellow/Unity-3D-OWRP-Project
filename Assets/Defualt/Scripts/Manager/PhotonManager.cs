using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager Instance;

    private string gameVersion = "1.0"; // ���� ����, ȣȯ���� ���� ����

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        GameManager.Instance.photonManager = this;
    }

    void Start()
    {
        ConnectToPhoton();
    }

    // ���� ������ ����
    public void ConnectToPhoton()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // ���� ������ ���������� ����Ǿ��� �� ȣ���
    public override void OnConnectedToMaster()
    {
        print("���� ������ ����Ǿ����ϴ�.");

        // �� ����� �������ų� �߰� �۾��� ����
    }

    // �� ���� �Ǵ� ����
    public void CreateOrJoinRoom(string roomName)
    {
        if (GameManager.Instance.GetIsManager())
        {
            // �������� ��� �� ����
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 20; // �ִ� �÷��̾� �� ����
            PhotonNetwork.CreateRoom(roomName, roomOptions);
        }
        else
        {
            // �Ϲ� ������� ��� �� ����
            PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions { MaxPlayers = 20 }, TypedLobby.Default);
        }
    }

    // �� ������ �������� �� ȣ���
    public override void OnCreatedRoom()
    {
        print("���� �����߽��ϴ�.");
        GameManager.Instance.sceneLoadManager.LoadScene("GameScene");
    }

    // �濡 ���������� �������� �� ȣ���
    public override void OnJoinedRoom()
    {
        print("�濡 �����߽��ϴ�.");
        GameManager.Instance.sceneLoadManager.LoadScene("GameScene");
    }

    // �� ������ �������� �� ȣ���
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("�� ������ �����߽��ϴ�: " + message);
    }
}
