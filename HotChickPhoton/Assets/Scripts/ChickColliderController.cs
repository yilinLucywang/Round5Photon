using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChickColliderController : MonoBehaviour
{
    PhotonView photonView;

    int lightUpTimer = 10;
    int lightUpTimeLeft = 0;

    bool isLighting = false;
    float flamingTime = 2; 
    float flamingTimeLeft;
    int lighterCount = 0;

    bool onFire = false;


    private void Start()
    {
        photonView = GameObject.Find("QuickStartRoomController").GetComponent<PhotonView>();

        onFire = transform.GetChild(1).gameObject.activeInHierarchy;
    }

    private void Update()
    {

        if (isLighting)
        {
            flamingTimeLeft -= Time.deltaTime;
            if(flamingTimeLeft <= 0)
            {
                onFire = true;
                isLighting = false;
                flamingTimeLeft = flamingTime;
                lighterCount = 0;

                photonView.RPC("LightChick", RpcTarget.All, transform.parent.gameObject.name);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        // flaming chick
        if (other.tag == "ChickCollider" && other.transform.GetChild(1).gameObject.activeInHierarchy)
        {
            isLighting = true;
            lighterCount += 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "ChickCollider" && other.transform.GetChild(1).gameObject.activeInHierarchy)
        {
            lighterCount = lighterCount - 1; 
            if(lighterCount <= 0)
            {
                isLighting = false;
                flamingTimeLeft = flamingTime;
            }
        }
    }

    // private void OnTriggerStay(Collider other)
    // {
    //     if (other.tag == "ChickCollider" && lightUpTimeLeft == 0)
    //     {
    //         bool onFire = transform.GetChild(1).gameObject.activeInHierarchy;
    //         bool otherOnFire = other.transform.GetChild(1).gameObject.activeInHierarchy;

    //         if (onFire && !otherOnFire)
    //         {
    //             lightUpTimeLeft = lightUpTimer;
    //             photonView.RPC("LightChick", RpcTarget.All, other.transform.parent.gameObject.name);
    //         }
    //         else if (otherOnFire && !onFire)
    //         {
    //             lightUpTimeLeft = lightUpTimer;
    //             photonView.RPC("LightChick", RpcTarget.All, transform.parent.gameObject.name);
    //         }
    //     }
    // }

}
