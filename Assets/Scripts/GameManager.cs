using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Spawn 위치를 담아놓을 변수
    public Vector3[] spawnPos;
    public Transform trSpownPointGroup;
    void Start()
    {
       
        SetSpawnPos();
        //RPC 보내는 빈도 설정
        PhotonNetwork.SendRate = 60;
        //
        PhotonNetwork.SerializationRate = 60;

        //내가 위치해야 하는 idx를 알아오자
        int idx = PhotonNetwork.CurrentRoom.PlayerCount - 1;
        // 플레이어를 생성 (현재 Room에 접속 되어 있는 친구들도 보이게)
        PhotonNetwork.Instantiate("Player", spawnPos[idx], Quaternion.identity);

    }

    void Update()
    {
        
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
}
