using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMgr : MonoBehaviourPunCallbacks
{
    // Input room Name
    public TMP_InputField inputRoomName;
    // Input Max Player
    public TMP_InputField inputMaxPlayer;
    // Create Button
    public Button btnCreate;
    //Join Button
    public Button btnJoin;
    //전체 방에 대한 정보
    Dictionary<string, RoomInfo> allRoomInfo = new Dictionary<string, RoomInfo>();

    void Start()
    {
        // 로비 진입
        PhotonNetwork.JoinLobby();
        // inputRoomName 의 내용이 변경 될 때 호출되는 함수 등록
        inputRoomName.onValueChanged.AddListener(OnValueChangedRoomName);
        // inputMaxPlayer의 내용이 변경 될 때 호출되는 함수 등록
        inputMaxPlayer.onValueChanged.AddListener(OnValueChangedMaxPlayer);
        #region Dictionary 사용 예제
        //Dictionary<int, string> dic = new Dictionary<int, string>();
        // 두개 같은 것
        //dic.Add(12, "값1");

        // value 에 대한 값을 추가/ 수정
        //dic[12] = "값1";
        //dic[12] = "안녕하세요";
        //dic[13] = "값2";
        //print(dic[12]); // 안녕하세요
        //print(dic[13]); // 값 2

        // 삭제
        //dic.Remove(12);
        //print(dic[12]); // 오류
        #endregion
    }

    void Update()
    {
        
    }

    // 방이름과 최대인원의 값이 있을 때만 Create 버튼 활성화
    // 방이름에 값이 있으면 join 버튼 활성화

    //  join &  Create 버튼을 활성화 / 비활성화
    void OnValueChangedRoomName(string roomName)
    {
        // Join 버튼 활성 / 비활성
        btnJoin.interactable = roomName.Length > 0;

        // Create 버튼 활성화 / 비활성화
        btnCreate.interactable = roomName.Length > 0 && inputMaxPlayer.text.Length > 0;
        
    }
    // Create 버튼을 활성화 / 비활성화
    void OnValueChangedMaxPlayer(string maxPlayer)
    {
        // Create 버튼 활성화/ 비활성화
        btnCreate.interactable = maxPlayer.Length > 0 && inputRoomName.text.Length > 0;
    }

    public void CreateRoom()
    {
        // 방 옵션 설정
        RoomOptions options = new RoomOptions();
        // 최대 인원 설정
        options.MaxPlayers = int.Parse(inputMaxPlayer.text);
        // 방 옵션을 기반으로 방을 생성
        PhotonNetwork.CreateRoom(inputRoomName.text, options);
    }
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("방 생성 완료");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("방 생성 실패 : " + message);
    }
    public void JoinRoom()
    {
        // 방 입장 요청
        PhotonNetwork.JoinRoom(inputRoomName.text);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("방 입장 완료");
        PhotonNetwork.LoadLevel("GameScene");

    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("방 입장 실패" + message);
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("로비 진입 성공");
    }

    //로비에 있을 때 방에 대한 정보들이 변경되어 호출되는 함수
    // roomList : 전체 방목록  x , 변경된 방 정보
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        // 방 목록 UI를 전체 삭제
        RemoveRoomList();

        // 전체 방 정보를 갱신
        UpdateRoomList(roomList);
        CreateRoomList();
        // allRoomInfo를 기반으로 방목록 UI를 만들자

        //for (int i = 0; i < roomList.Count; i++) 
        //{
        //    print(roomList[i].Name + ", " + roomList[i].PlayerCount + " , " + roomList[i].RemovedFromList);

        //}

    }

     void UpdateRoomList(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            // allRoomInfo 에 roomList의 i 번째 정보가 있니? (roomList[i]의 방이름이 key 값으로 존재하니?)
            if (allRoomInfo.ContainsKey(roomList[i].Name))
            {
                // allRoomInfo 수정 or 삭제
                // 삭제 된 방이니?
                if (roomList[i].RemovedFromList == true)
                {
                    allRoomInfo.Remove(roomList[i].Name);
                }
                // 수정
                else
                {
                    allRoomInfo[roomList[i].Name ] = roomList[i];
                }
            }
            else
            {
                // allRoomInfo 추가
                //allRoomInfo.Add(roomList[i].Name, roomList[i]);
                allRoomInfo[roomList[i].Name] = roomList[i];
            }
        }
    }

    // RoomItem 의 prefab
    public GameObject roomItemFactory;
    // scrollView의 Conent Trasfrom
    public RectTransform trContent;
    void CreateRoomList()
    {
        foreach (RoomInfo info in allRoomInfo.Values)
        {
            // roomItem prefab을 이용해서 roomItem 을 만든다.
            GameObject go = Instantiate(roomItemFactory, trContent);
            // 만들어진 roomItem 의 내용을 변경
            //tmp_text 가져오자
            TMP_Text roomItem =  go.GetComponentInChildren<TMP_Text>();
            roomItem.text = info.Name + " ( " + info.PlayerCount + "/" + info.MaxPlayers + ")";

        }
    }

    void RemoveRoomList()
    {
        // trContent에 있는 자식을 모두 삭제
        for(int i = 0; i < trContent.childCount; i++)
        {
            Destroy(trContent.GetChild(i).gameObject);
        }
    }
}
