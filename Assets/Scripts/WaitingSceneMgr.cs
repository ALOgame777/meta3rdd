using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaitingSceneMgr : MonoBehaviour
{
    // 자기 자신을 담을 static 변수
    public static WaitingSceneMgr instance;
    // playerWaiting prefab
    public GameObject playerWaitingFactory;

    // 나의 해당되는 PlayerWaiting을 가지고 있는 변수
    public PlayerWaiting myPlayer;

    // 현재 Ready를 한 인원수
    public int readyCount;

    // 내가 Ready를 했는지 여부
    public bool isReady;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        // 방장이 Scene을 전환하면 나머지는 자동으로 같이 이동해라!
        PhotonNetwork.AutomaticallySyncScene = true;
        // 현재 방에 최대 인원
        int max = 4;
        
        // 내가 몇번째로 들어왔는지 가지고 있는 변수
        int idx = 0;
        
        // 캐릭터들 간의 간격
        float distance = 3;
        

        // 만약에 내가 포톤에 접속 되어 있다면 
        if(PhotonNetwork.IsConnected)
        {
            // max 값을 방의 최대 인원으로
            max = PhotonNetwork.CurrentRoom.MaxPlayers;
            // idx 값을 내가 몇번째 들어왔는지
            idx = PhotonNetwork.CurrentRoom.PlayerCount - 1;
            //ProjectMgr에 나의 순서를 저장
            ProjectMgr.Get().orderInRoom = idx;
        }

        // 총길이 / 2 
        float xDist = (max - 1) * distance * 0.5f;
        //idx 에 따라서 좌표를 구하자
        Vector3 pos = new Vector3(idx * distance - xDist, 0, 0);


        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Instantiate(playerWaitingFactory.name, pos, Quaternion.identity);
        }
        else
        {
            // 플레이어 생성
            Instantiate(playerWaitingFactory, pos, Quaternion.identity);
        }
    }

    void Update()
    {
        
    }

    public void OnClickReady()
    {
        //나에 해당되는 캐릭터의 SetReady 함수를 호출하자
        isReady = !isReady;
        myPlayer.SetReady(isReady);
    }
    // 모두 ready를 했는지 체크 하는 함수
    public void CheckAllready(bool playerReady)
    {
        // 만약에 방장이 아니라면 함수를 나가자
        if(PhotonNetwork.IsMasterClient == false) return;

        // Readyt 한 인원수 하나 증가 / 감소
        if (playerReady)
        {
            readyCount++;
        }
        else
        {
            readyCount--;
        }
        //만약에 모두 Ready를 했다면
        if(readyCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            //GameScene으로 이동한다.
            PhotonNetwork.LoadLevel("GameScene");
        }
    }
}
