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
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 20 }; // 방 옵션 설정
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default); // 방에 참여하거나 새로 생성
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.IsMasterClient ? "마스터 클라이언트로 방에 입장했습니다." : "일반 클라이언트로 방에 입장했습니다.");

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
        // 마스터 클라이언트 전용 작업, 예: 중요한 게임 오브젝트 생성 및 초기화
        Debug.Log("마스터 클라이언트가 게임 세팅을 준비합니다.");

        GameManager.Instance.sceneLoadManager.LoadScene("GameScene");
        // 예시: 게임 시작을 위한 초기 오브젝트 배치 로직
        // 이 부분에 게임 시작 시 필요한 오브젝트를 생성하고 초기화하는 코드를 추가합니다.
        // 예: PhotonNetwork.Instantiate("SomePrefab", position, rotation);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log($"방 생성 완료: {PhotonNetwork.CurrentRoom.Name}");
    }

    // 연결 실패에 대한 처리
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Photon에서 연결이 끊겼습니다. 이유: {cause}");
    }
}
