﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using UnityEngine.UI;

public class HostController : MonoBehaviour
{
    class ChickCompare : IComparer
    {
        int IComparer.Compare(object x, object y)
        {
            return string.Compare(((GameObject)x).name, ((GameObject)y).name);
        }
    }

    int waterLeft;
    int maxWater = 3;
    int splashingWater = 0;
    int maxSplashingWater = 60;

    GameObject[] allChicks;
    GameObject[] allChickObjects;

    GameObject farmerParent;
    public static GameObject farmerObject;
    ParticleSystem waterParticleSystem;
    Collider waterCol;
    Camera myCamera;
    WaterPutOutChick waterController;
    public static Rigidbody rb;
    public PhotonView photonView;
    int pvID;


    public float moveSpeed = 0.5f;
    public float rotationSpeed = 1.0f;

    int frameCounter = 0;


    // Start is called before the first frame update
    void Start()
    {
        photonView = GameObject.Find("QuickStartRoomController").GetComponent<PhotonView>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        myCamera = gameObject.AddComponent<Camera>();
        gameObject.AddComponent<AudioListener>();
        transform.tag = "Camera";
    }

    // Update is called once per frame
    void Update()
    {
        if (farmerParent == null)
        {
            PopulateFarmerAndChicks();
        }

        if (farmerParent == null)
        {
            return;
        }


        MoveCamera();


    }


    void PopulateFarmerAndChicks()
    {
        allChicks = GameObject.FindGameObjectsWithTag("Chick");
        Array.Sort(allChicks, new ChickCompare());

        if (allChicks.Length == 0)
        {
            return;
        }

        allChickObjects = allChicks.Select(chickParent => chickParent.transform.GetChild(0).gameObject).ToArray();

        farmerParent = GameObject.FindGameObjectWithTag("Farmer");
        farmerObject = farmerParent.transform.GetChild(0).gameObject;
        farmerObject.GetComponent<BoxCollider>().enabled = false;
        farmerObject.GetComponent<Rigidbody>().useGravity = false;
        farmerObject.GetComponent<Rigidbody>().isKinematic = true;

        waterParticleSystem = farmerObject.transform.GetChild(1).GetChild(0).GetComponent<ParticleSystem>();
        waterCol = waterParticleSystem.transform.parent.GetComponent<Collider>();





    }

    void MoveCamera()
    {
        transform.RotateAround(transform.position, Vector3.up, Input.GetAxis("Mouse X") * rotationSpeed);
        transform.RotateAround(transform.position, transform.right, - Input.GetAxis("Mouse Y") * rotationSpeed);

        transform.position += transform.forward * Input.GetAxis("Vertical") * moveSpeed;
        transform.position += transform.right * Input.GetAxis("Horizontal") * moveSpeed;
        transform.position += transform.up * Input.GetAxis("UpDown") * moveSpeed;

    }

    [PunRPC]
    public void UpdateChick(string chickName, Vector3 chickPosition, Quaternion chickRotation, string chickNameToUpdate)
    {
        int chickIndex = allChicks.Select((chick, index) => chick.name == chickName ? index : -1).Where(index => index != -1).ToArray()[0];
        if (chickNameToUpdate != null)
        {
            allChickObjects[chickIndex].transform.GetChild(3).GetChild(0).GetComponent<Text>().text = chickNameToUpdate;
        }
        StartCoroutine(UpdateChickLerp(chickIndex, chickPosition, chickRotation));
    }

    IEnumerator UpdateChickLerp(int chickIndex, Vector3 chickPosition, Quaternion chickRotation)
    {

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
    public void UpdateFarmer(Vector3 chickPosition, Quaternion chickRotation, string farmerName)
    {
        if (farmerName == null) 
        { 
            GameObject.Find("FarmerName").GetComponent<Text>().text = farmerName; 
        }
        StartCoroutine(UpdateFarmerLerp(chickPosition, chickRotation));
    }

    IEnumerator UpdateFarmerLerp(Vector3 farmerPosition, Quaternion farmerRotation)
    {
        Vector3 startingPosition = farmerObject.transform.position;
        Quaternion startingRotation = farmerObject.transform.rotation;


        int framesInBetweenMessages = 6;
        for (int frameNum = 1; frameNum <= framesInBetweenMessages; frameNum++)
        {
            float frameLerpPhase = (float)frameNum / (float)framesInBetweenMessages;

            farmerObject.transform.position = Vector3.Lerp(startingPosition, farmerPosition, frameLerpPhase);
            farmerObject.transform.rotation = Quaternion.Lerp(startingRotation, farmerRotation, frameLerpPhase);

            // Wait one frame.
            yield return new WaitForSeconds(0.0166f);
        }

    }

    [PunRPC]
    public void PutOutChick(string remoteChick)
    {
        GameObject localChick = allChicks.Where(chick => chick.name == remoteChick).ToArray()[0];

        localChick.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        localChick.transform.GetChild(0).GetChild(4).gameObject.SetActive(true);

    }

    [PunRPC]
    public void LightChick(string remoteChick)
    {
        GameObject localChick = allChicks.Where(chick => chick.name == remoteChick).ToArray()[0];

        localChick.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
    }


    [PunRPC]
    public void SyncWater(Vector3 waterPosition, Quaternion waterRotation)
    {
        waterParticleSystem.transform.parent.position = waterPosition;
        waterParticleSystem.transform.parent.rotation = waterRotation;

        waterParticleSystem.Play();

        StartCoroutine(EnableWaterCollider());
    }

    IEnumerator EnableWaterCollider()
    {
        yield return new WaitForSeconds(0.2f);

        waterCol.enabled = true;

        yield return new WaitForSeconds(0.55f);

        waterCol.enabled = false;
    }

}
