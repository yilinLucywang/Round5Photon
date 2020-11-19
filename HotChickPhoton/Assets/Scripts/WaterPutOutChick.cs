using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WaterPutOutChick : MonoBehaviour
{
    PhotonView photonView;
    Collider waterCollider;
    ParticleSystem waterSpray;

    FarmerController farmerController;

    public bool isSpigot = true;

    int putOutTimer = 10;
    int putOutTimeLeft = 0;

    private void Start()
    {
        photonView = GameObject.Find("QuickStartRoomController").GetComponent<PhotonView>();
        waterCollider = GetComponent<Collider>();
        waterSpray = transform.GetChild(0).GetComponent<ParticleSystem>();

        farmerController = GameObject.Find("QuickStartRoomController").GetComponent<FarmerController>();
    }

    private void Update()
    {
        if (putOutTimeLeft > 0)
        {
            putOutTimeLeft--;
        }

    }

    public bool SplashWater()
    {
        if (transform.parent == null) 
        {
            return false;
        }

        photonView.RPC("SyncWater", RpcTarget.All, transform.parent.position, transform.parent.rotation);

        StartCoroutine(SplashWaterCoroutine());

        return true;
    }

    IEnumerator SplashWaterCoroutine()
    {
        GameObject farmerParentGameObject = transform.parent.gameObject;
        Vector3 underFarmerPosition = transform.localPosition;
        Quaternion underFarmerRotation = transform.localRotation;
        transform.parent = null;

        waterSpray.Play();
        yield return new WaitForSeconds(0.2f);

        waterCollider.enabled = true;

        yield return new WaitForSeconds(0.55f);

        waterCollider.enabled = false;


        transform.parent = farmerParentGameObject.transform;
        transform.localPosition = underFarmerPosition;
        transform.localRotation = underFarmerRotation;

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "ChickCollider" && putOutTimeLeft == 0) 
        {
            putOutTimeLeft = putOutTimer;
            photonView.RPC("PutOutChick", RpcTarget.All, other.transform.parent.name);
        }

        if (isSpigot && other.tag == "FarmerCollider" && farmerController != null) 
        {
            farmerController.FillBucket();
        }
    }
}
