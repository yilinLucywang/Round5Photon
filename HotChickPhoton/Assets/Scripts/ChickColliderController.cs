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

    float dryingTime = 3;
    float dryingTimeLeft; 

    bool isDrying = false;
    bool onFire = false;
    bool isWet = false;
    bool isPutOut = false;


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

        if(isPutOut){
            photonView.RPC("PutOutChick", RpcTarget.All, transform.parent.gameObject.name);
            isPutOut = false;
        }

        if (isWet){
            if(isDrying){
                dryingTimeLeft -= Time.deltaTime;
                if(dryingTimeLeft <= 0){
                    isWet = false; 
                    isDrying = false;
                    dryingTimeLeft = dryingTime;
                    lighterCount = 0;
                }
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        // flaming chick
        if (other.tag == "ChickCollider" && other.transform.GetChild(1).gameObject.activeInHierarchy)
        {
            if(! isWet){
                isLighting = true;
                lighterCount += 1;
            }
            else if(isWet){
                isDrying = true; 
                lighterCount += 1;
            }
        }
        
        //else if the other chick is wet and current chick is on fire
        //TODO: change the second condition to wet chick
        else if(other.tag == "ChickCollider" && other.transform.GetChild(1).gameObject.activeInHierarchy){
            if(transform.GetChild(1).gameObject.activeInHierarchy){
                isPutOut = true; 
                isWet = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "ChickCollider" && other.transform.GetChild(1).gameObject.activeInHierarchy)
        {
            lighterCount = lighterCount - 1; 
            if(lighterCount <= 0)
            {
                if(! isDrying){
                    isLighting = false;
                    flamingTimeLeft = flamingTime;
                }
                else{
                    isDrying = false; 
                    dryingTimeLeft = dryingTime;
                }
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
