using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using Photon.Pun;
using TMPro;

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

    // Animator
    Animator anim;
    // 보정 속력
    public float lerpSpeed = 50;


    // AD 키 입력 받을 변수
    float h;
    // WS 키 입력 받을 변수
    float v;

    // LookPos
    public Transform lookPos;

    // 닉네임 UI
    public TMP_Text nickName;
    void Start()
    {
        
        if (photonView.IsMine)
        {
            // 커서를 윈도우 창 안에 고정
            Cursor.lockState = CursorLockMode.Locked;
        }
         //캐릭터 컨트롤러 가져오자
        cc = GetComponent<CharacterController>();
        // 내 것일 때만 카메라를 활성화하자
        cam.SetActive(photonView.IsMine);
        // animator 가져오자
        anim = GetComponentInChildren<Animator>();

        // 닉네임 UI 에 해당 캐릭터의 주인의 닉네임 설정
        nickName.text = photonView.Owner.NickName;
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
        // 마우스 lockMode가 none이면 (마우스 포인터가 활성화 되어 있다면) 함수를 나가자
        if (Cursor.lockState == CursorLockMode.None) return;

        // 내것만 움직이자
        if (photonView.IsMine)
        {
            // 1. 키보드 wasd 키 입력을 받자
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");

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

        // anim을 이용해서 h, v 값을 전달
        anim.SetFloat("DirH", h);
        anim.SetFloat("DirV", v);


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

            // 나의 h 값
            stream.SendNext(h);
            // 나의 v값
            stream.SendNext(v);

            // LookPos의 위치 값을 보낸다.
            stream.SendNext(lookPos.position);
            

            
        }
        // 보낼 때의 순서와 받았을 때 순서가 같아야 오류가 안난다.
        // 데이터를 받을 수 있는 상태라면 (내 것이 아니라면)
        else if(stream.IsReading)
        {
            // 위치 값을 받자.
            receviePos = (Vector3)stream.ReceiveNext();
            // 회전 값을 받자
            receiveRot = (Quaternion)stream.ReceiveNext();
            // 서버에서 전달 되는 h 값 받자.
            h = (float)stream.ReceiveNext();
            // 서버에서 전달 되는 v 값 받자.
            v = (float)stream.ReceiveNext();
            // LookPos의 위치값을 받자.
            lookPos.position = (Vector3)stream.ReceiveNext();
        }
    }
}
