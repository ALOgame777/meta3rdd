using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;


public class PlayerFire : MonoBehaviourPun
{
    // 큐브 Prefab
    public GameObject cubeFactory;

    // Fx Prefab
    public GameObject fxFactory;

    // 총알 프리팹
    public GameObject bulletFactory;

    // 총구의 Transform
    public Transform firePos;
    void Start()
    {
        
    }

    void Update()
    {
        // 만약에 내 것이라면
        if (photonView.IsMine == false) return;

        // 마우스 왼쪽 버튼 누르면
        if (Input.GetMouseButton(0))
        {
            // 총알 공장에서 총알을 생성, 총구위치 셋팅, 총구회전 셋팅
            //Instantiate(bulletFactory, firePos.position, Camera.main.transform.rotation);
            photonView.RPC(nameof(Createbullet), RpcTarget.All, firePos.position, Camera.main.transform.rotation);

        }
       // 마우스 오른쪽 버튼 누르면
        if (Input.GetMouseButton(1))
        {
            // 카메라 위치, 카메라 앞방향으로 된 Ray를 만들자.
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            // 만들어진 Ray를 이용해서 RayCast 하자
            RaycastHit hit;
            // 만약 부딪힌 지점이 있으면
            if (Physics.Raycast(ray, out hit))
            {
                // 폭발 효과를 생성하고 부딪힌 위치에 놓자.
                //CreateFX(hit.point);
                photonView.RPC(nameof(CreateFX), RpcTarget.All, hit.point);
            }
        }
        
       // 1 번 키 누르면
        if (Input.GetKeyDown(KeyCode.Alpha1))
         {
            // 카메라의 앞방향으로 5만큼 떨어진 위치를 구하자
            Vector3 pos = Camera.main.transform.position + Camera.main.transform.forward * 5;
            //// 프리팹에서 큐브를 생성하고 싶다.
            //PhotonNetwork.Instantiate("Cube", pos, Quaternion.identity);
            photonView.RPC(nameof(CreateCube), RpcTarget.All, pos);
        }

    }
    [PunRPC]
    public void Createbullet(Vector3 position, Quaternion rotation)
    {
        Instantiate(bulletFactory, position, rotation);
    }


    [PunRPC]
    public void CreateFX(Vector3 position)
    {
        GameObject fx = Instantiate(fxFactory);
        fx.transform.position = position;
    }
    [PunRPC]
    public void CreateCube(Vector3 position)
    {
        Instantiate(cubeFactory, position, Quaternion.identity);
    }
}
