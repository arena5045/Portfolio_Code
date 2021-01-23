using Photon.Pun; // 유니티용 포톤 컴포넌트들
using Photon.Realtime; // 포톤 서비스 관련 라이브러리
using UnityEngine;
using UnityEngine.UI;

// 마스터(매치 메이킹) 서버와 룸 접속을 담당
public class LobbyManager : MonoBehaviourPunCallbacks {
    private string gameVersion = "1"; // 게임 버전

    public Text connectionInfoText; // 네트워크 정보를 표시할 텍스트
    public Button joinButton; // 룸 접속 버튼
    public InputField NicknameInput;

    // 게임 실행과 동시에 마스터 서버 접속 시도
    private void Start() {
        PhotonNetwork.GameVersion = gameVersion; //접속에 필요한 정보 게임정보설정
        PhotonNetwork.ConnectUsingSettings();//서버접속시도
        joinButton.interactable = false;
        connectionInfoText.text = "마스터 서버에 접속중...";
    }

    // 마스터 서버 접속 성공시 자동 실행
    public override void OnConnectedToMaster() {
        joinButton.interactable = true;//서버접속 성공시 룸접속 버튼 활성화
        connectionInfoText.text = "온라인 : 마스터 서버와 연결됨";
    }

    // 마스터 서버 접속 실패시 자동 실행
    public override void OnDisconnected(DisconnectCause cause) {
        joinButton.interactable = false;
        connectionInfoText.text = "오프라인 : 마스터 서버와 연결되지 않음 \n 접속 재시도중...";
        PhotonNetwork.ConnectUsingSettings(); //접속다시시도
    }

    // 룸 접속 시도
    public void Connect() {
        joinButton.interactable = false;//중복접속 시도를 막기위해 접속 버튼 잠시 비활성화
        if (PhotonNetwork.IsConnected)//마스터 서버에 접속중이면
        {
            PhotonNetwork.LocalPlayer.NickName = NicknameInput.text;
            connectionInfoText.text = "룸에 접속..";
            PhotonNetwork.JoinRandomRoom();//룸접속 실행
        
        }
        else
        {
            connectionInfoText.text = "오프라인 : 마스터 서버와 연결되지 않음 \n 접속 재시도중...";
            PhotonNetwork.ConnectUsingSettings(); //접속다시시도
        }
    }

    // (빈 방이 없어)랜덤 룸 참가에 실패한 경우 자동 실행
    public override void OnJoinRandomFailed(short returnCode, string message) {
        connectionInfoText.text = "빈 방이 없음 , 새로운 방 생성";
        PhotonNetwork.CreateRoom("Room"+Random.Range(0,1000), new RoomOptions { MaxPlayers = 4 });
        //룸을 생성한 클라이언트가 호스트 역할을 맡는다
    }

    // 룸에 참가 완료된 경우 자동 실행
    public override void OnJoinedRoom() {
        connectionInfoText.text = "방 참가 성공";
        PhotonNetwork.LoadLevel("Main");//방장 플레이어가 씬 로드를 시켜줌
    }
}