using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCube : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDie()
    {
        // 크기를 현재 Scale 20% 줄이자
        transform.localScale *= 0.8f;
        // hpSystem 가져오자
        HPSystem hpSystem = GetComponentInChildren<HPSystem>();
        // onDie 변수에 OnDie 함수 설정
        hpSystem.onDie = OnDie;
    }
}
