using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class PhotonInit : MonoBehaviourPunCallbacks
{

    public InputField userId;//사용자이름
    public InputField roomName;//방이름
    public GameObject scrollContents;//스크롤 객체
    public GameObject roomItem;//동적생성할 룸 아이템
    void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
        roomName.text = "ROOM_" + Random.Range(0, 999).ToString("000");
    }
    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby(); //서버 접속시 JoinLobby 호출
    public override void OnJoinedLobby()//로비 접속시 호출되는 콜백함수
    {
        base.OnJoinedLobby();
        Debug.Log("Entered Lobby!!");
        userId.text = GetUserId();
    }
   
    string GetUserId() //로컬에 저장된 플레이어 이름을 반환하거나 생성함
    {
        string userId = PlayerPrefs.GetString("USER_ID");//로컬에 키값으로 저장되어 있는 USER_ID를 반환
        if (string.IsNullOrEmpty(userId))
        {
            userId = "USER_" + Random.Range(0, 999).ToString("000");
        }
        return userId;
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    { //방 접속에 실패하면 새로운 방 생성
        base.OnJoinRandomFailed(returnCode, message);
        Debug.Log("No rooms!");
        PhotonNetwork.CreateRoom("My Room", new RoomOptions { MaxPlayers = 20 });
    }
   
    public override void OnJoinedRoom()
    { //방에 접속이 되면 탱크 생성
        base.OnJoinedRoom();
        Debug.Log("Entered Room");
        //CreateTank();
        StartCoroutine(this.LoadBattleField()); //메인 씬으로 이동하는 코루틴 함수 호출
    }
    IEnumerator LoadBattleField()
    {
        //씬을 이동하는 동안 포톤 클라우드 서버로부터 네트워크 메시지 수신 중단
        PhotonNetwork.IsMessageQueueRunning = false;
        AsyncOperation ao = Application.LoadLevelAsync("scBattleField"); //백 그라운드 씬 로딩
        yield return ao;
    }
    public void OnClickJoinRandomRoom() //  JoinRandomRoom 버튼 누르면 호출되는 함수
    {
        PhotonNetwork.NickName = userId.text; //로컬 플레이어의 이름을 설정
        PlayerPrefs.SetString("USER_ID", userId.text); //플레이어의 이름을 저장
        PhotonNetwork.JoinRandomRoom(); //무작위로 방에 입장
    }
    public void OnClickCreateRoom() //MaKe Room 버튼 클릭 시 호출될 함수
    {
        string _roomName = roomName.text;
        if (string.IsNullOrEmpty(roomName.text)) //룸 이름이 없거나 Null일 경우 룸 이름 지정
        {
            _roomName = "ROOM_" + Random.Range(0, 999).ToString("000");
        }
        PhotonNetwork.NickName = userId.text; //로컬 플레이어의 이름을 설정
        PlayerPrefs.SetString("USER_ID", userId.text); //플레이어의 이름을 저장

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 20;
        PhotonNetwork.CreateRoom(_roomName, roomOptions, TypedLobby.Default);

    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log("CreateRoom Failed = " + message);
    }
    //생성된 룸 목록이 변경되었을 때 호출되는 콜백 함수
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        //룸 목록을 다시 받았을 때 갱신하기 위해 기존에 생성된 RoomItem을 삭제
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Room_Item"))
        {
            Destroy(obj);
        }
        int rowCount = 0;
        //스크롤 영역 초기화
         scrollContents.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        //수신받은 룸 목록의 정보로 RoomItem을 생성
        foreach (RoomInfo _room in roomList)
        {
            Debug.Log(_room.Name);
            GameObject room = (GameObject)Instantiate(roomItem); // RoomItem 프리팹을 동적으로 생성
            room.transform.SetParent(scrollContents.transform, false); // 생성한 RoomItem 프리팹의 Parent를 지정

            //생성한 RoomItem에 표시하기 위한 텍스트 정보 전달
            RoomData roomData = room.GetComponent<RoomData>();
            roomData.roomName = _room.Name;
            roomData.connectPlayer = _room.PlayerCount;
            roomData.maxPlayer = _room.MaxPlayers;
            //텍스트 정보를 표시
            roomData.DispRoomData();
            //RoomItem의 Button 컴포넌트에 클릭 이벤트 동적으로 연결
            roomData.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate{ OnClickRoomItem(roomData.roomName); });

            scrollContents.GetComponent<GridLayoutGroup>().constraintCount = ++rowCount;  //Grid Layout Group 컴포넌트의 Constraint Count 값을 증가시킴
            scrollContents.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 20);  //스크롤 영역의 높이를 증가시킴
        }
    }
    //RoomItem이 클릭되면 호출된 이벤트 연결 함수
    public void OnClickRoomItem(string roomName)
    {
        PhotonNetwork.NickName = userId.text; //로컬 플레이어의 이름 설정
        PlayerPrefs.SetString("USER_ID", userId.text); // 플레이어 이름 저장
        PhotonNetwork.JoinRoom(roomName); //인자로 전달된 이름에 해당하는 룸으로 입장
    }
    private void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.NetworkClientState.ToString());
    }

}
