using Firebase.Auth;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager Instance;

    private string roomName = "DefaultRoom"; // �� �̸��� �����մϴ�.
    private string userFirebaseUserId; // Firebase ����� ID�� ������ ������ �߰��մϴ�.


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

        ConnectToPhoton();
    }

    void ConnectToPhoton()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings(); // Photon�� ���� �õ�
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Photon: Connected to Master Server.");
        JoinOrCreateRoom();
    }

    void JoinOrCreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = 20,
            IsVisible = true,
            IsOpen = true
        };
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public void ConnectToPhoton(string firebaseUserId, string serverName)
    {
        this.userFirebaseUserId = firebaseUserId; // Firebase ����� ID�� ����
        roomName = serverName; // ���� �̸��� ����

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.AuthValues = new AuthenticationValues(firebaseUserId); // Firebase ���� ������ Photon ���� ������ ����
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            DecideActionBasedOnManagerStatus();
        }
    }

    void DecideActionBasedOnManagerStatus()
    {
        if (GameManager.Instance.GetIsManager())
        {
            CreateAndJoinRoom();
        }
        else
        {
            TryJoinRoom();
        }
    }


    private void CreateAndJoinRoom()
    {
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = 20,
            IsVisible = true,
            IsOpen = true
        };
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    private void TryJoinRoom()
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"�� ���� ����: {message}. ���ο� ���� Ž���մϴ�...");
        // �� ���忡 �������� ���� ������ ���⿡ ����
        // ��: �ٸ� �� Ž�� �Ǵ� ���� �޽��� ǥ��
    }

    public override void OnJoinedRoom()
    {
        print(GameManager.Instance.GetIsManager() ? "������ Ŭ���̾�Ʈ�� �濡 �����߽��ϴ�." : "�Ϲ� Ŭ���̾�Ʈ�� �濡 �����߽��ϴ�.");

        if (GameManager.Instance.GetIsManager())
        {
            SetupMasterClient();
        }
        else
        {
            GameManager.Instance.sceneLoadManager.LoadScene("GameScene");
        }
    }

    private void SetupMasterClient()
    {
        // ������ Ŭ���̾�Ʈ ���� �۾�, ��: �߿��� ���� ������Ʈ ���� �� �ʱ�ȭ
        print("������ Ŭ���̾�Ʈ�� ���� ������ �غ��մϴ�.");

        GameManager.Instance.sceneLoadManager.LoadScene("GameScene");
    }

    public override void OnCreatedRoom()
    {
        print($"�� ���� �Ϸ�: {PhotonNetwork.CurrentRoom.Name}");
    }

    // ���� ���п� ���� ó��
    public override void OnDisconnected(DisconnectCause cause)
    {
        print($"Photon���� ������ ������ϴ�. ����: {cause}");
    }
}
