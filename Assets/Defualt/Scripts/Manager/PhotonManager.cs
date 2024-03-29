using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using TMPro; // TextMeshPro�� ����ϱ� ���� �߰�

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
    public void CreateOrJoinRoom(string roomName, string nickname)
    {
        PhotonNetwork.NickName = nickname;

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
            PhotonNetwork.JoinRoom(roomName);
        }
    }

    public override void OnJoinedRoom()
    {
        // �濡 ���������� ������ �� ���� ������ �̵�
        GameManager.Instance.sceneLoadManager.LoadScene("GameScene");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("�� ���� ����: " + message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("�� ���� ����: " + message);
    }
}
