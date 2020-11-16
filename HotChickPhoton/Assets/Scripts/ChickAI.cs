using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChickAI : MonoBehaviour
{
    public PhotonView photonView;

    int frameCounter = 0;
    Rigidbody rb;

    float moveSpeed = 7.5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {


        MoveChick();

        frameCounter++;
        if (frameCounter == 6)
        {
            Debug.Log("Sending Chick Movement");
            SendChickMovement();
            frameCounter = 0;
        }
    }

    void MoveChick()
    {
        transform.RotateAround(transform.position, transform.up, Random.Range(-1.0f, 1.0f));
        rb.velocity = transform.forward * moveSpeed;
    }

    public void SendChickMovement()
    {
        photonView.RPC("UpdateChick", RpcTarget.Others, transform.parent.name, transform.position, transform.rotation);
    }

}
