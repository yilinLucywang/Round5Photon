using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChickColliderController : MonoBehaviour
{
    PhotonView photonView;

    private void Start()
    {
        photonView = GameObject.Find("QuickStartRoomController").GetComponent<PhotonView>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "ChickCollider")
        {
            bool onFire = transform.GetChild(1).gameObject.activeInHierarchy;
            bool otherOnFire = other.transform.GetChild(1).gameObject.activeInHierarchy;

            if (onFire && !otherOnFire)
            {
                photonView.RPC("LightChick", RpcTarget.All, other.transform.parent.gameObject.name);
            }
            else if (otherOnFire && !onFire)
            {
                photonView.RPC("LightChick", RpcTarget.All, transform.parent.gameObject.name);
            }
        }
    }
}
