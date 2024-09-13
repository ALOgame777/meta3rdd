using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;
    // Spawn 위치를 담아놓을 변수
    public Vector3[] spawnPos;
    public Transform trSpownPointGroup;

    // 모든 player의 photon View 가지고 있는 변수
    public PhotonView[] allPlayer;

    // GameScene 으로 넘어온 player의 갯수를 체크하는 변수
    int enterPlayerCnt;

    // 현재 총을 쏠 수 있는 Player idx
    int turnIdx = -1;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
       
        SetSpawnPos();
        //RPC 보내는 빈도 설정
        PhotonNetwork.SendRate = 60;
        //
        PhotonNetwork.SerializationRate = 60;

        //내가 위치해야 하는 idx를 알아오자
        int idx = ProjectMgr.Get().orderInRoom;
        // 플레이어를 생성 (현재 Room에 접속 되어 있는 친구들도 보이게)
        PhotonNetwork.Instantiate("Player", spawnPos[idx], Quaternion.identity);
        // 모든 플레이어 담을 변수 공간 할당
        allPlayer = new PhotonView[PhotonNetwork.CurrentRoom.MaxPlayers];

        // 더 이상 이방에 접속을 하지 못하게 하자


    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            //방을 떠나자
            PhotonNetwork.LeaveRoom();
        }

    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        // 방장에 의해서 Scene 전환되는 옵션 비활성화
        PhotonNetwork.AutomaticallySyncScene = false;
        // 마우스 커서 Lock mode 풀자
        Cursor.lockState = CursorLockMode.None;
        
    }

    public override void OnConnectedToMaster()
    {
        // 자동으로 master server에 접속 시도
        base.OnConnectedToMaster();
        // lOBBY 씬으로 전환
        PhotonNetwork.LoadLevel("LobbyScene");
    }


    void SetSpawnPos()
    {
        // 최대 인원 만큼 spanwPos 의 공간을 할당
        int maxPlayer = PhotonNetwork.CurrentRoom.MaxPlayers;
        spawnPos = new Vector3[maxPlayer];

        // spwnPos 간의 간격(각도)
        float angle = 360.0f / maxPlayer;

        // maxplayer만틈 반복
        for (int i = 0; i < maxPlayer; i++)
        {
            // trSpownPointGroup 회전 ( i * angle) 만큼
            trSpownPointGroup.eulerAngles  = new Vector3(0, i * angle, 0);
            //trSpownPointGroup  앞 방향으로 2만큼 떨어진 위치 구하자
            spawnPos[i] = trSpownPointGroup.position + trSpownPointGroup.forward * 2;
            
        }
        
    }

    public void AddPlayer(PhotonView pv, int order)
    {
        enterPlayerCnt++;
        allPlayer[order] = pv;
        if (enterPlayerCnt == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            // 방장일때만
            if (PhotonNetwork.IsMasterClient)
            {
                // 턴 시작
                ChangeTurn();
            }
        }
    }

    // 방장아 다음 턴으로 넘겨줘
    public void ChangeTurn()
    {
        photonView.RPC(nameof(RpcChangeTurn), RpcTarget.MasterClient);
    }
    //방장에서 호출된다.
    [PunRPC]    
    void RpcChangeTurn()
    {
        // turnIdx를 최대인원 값보다 작게 만들자.
        turnIdx = ++turnIdx % allPlayer.Length;
        print("현재 턴 : " + turnIdx);
        PlayerFire pf = allPlayer[turnIdx].GetComponent<PlayerFire>();
        pf.ChangeTurn(true);
    }
}
