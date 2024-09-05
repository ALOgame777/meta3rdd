using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;

public class HPSystem : MonoBehaviourPun
{
    // 최대 HP
    public float maxHP;
    // 현재 HP
    float currHP;

    //HPBar Image
    public Image hpBar;

    // HP 가 0이 되었을 때 호출되는 함수를 담을 변수
    public Action onDie;
    void Start()
    {
        InitHP();
    }

    void Update()
    {
        
    }

    public void InitHP()
    {
        // 현재 HP를 최대 HP로 설정
        currHP = maxHP;
    }
    public void UpdateHP(float value)
    {
       photonView.RPC(nameof(RpcUpdateHP), RpcTarget.All, value);
    }

    //HP 갱신 함수
    [PunRPC]
    public void RpcUpdateHP(float value)
    {
        // 현재 HP를 value 만큼 더하자
        currHP += value;

        if (hpBar != null)
        {
            // hpbar image 갱신
            hpBar.fillAmount = currHP / maxHP;

        }
        // 만약에 현재 hp가 0보다 작거나 같으면
        if(currHP <= 0)
        {
            print(gameObject.name + "의 HP가 0입니다.");
            if(onDie != null)
            {
                onDie(); 
            }
            else
            {
                // 기본적으로 죽음 처리되는 것 실행
                Destroy(gameObject);
            }

            //if(gameObject.layer == LayerMask.NameToLayer("Player"))
            //{
            //    //플레이어 죽음 처리
            //    PlayerFire pf = GetComponentInParent<PlayerFire>();
            //    pf.OnDie();
            //}
            //else if(gameObject.layer == LayerMask.NameToLayer("ObstacleCube"))
            //{
            //    //큐브 죽음 처리
            //    ObstacleCube cube = GetComponent<ObstacleCube>();
            //    cube.OnDie();
            //}
            //else if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
            //{
            //    //적 죽음 처리
            //}
        }
    }
}

