using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WaterPutOutChick : MonoBehaviour
{
    PhotonView photonView;
    Collider waterCollider;
    ParticleSystem waterSpray;

    private void Start()
    {
        photonView = GameObject.Find("QuickStartRoomController").GetComponent<PhotonView>();
        waterCollider = GetComponent<Collider>();
        waterSpray = transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    public void SplashWater()
    {
        StartCoroutine(SplashWaterCoroutine());
    }

    IEnumerator SplashWaterCoroutine()
    {
        GameObject farmerParentGameObject = transform.parent.gameObject;
        Vector3 underFarmerPosition = transform.localPosition;
        Quaternion underFarmerRotation = transform.localRotation;
        transform.parent = null;

        waterSpray.Play();
        yield return new WaitForSeconds(0.5f);
        waterSpray.Stop();
        yield return new WaitForSeconds(0.5f);

        waterCollider.enabled = true;

        yield return new WaitForSeconds(1);

        waterCollider.enabled = false;


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
    }
}
