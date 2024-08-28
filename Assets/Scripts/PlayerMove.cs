using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using Photon.Pun;

public class PlayerMove : MonoBehaviourPun
{
    // 이동 속력
    public float speed = 5;
    // 캐릭터 컨트롤러
    CharacterController cc;

    // 중력
    float gravity = -9.81f;
    // y 속력
    float yVelocity;
    // 점프 초기 속력
    public float jumpPower = 3;

    // 카메라
    public GameObject cam;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //캐릭터 컨트롤러 가져오자
        cc = GetComponent<CharacterController>();
        // 내 것일 때만 카메라를 활성화하자
        cam.SetActive(photonView.IsMine);
        
    }

    void Update()
    {
        
        Move();
    }

    public void Move()
    {
        //h = Input.GetAxis("Horizontal");
        //v = Input.GetAxis("Vertical");
        //Vector3 dir = new Vector3(h, 0, v);
        //dir = transform.TransformDirection(dir);
        //dir.Normalize();
        //transform.position += dir * speed * Time.deltaTime;

        // 내것만 움직이자
        if (photonView.IsMine)
        {
            // 1. 키보드 wasd 키 입력을 받자
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            // 2. 방향을 정하자.
            Vector3 dirH = transform.right * h;
            Vector3 dirV = transform.forward * v;
            Vector3 dir = dirH + dirV;

            dir.Normalize();

            // 만약에 땅에 있으면 yVelocity 를 0으로 초기화
            if (cc.isGrounded)
            {
                yVelocity = 0;
            }

            // 만약에 Space 바를 누르면
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // yVelocity 를 jumpPower로 설정
                yVelocity = jumpPower;
            }
            // yVelocity 값을 중력에 의해서 변경시키자.
            yVelocity += gravity * Time.deltaTime;


            #region 물리적인 점프 아닌것
            //dir.y 에 yVelocity 값을 셋팅
            dir.y = yVelocity;


            //3.그 방향으로 움직이자
            cc.Move(dir * speed * Time.deltaTime);
            #endregion

            #region 물리적인 점프
            //dir = dir * speed;
            //dir.y = yVelocity;
            //cc.Move(dir * Time.deltaTime);
            #endregion
        }


    }
}
