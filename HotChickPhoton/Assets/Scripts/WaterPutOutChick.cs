using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WaterPutOutChick : MonoBehaviour
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
            photonView.RPC("PutOutChick", RpcTarget.All, other.transform.parent.name);
        }
    }
}
