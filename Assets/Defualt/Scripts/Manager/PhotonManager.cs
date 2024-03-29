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

    private string roomName = "DefaultRoom"; // 방 이름을 설정합니다.
    private string userFirebaseUserId; // Firebase 사용자 ID를 저장할 변수를 추가합니다.


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
        this.userFirebaseUserId = firebaseUserId; // Firebase 사용자 ID를 저장
        roomName = serverName; // 서버 이름을 설정

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.AuthValues = new AuthenticationValues(firebaseUserId); // Firebase 인증 정보를 Photon 인증 정보로 설정
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
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 20 }; // 방 옵션 설정
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default); // 방에 참여하거나 새로 생성
    }

    public override void OnJoinedRoom()
    {
        print("Joined the room: " + PhotonNetwork.CurrentRoom.Name);
        GameManager.Instance.sceneLoadManager.LoadScene("GameScene"); // 게임 씬으로 이동
    }

    // 연결 실패에 대한 처리
    public override void OnDisconnected(DisconnectCause cause)
    {
        print("Disconnected from Photon! Reason: " + cause.ToString());
    }
}
