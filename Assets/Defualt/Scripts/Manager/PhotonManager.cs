using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using TMPro; // TextMeshPro를 사용하기 위해 추가

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager Instance;

    private string gameVersion = "1.0"; // 게임 버전, 호환성을 위해 사용됨

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

    // 포톤 서버에 연결
    public void ConnectToPhoton()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // 포톤 서버에 성공적으로 연결되었을 때 호출됨
    public override void OnConnectedToMaster()
    {
        print("포톤 서버에 연결되었습니다.");

        // 방 목록을 가져오거나 추가 작업을 수행
    }

    // 방 생성 또는 참여
    public void CreateOrJoinRoom(string roomName, string nickname)
    {
        PhotonNetwork.NickName = nickname;

        if (GameManager.Instance.GetIsManager())
        {
            // 관리자인 경우 방 생성
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 20; // 최대 플레이어 수 설정
            PhotonNetwork.CreateRoom(roomName, roomOptions);
        }
        else
        {
            // 일반 사용자인 경우 방 참여
            PhotonNetwork.JoinRoom(roomName);
        }
    }

    public override void OnJoinedRoom()
    {
        // 방에 성공적으로 참여한 후 게임 씬으로 이동
        GameManager.Instance.sceneLoadManager.LoadScene("GameScene");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("방 생성 실패: " + message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("방 참여 실패: " + message);
    }
}
