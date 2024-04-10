using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager Instance;
     
    public string roomName;
    public string lobbyName = "ExampleLobby";
    private string gameVersion = "1.0"; // ���� ����, ȣȯ���� ���� ���

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
        PhotonNetwork.AutomaticallySyncScene = true;
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

    // ���� ������ ���������� ����Ǿ��� �� ȣ��
    public override void OnConnectedToMaster()
    {
        print("���� ������ ����Ǿ����ϴ�.");
    }

    // �� ���� �Ǵ� ����
    public void CreateOrJoinRoom(string roomName, string nickname)
    {
        TypedLobby customLobby = new TypedLobby(lobbyName, LobbyType.Default);
        PhotonNetwork.LocalPlayer.NickName = nickname;
        print(PhotonNetwork.LocalPlayer.NickName);
        PhotonNetwork.JoinLobby(customLobby);
        this.roomName = roomName;
        
    }

    // �κ� ���������� �����Ͽ��� �� ȣ��
    public override void OnJoinedLobby()
    {
        GameManager.Instance.sceneLoadManager.JoiningServer(30);
        print(GameManager.Instance.GetIsManager());
        print(lobbyName); 
        if (GameManager.Instance.GetIsManager())
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.IsVisible = true;
            roomOptions.IsOpen = true;
            roomOptions.MaxPlayers = 20; // �ִ� �÷��̾� �� ����
            print(roomName);
            
            PhotonNetwork.CreateRoom(roomName, roomOptions); // �������� ��� �� ����
        }
        else
        {
            print(roomName);
            PhotonNetwork.JoinRoom(roomName); // �Ϲ� ������� ��� �� ����
        }
    }

    // �濡 ���������� �����Ͽ��� �� ȣ��
    public override void OnJoinedRoom()
    {
        GameManager.Instance.sceneLoadManager.JoiningServer(70); // �濡 ���������� ������ �� ���� ������ �̵�
    }

    // ���� ���������� �������� �� ȣ��
    public override void OnCreatedRoom()
    {
        // GameManager.Instance.sceneLoadManager.JoiningServer(70);
    }

    // �� ������ �����Ͽ��� �� ȣ��
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print($"�� ���� ����: {message}");
    }

    // �� ������ �����Ͽ��� �� ȣ��
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"�� ���� ����: {message}");
    }
}
