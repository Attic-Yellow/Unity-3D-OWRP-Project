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
    }

    // 로그인 성공 후 호출될 메서드로 변경
    public void LoginAndConnectToPhoton(string firebaseUserId, string serverName)
    {
        this.userFirebaseUserId = firebaseUserId; // Firebase 사용자 ID 저장
        roomName = serverName; // 서버 이름 설정

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.AuthValues = new AuthenticationValues(firebaseUserId); // Firebase 인증 정보 설정
            PhotonNetwork.ConnectUsingSettings(); // Photon에 연결 시도
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Photon: Connected to Master Server.");
        DecideActionBasedOnManagerStatus();
    }

    void DecideActionBasedOnManagerStatus()
    {
        if (GameManager.Instance.GetIsManager())
        {
            CreateAndJoinRoom(); // 매니저는 방을 생성
        }
        else
        {
            TryJoinRoom(); // 일반 사용자는 기존 방에 입장 시도
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"방 입장 실패: {message}. 일반 사용자는 방이 없으면 입장할 수 없습니다.");
        // 추가적으로 사용자에게 방이 없어 입장할 수 없다는 메시지를 보여주는 등의 로직을 구현할 수 있습니다.
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
