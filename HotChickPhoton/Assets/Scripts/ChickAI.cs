using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChickAI : MonoBehaviour
{
    public PhotonView photonView;
    Rigidbody rb;
    bool applicationRunning;

    public float moveSpeed = 4;
    public float rotationSpeed = 1.2f;

    float virtualMouseRotation = 0;
    int frameCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        applicationRunning = true;
        rb = GetComponent<Rigidbody>();

        StartCoroutine(AICoroutine());
    }

    // Update is called once per frame
    void Update()
    {




        transform.RotateAround(transform.position, transform.up, virtualMouseRotation * rotationSpeed);
        rb.velocity = transform.forward * moveSpeed;


        frameCounter++;
        if (frameCounter == 6)
        {
            Debug.Log("Sending Chick Movement");
            photonView.RPC("UpdateChick", RpcTarget.Others, transform.parent.name, transform.position, transform.rotation);
            frameCounter = 0;
        }


    }

    IEnumerator AICoroutine() 
    {
        float lastRotation = 0;
        float lastDifference = 0;

        while (applicationRunning)
        {
            float mouseDifference;
            if (lastDifference > 0.01f)
            {
                mouseDifference = Random.Range(-0.02f, 0.06f);
            }
            else if (lastDifference < -0.01f)
            {
                mouseDifference = Random.Range(-0.06f, 0.02f);
            }
            else 
            {
                mouseDifference = Random.Range(-0.05f, 0.05f);
            }


            virtualMouseRotation = Mathf.Max(-0.5f, Mathf.Min(0.5f, lastRotation + mouseDifference));

            lastRotation = virtualMouseRotation;
            lastDifference = mouseDifference;
            yield return null;
        }
    }

    private void OnApplicationQuit()
    {
        applicationRunning = false;
    }

}
