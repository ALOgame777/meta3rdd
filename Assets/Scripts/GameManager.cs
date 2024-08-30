using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        //RPC 보내는 빈도 설정
        PhotonNetwork.SendRate = 60;
        //
        PhotonNetwork.SerializationRate = 60;


        // 플레이어를 생성 (현재 Room에 접속 되어 있는 친구들도 보이게)
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
    }

    void Update()
    {
        
    }
}
