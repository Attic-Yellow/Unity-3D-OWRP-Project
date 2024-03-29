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

        GameManager.Instance.photonManager = this; 
        PhotonNetwork.AutomaticallySyncScene = true;
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
            if (PhotonNetwork.IsMasterClient)
            {
                CreateAndJoinRoom();
            }
            else
            {
                JoinRoom();
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        print("Connected to Photon as " + PhotonNetwork.AuthValues.UserId);
        JoinRoom();
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

    private void JoinRoom()
    {
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 20 }; // �� �ɼ� ����
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default); // �濡 �����ϰų� ���� ����
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.IsMasterClient ? "������ Ŭ���̾�Ʈ�� �濡 �����߽��ϴ�." : "�Ϲ� Ŭ���̾�Ʈ�� �濡 �����߽��ϴ�.");

        if (PhotonNetwork.IsMasterClient)
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
        Debug.Log("������ Ŭ���̾�Ʈ�� ���� ������ �غ��մϴ�.");

        GameManager.Instance.sceneLoadManager.LoadScene("GameScene");
        // ����: ���� ������ ���� �ʱ� ������Ʈ ��ġ ����
        // �� �κп� ���� ���� �� �ʿ��� ������Ʈ�� �����ϰ� �ʱ�ȭ�ϴ� �ڵ带 �߰��մϴ�.
        // ��: PhotonNetwork.Instantiate("SomePrefab", position, rotation);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log($"�� ���� �Ϸ�: {PhotonNetwork.CurrentRoom.Name}");
    }

    // ���� ���п� ���� ó��
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Photon���� ������ ������ϴ�. ����: {cause}");
    }
}
