using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{


    //public float speed = 2.0f;
    //private Vector3 targetPosition;
    // p = p0 + vt

    // 오른쪽으로 정확하게 5만큼 움직이고 싶다.

    //public Vector3 firstPosition;
    //public Vector3 dir = new Vector3(1, 0, 0);
    //public float duration = 5.0f;

    //private float endtime = 0f;
    
    // 이동 거리
    float moveDist = 0;
    // 움직일 수 있니?
    bool isMove = true;


    void Start()
    {
        //targetPosition = transform.position + new Vector3(5, 0, 0);
        //firstPosition = transform.position;
    }

    void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        //endtime += Time.deltaTime;
        //transform.position = firstPosition + dir * endtime;


        //if (endtime >= duration)
        //{
        //    enabled = false;
        //}
        if (isMove == true)
        {
            // 1. 오른쪽으로 움직이자.
            transform.position += Vector3.right * 5 * Time.deltaTime;
            // 이동 거리를 누적
            moveDist += 5 * Time.deltaTime;
            // 2. 만약에 움직인 거리가 5보다 커지면
            if (moveDist >= 5)
            {
                // 3. 멈추자.
                isMove = false;

                // 4. 오버 된 이동거리 만큼 왼쪽으로 이동시키자.
                float overDist = moveDist - 5;
                transform.position += Vector3.left * overDist;
            }
        }
    }
}
