using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RpcBullet : MonoBehaviour
{
    // 이동 속력
    public float moveSpeed = 20;
   

    void Update()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
}
