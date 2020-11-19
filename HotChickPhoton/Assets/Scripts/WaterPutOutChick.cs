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

    private void Start()
    {
        photonView = GameObject.Find("QuickStartRoomController").GetComponent<PhotonView>();
        waterCollider = GetComponent<Collider>();
        waterSpray = transform.GetChild(0).GetComponent<ParticleSystem>();

        farmerController = GameObject.Find("QuickStartRoomController").GetComponent<FarmerController>();
    }

    private void Update()
    {

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
        if (other.tag == "ChickCollider" && other.transform.GetChild(1).gameObject.activeInHierarchy) 
        {
            photonView.RPC("PutOutChick", RpcTarget.All, other.transform.parent.name);
        }

        if (isSpigot && other.tag == "FarmerCollider" && farmerController != null) 
        {
            farmerController.FillBucket();
        }
    }
}
