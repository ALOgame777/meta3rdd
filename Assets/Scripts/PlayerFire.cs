using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerFire : MonoBehaviourPun
{
    // 큐브 Prefab
    public GameObject cubeFactory;
    void Start()
    {
        
    }

    void Update()
    {
        // 만약에 내 것이라면
        if (photonView.IsMine == false) return;
        
       // 1 번 키 누르면
        if (Input.GetKeyDown(KeyCode.Alpha1))
         {
           // 카메라의 앞방향으로 5만큼 떨어진 위치를 구하자
           Vector3 pos = Camera.main.transform.position + Camera.main.transform.forward * 5;
           // 프리팹에서 큐브를 생성하고 싶다.
           PhotonNetwork.Instantiate("Cube", pos, Quaternion.identity);
        }

    }
}
