using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


//방참가자가 들어왔는지, 나갔는지 검사
//방참가자 리스트, 룸 정보 관리
//사용자가 입력한 채팅내용 스크롤 뷰에 표시
//사용자가 입력한 내용 다른 클라이어언트에 전달
public class Chatmanager : MonoBehaviourPunCallbacks
{
    public Text ListText;
    public Text RoomInfoText;
    public Text[] ChatText;
    public InputField ChatInput;

    // Start is called before the first frame update
    void Start()
    {
        RoomRenual();
    }

    void RoomRenual() {
        ListText.text = "";
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            ListText.text += PhotonNetwork.PlayerList[i].NickName + ((i + 1 == PhotonNetwork.PlayerList.Length) ? "" : ", ");
            Debug.Log(PhotonNetwork.PlayerList[i].NickName);
        }
        Debug.Log(PhotonNetwork.CurrentRoom.Name);
        RoomInfoText.text = PhotonNetwork.CurrentRoom.Name + "/" + + PhotonNetwork.CurrentRoom.PlayerCount + "명 / " + "최대 " +PhotonNetwork.CurrentRoom.MaxPlayers +"명";

    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        RoomRenual();
        ChatRPC("<color=yellow>" + newPlayer.NickName + "님이 참가하셨습니다</color>");
        base.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player newPlayer)
    {
        RoomRenual();
        ChatRPC("<color=yellow>" + newPlayer.NickName + "님이 퇴장하셨습니다</color>");
        base.OnPlayerEnteredRoom(newPlayer);
    }

    public void Send() //메시지 전송하는 버튼 매서드
    {
        photonView.RPC("ChatRPC",RpcTarget.All,PhotonNetwork.NickName + " : " + ChatInput.text);
        ChatInput.text = ""; //보낸 뒤 변수 초기화 
    
    }

    [PunRPC]
    void ChatRPC(string msg) //채팅창에 채팅 띄우기
    {
        bool isInput = false;
        for (int i = 0; i < ChatText.Length; i++) //채팅창 길이만큼 반복하면서 검사
        {
            if (ChatText[i].text == "") //채팅창에 공백이 나올때까지 확인
            {//공백이면
                isInput = true;
                ChatText[i].text = msg; //받아온 메세지로 변경
                break;
            
            }
        }
        if (!isInput) //공백이 없을경우에는 한칸씩 올리고 갱신
        {
            for (int i = 1; i < ChatText.Length; i++)
            {
                ChatText[i - 1].text = ChatText[i].text;
            }
            ChatText[ChatText.Length - 1].text = msg;
        }
    }
}
