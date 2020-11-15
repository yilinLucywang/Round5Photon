using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WaterPutOutChick : MonoBehaviour
{
    public bool isSpigot = true;

    PhotonView photonView;
    Collider waterCollider;
    ParticleSystem waterSpray;

    FarmerController farmerController;

    private void Start()
    {
        photonView = GameObject.Find("QuickStartRoomController").GetComponent<PhotonView>();
        waterCollider = GetComponent<Collider>();
        waterSpray = transform.GetChild(0).GetComponent<ParticleSystem>();

        farmerController = GameObject.Find("QuickStartRoomController").GetComponent<FarmerController>();
    }

    public bool SplashWater()
    {
        if (transform.parent != null)
        {
            StartCoroutine(SplashWaterCoroutine());
            return true;
        }

        return false;
    }

    IEnumerator SplashWaterCoroutine()
    {
        GameObject farmerParentGameObject = transform.parent.gameObject;
        Vector3 underFarmerPosition = transform.localPosition;
        Quaternion underFarmerRotation = transform.localRotation;
        transform.parent = null;

        waterSpray.Play();
        photonView.RPC("SyncWater", RpcTarget.Others, waterSpray.transform.position, waterSpray.transform.rotation);
        yield return new WaitForSeconds(0.75f);

        waterCollider.enabled = true;

        yield return new WaitForSeconds(0.25f);

        waterCollider.enabled = false;

        yield return new WaitForSeconds(1.0f);

        transform.parent = farmerParentGameObject.transform;
        transform.localPosition = underFarmerPosition;
        transform.localRotation = underFarmerRotation;

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "ChickCollider") 
        {
            photonView.RPC("PutOutChick", RpcTarget.All, other.transform.parent.name);
        }

        if (other.tag == "FarmerCollider" && isSpigot)
        {
            if (farmerController != null) 
            {
                farmerController.FillBucket();
            }
        }
    }
}
