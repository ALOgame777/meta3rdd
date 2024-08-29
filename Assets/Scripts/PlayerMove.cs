using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using Photon.Pun;

public class PlayerMove : MonoBehaviourPun,  IPunObservable
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

    //서버에서 넘어오는 위치값
    Vector3 receviePos;
    // 서버에서 넘어오는 회전값
    Quaternion receiveRot;
    // 보정 속력
    public float lerpSpeed = 50;

    private void Awake()
    {
        // 커서를 윈도우 창 안에 고정
        Cursor.lockState = CursorLockMode.Confined;
        // 커서를 보이게 설정
        Cursor.visible = true;
    }
    void Start()
    {
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
        // 나의 player 아니라면
        else
        {
            // 위치 보정
            transform.position = Vector3.Lerp(transform.position, receviePos, Time.deltaTime * lerpSpeed);
            // 회전 보정
            transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, Time.deltaTime * lerpSpeed);
        }


    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 만약에 내가 데이터를 보낼 수 있는 상태라면 ( 내 것이라면)
        if (stream.IsWriting)
        {
            // 나의 위치 값을 보낸다.
            stream.SendNext(transform.position);
            // 나의 회전 값을 보낸다.
            stream.SendNext(transform.rotation);
        }
        // 데이터를 받을 수 있는 상태라면 (내 것이 아니라면)
        else if(stream.IsReading)
        {
            // 위치 값을 받자.
            receviePos = (Vector3)stream.ReceiveNext();
            // 회전 값을 받자
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
