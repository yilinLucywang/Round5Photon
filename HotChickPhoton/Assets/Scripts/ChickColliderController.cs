using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChickColliderController : MonoBehaviour
{
    PhotonView photonView;

    int lightUpTimer = 10;
    int lightUpTimeLeft = 0;

    private void Start()
    {
        photonView = GameObject.Find("QuickStartRoomController").GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (lightUpTimeLeft > 0)
        {
            lightUpTimeLeft--;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "ChickCollider" && lightUpTimeLeft == 0)
        {
            bool onFire = transform.GetChild(1).gameObject.activeInHierarchy;
            bool otherOnFire = other.transform.GetChild(1).gameObject.activeInHierarchy;

            if (onFire && !otherOnFire)
            {
                lightUpTimeLeft = lightUpTimer;
                photonView.RPC("LightChick", RpcTarget.All, other.transform.parent.gameObject.name);
            }
            else if (otherOnFire && !onFire)
            {
                lightUpTimeLeft = lightUpTimer;
                photonView.RPC("LightChick", RpcTarget.All, transform.parent.gameObject.name);
            }
        }
    }
}
