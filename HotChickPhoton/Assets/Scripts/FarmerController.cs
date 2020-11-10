using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class FarmerController : MonoBehaviour
{
    class ChickCompare : IComparer
    {
        int IComparer.Compare(object x, object y)
        {
            return string.Compare(((GameObject)x).name, ((GameObject)y).name);
        }
    }

    public bool hasWater = true;

    GameObject[] allChicks;
    GameObject[] allChickObjects;

    GameObject myFarmerParent;
    public static GameObject myFarmerObject;
    Camera myCamera;
    GameObject waterCollider;
    public static Rigidbody rb;
    public PhotonView photonView;
    int pvID;


    public float moveSpeed = 3.5f;
    public float rotationSpeed = 1.2f;

    int frameCounter = 0;


    // Start is called before the first frame update
    void Start()
    {
        photonView = GameObject.Find("QuickStartRoomController").GetComponent<PhotonView>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (myFarmerParent == null) 
        {
            ClaimFarmer();
        }

        if (myFarmerParent == null) 
        {
            return;
        }

        Debug.Log(allChicks[0].name + "  " + allChicks[1].name + "  " + allChicks[2].name);

        MoveFarmer();




        if (hasWater && Input.GetAxis("Fire1") > 0) 
        {
            SplashWater();
        }



        frameCounter++;
        if (frameCounter == 6) 
        {
            SendFarmerMovement();
            frameCounter = 0;
        }


    }

    void ClaimFarmer() 
    {
        allChicks = GameObject.FindGameObjectsWithTag("Chick");
        Array.Sort(allChicks, new ChickCompare());
        allChickObjects = allChicks.Select(chickParent => chickParent.transform.GetChild(0).gameObject).ToArray();

        if (allChicks.Length == 0) 
        {
            return;
        }


        myFarmerParent = GameObject.Find("FarmerParent");

        if (myFarmerParent != null) 
        {

            myFarmerObject = myFarmerParent.transform.GetChild(0).gameObject;
            myCamera = myFarmerObject.transform.GetChild(0).GetComponent<Camera>();
            waterCollider = myFarmerObject.transform.GetChild(1).gameObject;

            rb = myFarmerObject.GetComponent<Rigidbody>();

            myCamera.gameObject.SetActive(true);

        }



    }

    void SplashWater() 
    {
        hasWater = false;
        StartCoroutine(SplashWaterCoroutine());
    }

    IEnumerator SplashWaterCoroutine() 
    {
        // Play splashing water animation and unparent from farmer so it doesnt move with him.

        yield return new WaitForSeconds(0.5f);

        waterCollider.SetActive(true);

        yield return new WaitForSeconds(1);

        waterCollider.SetActive(false);

        yield return new WaitForSeconds(1);

        hasWater = true;
    }

    void MoveFarmer()
    {
        myFarmerObject.transform.RotateAround(myFarmerObject.transform.position, myFarmerObject.transform.up, Input.GetAxis("Mouse X") * rotationSpeed);
        rb.velocity = myFarmerObject.transform.forward * moveSpeed;
    }

    void SendFarmerMovement() 
    {
        photonView.RPC("UpdateFarmer", RpcTarget.Others, myFarmerObject.transform.position, myFarmerObject.transform.rotation);
    }

    [PunRPC]
    public void UpdateChick(string chickName, Vector3 chickPosition, Quaternion chickRotation) 
    {
        Debug.Log("Updating chick " + chickName);
        StartCoroutine(UpdateChickLerp(chickName, chickPosition, chickRotation));
    }

    IEnumerator UpdateChickLerp(string chickName, Vector3 chickPosition, Quaternion chickRotation)
    {
        int chickIndex = allChicks.Select((chick, index) => chick.name == chickName ? index : -1).Where(index => index != -1).ToArray()[0];

        Vector3 startingPosition = allChickObjects[chickIndex].transform.position;
        Quaternion startingRotation = allChickObjects[chickIndex].transform.rotation;


        int framesInBetweenMessages = 6;
        for (int frameNum = 1; frameNum <= framesInBetweenMessages; frameNum++)
        {
            float frameLerpPhase = (float)frameNum / (float)framesInBetweenMessages;

            allChickObjects[chickIndex].transform.position = Vector3.Lerp(startingPosition, chickPosition, frameLerpPhase);
            allChickObjects[chickIndex].transform.rotation = Quaternion.Lerp(startingRotation, chickRotation, frameLerpPhase);

            // Wait one frame.
            yield return new WaitForSeconds(0.0166f);
        }

    }

    [PunRPC]
    public void PutOutChick(string remoteChick)
    {
        GameObject localChick = allChicks.Where(chick => chick.name == remoteChick).ToArray()[0];

        localChick.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
    }

    [PunRPC]
    public void LightChick(string remoteChick)
    {
        GameObject localChick = allChicks.Where(chick => chick.name == remoteChick).ToArray()[0];

        localChick.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
    }

}
