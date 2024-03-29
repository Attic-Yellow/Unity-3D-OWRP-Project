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

        ConnectToPhoton();
    }

    void ConnectToPhoton()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings(); // Photon과 연결 시도
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
        this.userFirebaseUserId = firebaseUserId; // Firebase 사용자 ID를 저장
        roomName = serverName; // 서버 이름을 설정

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.AuthValues = new AuthenticationValues(firebaseUserId); // Firebase 인증 정보를 Photon 인증 정보로 설정
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
        Debug.Log($"방 입장 실패: {message}. 새로운 방을 탐색합니다...");
        // 방 입장에 실패했을 때의 로직을 여기에 구현
        // 예: 다른 방 탐색 또는 에러 메시지 표시
    }

    public override void OnJoinedRoom()
    {
        print(GameManager.Instance.GetIsManager() ? "마스터 클라이언트로 방에 입장했습니다." : "일반 클라이언트로 방에 입장했습니다.");

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
        // 마스터 클라이언트 전용 작업, 예: 중요한 게임 오브젝트 생성 및 초기화
        print("마스터 클라이언트가 게임 세팅을 준비합니다.");

        GameManager.Instance.sceneLoadManager.LoadScene("GameScene");
    }

    public override void OnCreatedRoom()
    {
        print($"방 생성 완료: {PhotonNetwork.CurrentRoom.Name}");
    }

    // 연결 실패에 대한 처리
    public override void OnDisconnected(DisconnectCause cause)
    {
        print($"Photon에서 연결이 끊겼습니다. 이유: {cause}");
    }
}
