using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerWaiting : MonoBehaviourPun
{
    // 닉네임 UI 담을 변수
    public TMP_Text nickName;


    void Start()
    {
        // 닉네임 UI에 표시
        nickName.text = photonView.Owner.NickName;
        
        // 내것이라면
        if(photonView.IsMine)
        {
            // WaitingMgr 에게 나를 알려주자.

            WaitingSceneMgr.instance.myPlayer = this;
        }
    }

    void Update()
    {
        
    }

    public void SetReady(bool isReady)
    {
        photonView.RPC(nameof(RpcSetReady), RpcTarget.AllBuffered, isReady);
    }
    [PunRPC]
    void RpcSetReady(bool isReady)
    {
        if (isReady)
        {
            // nickName 색을 파란색으로 설정
            nickName.color = Color.blue;
        }
        else
        {
            nickName.color = Color.white;
        }
        // 모두 Readt 했는지 체크
        WaitingSceneMgr.instance.CheckAllready(isReady);

    }
}
