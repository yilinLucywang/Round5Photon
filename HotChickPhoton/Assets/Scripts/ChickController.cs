using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using UnityEngine.UI;

public class ChickController : MonoBehaviour
{
    class ChickCompare : IComparer
    {
        int IComparer.Compare(object x, object y)
        {
            return string.Compare(((GameObject)x).name, ((GameObject)y).name);
        }
    }


    GameObject fireObject;

    public bool onFire;
    public PhotonView photonView;
    int pvID;
    public int myChickNumber = -1;

    bool claimedChick = false;
    GameObject[] allChicks;
    GameObject[] allChickObjects;
    GameObject myChickParent;
    int myChickIndex = -1;
    public static GameObject myChickObject;
    Camera myCamera;
    public static Rigidbody rb;

    GameObject farmerObject;
    ParticleSystem waterParticleSystem;
    Collider waterCol;

    public float moveSpeed = 7.5f;
    public float wetSpeed = 5.0f; 
    public float rotationSpeed = 1.5f;

    int frameCounter = 0;
    private bool isWet = false;

    AudioController splashSound;
    AudioController catchFireSound;
    AudioController onFireSound;
    AudioController wetWalkSound;
    AudioController bawkSound;

    public int totalScore = 0;

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

        if (!claimedChick)
        {
            ClaimChick();
        }




        MoveChick();





        frameCounter++;
        if (frameCounter == 6)
        {
            Debug.Log("Sending Chick Movement");
            SendChickMovement();
            frameCounter = 0;
        }


        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            photonView.RPC("BawkAt", RpcTarget.All, myChickObject.transform.position);
        }


    }

    public void AddPoints(int pointsToAdd) 
    {
        totalScore += pointsToAdd;
    }

    void ClaimChick()
    {
        allChicks = GameObject.FindGameObjectsWithTag("Chick");
        Array.Sort(allChicks, new ChickCompare());
        allChickObjects = allChicks.Select(chickParent => chickParent.transform.GetChild(0).gameObject).ToArray();

        if (allChicks.Length == 0)
        {
            return;
        }

        farmerObject = GameObject.FindGameObjectWithTag("Farmer").transform.GetChild(0).gameObject;
        farmerObject.GetComponent<BoxCollider>().enabled = false;
        farmerObject.GetComponent<Rigidbody>().useGravity = false;
        farmerObject.GetComponent<Rigidbody>().isKinematic = true;

        waterParticleSystem = farmerObject.transform.GetChild(1).GetChild(0).GetComponent<ParticleSystem>();
        waterCol = waterParticleSystem.transform.parent.GetComponent<Collider>();

        ChickStorage chickStorage = GameObject.Find("ChickStorage").GetComponent<ChickStorage>();

        myChickParent = chickStorage.ClaimChick(out myChickIndex, allChicks[myChickNumber].name);

        if (myChickParent != null)
        {
            claimedChick = true;

            myChickObject = myChickParent.transform.GetChild(0).gameObject;
            myCamera = myChickObject.transform.GetChild(0).GetComponent<Camera>();
            fireObject = myChickObject.transform.GetChild(1).gameObject;

            myChickObject.GetComponent<ChickColliderController>().chickController = this;

            myChickObject.GetComponent<ChickAI>().enabled = false;
            photonView.RPC("StopChickAI", RpcTarget.Others, myChickParent.name);

            if (fireObject.activeInHierarchy)
            {
                onFire = true;
            }
            else
            {
                onFire = false;
            }

            rb = myChickObject.GetComponent<Rigidbody>();

            myCamera.gameObject.SetActive(true);

            myChickParent.transform.parent = this.transform;

            myChickObject.transform.GetChild(3).GetChild(0).gameObject.SetActive(false);
        }


        splashSound = GameObject.Find("SplashSound").GetComponent<AudioController>();
        catchFireSound = GameObject.Find("Catch Fire Sound").GetComponent<AudioController>();
        onFireSound = GameObject.Find("Chick Aflame Sound").GetComponent<AudioController>();
        wetWalkSound = GameObject.Find("Wet Walk").GetComponent<AudioController>();
        bawkSound = GameObject.Find("Chicken Call").GetComponent<AudioController>();

    }

    void MoveChick()
    {
        myChickObject.transform.RotateAround(myChickObject.transform.position, myChickObject.transform.up, Input.GetAxis("Mouse X") * rotationSpeed);
        if(isWet){
            rb.velocity = (myChickObject.transform.forward * Input.GetAxis("Vertical") + myChickObject.transform.right * Input.GetAxis("Horizontal"))  * wetSpeed;
        }
        else{
            rb.velocity = (myChickObject.transform.forward * Input.GetAxis("Vertical") + myChickObject.transform.right * Input.GetAxis("Horizontal")) * (onFire ? moveSpeed : moveSpeed * 0.75f);
        }
    }

    bool sentName = false;
    public void SendChickMovement() 
    {
        if (!sentName)
        {
            sentName = true;
            photonView.RPC("UpdateChick", RpcTarget.Others, myChickParent.name, myChickObject.transform.position, myChickObject.transform.rotation, PhotonNetwork.NickName);
        }
        else
        {
            photonView.RPC("UpdateChick", RpcTarget.Others, myChickParent.name, myChickObject.transform.position, myChickObject.transform.rotation, null);
        }
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
        if (farmerName != null)
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
        //this set the chick to be wet
        localChick.transform.GetChild(0).GetChild(4).gameObject.SetActive(true);

        if (localChick == myChickObject)
        {
            onFire = false;
            isWet = true;
        }
    }

    [PunRPC]
    public void LightChick(string remoteChick)
    {
        GameObject localChick = allChicks.Where(chick => chick.name == remoteChick).ToArray()[0];

        localChick.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);

        if (localChick == myChickObject)
        {
            onFire = true;
        }

        GameObject catchFireObject = GameObject.Instantiate(catchFireSound.gameObject);
        catchFireObject.transform.position = localChick.transform.position;
        catchFireObject.GetComponent<AudioController>().PlaySound();

    }


    [PunRPC]
    public void SyncWater(Vector3 waterPosition, Quaternion waterRotation)
    {
        waterParticleSystem.transform.parent.position = waterPosition;
        waterParticleSystem.transform.parent.rotation = waterRotation;

        waterParticleSystem.Play();

        splashSound.PlaySound();

        StartCoroutine(EnableWaterCollider());
    }

    IEnumerator EnableWaterCollider() 
    {
        yield return new WaitForSeconds(0.2f);

        waterCol.enabled = true;

        yield return new WaitForSeconds(0.55f);

        waterCol.enabled = false;
    }

    [PunRPC]
    public void DryUpChick(string remoteChick){
        GameObject localChick = allChicks.Where(chick => chick.name == remoteChick).ToArray()[0];
        localChick.transform.GetChild(0).GetChild(4).gameObject.SetActive(false);
        isWet = false;
    }

    [PunRPC]
    public void BawkAt(Vector3 position) 
    {
        GameObject bawkObject = GameObject.Instantiate(bawkSound.gameObject);
        bawkObject.transform.position = position;
        bawkObject.GetComponent<AudioController>().PlaySound();
    }

}
