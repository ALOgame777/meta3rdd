using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviourPun
{
    public float moveSpeed = 10;

    //Rigidbody
    Rigidbody rb;

    void Start()
    {
        // 내것일때만
        if (photonView.IsMine)
        {
            rb = GetComponent<Rigidbody>();
            rb.velocity = transform.forward * moveSpeed;
        }
    }

    void Update()
    {
        //transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
}
