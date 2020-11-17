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
    int timeSinceLastTurn = 0;

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
        bool shouldTurn = Random.Range(0, 90 - timeSinceLastTurn) == 0;
        if (shouldTurn)
        {
            timeSinceLastTurn = 0;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, Random.Range(0.0f, 360.0f), transform.eulerAngles.z);
        }
        else 
        {
            timeSinceLastTurn++;
        }

        rb.velocity = transform.forward * moveSpeed;
    }

    public void SendChickMovement()
    {
        photonView.RPC("UpdateChick", RpcTarget.Others, transform.parent.name, transform.position, transform.rotation, "ChickBot");
    }

}
