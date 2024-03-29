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
            JoinRoom();
        }
    }

    public override void OnConnectedToMaster()
    {
        print("Connected to Photon as " + PhotonNetwork.AuthValues.UserId);
        JoinRoom();
    }

    private void JoinRoom()
    {
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 20 }; // �� �ɼ� ����
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default); // �濡 �����ϰų� ���� ����
    }

    public override void OnJoinedRoom()
    {
        print("Joined the room: " + PhotonNetwork.CurrentRoom.Name);
        GameManager.Instance.sceneLoadManager.LoadScene("GameScene"); // ���� ������ �̵�
    }

    // ���� ���п� ���� ó��
    public override void OnDisconnected(DisconnectCause cause)
    {
        print("Disconnected from Photon! Reason: " + cause.ToString());
    }
}
