using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FarmerController : MonoBehaviour
{
    class ChickCompare : IComparer
    {
        int IComparer.Compare(object x, object y)
        {
            return string.Compare(((GameObject)x).name, ((GameObject)y).name);
        }
    }

    // TODO: Temporary until we get a water model.
    GameObject bucketWater;
    Vector3[] bucketWaterLocalPositions;

    int waterLeft;
    int maxWater = 5;
    int splashingWater = 0;
    int maxSplashingWater = 60;

    GameObject[] allChicks;
    GameObject[] allChickObjects;

    GameObject myFarmerParent;
    public static GameObject myFarmerObject;
    Camera myCamera;
    WaterPutOutChick waterController;
    public static Rigidbody rb;
    public PhotonView photonView;
    int pvID;
    public GameObject leaderBoard;


    public float moveSpeed = 6.5f;
    public float rotationSpeed = 1.3f;
    public bool isChick = false;

    int frameCounter = 0;

    AudioController splashSound;
    AudioController catchFireSound;
    AudioController onFireSound;
    AudioController wetWalkSound;
    AudioController bawkSound;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GameObject.Find("QuickStartRoomController").GetComponent<PhotonView>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        waterLeft = maxWater;

        bucketWaterLocalPositions = new Vector3[maxWater + 1];

        bucketWaterLocalPositions[5] = new Vector3(0, 0.02f, 0);
        bucketWaterLocalPositions[4] = new Vector3(0, 0.017f, 0);
        bucketWaterLocalPositions[3] = new Vector3(0, 0.014f, 0);
        bucketWaterLocalPositions[2] = new Vector3(0, 0.011f, 0);
        bucketWaterLocalPositions[1] = new Vector3(0, 0.008f, 0);
        bucketWaterLocalPositions[0] = new Vector3(0, -100, 0);

        leaderBoard = GameObject.Find("LeaderBoard");
        leaderBoard.SetActive(false);
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

        // TODO: Fix to allow splashing while other water is still going.
        if (waterLeft > 0 && Input.GetButtonDown("Fire1") && splashingWater == 0)
        {
            bool splashedSuccessfully = waterController.SplashWater();
            if (splashedSuccessfully)
            {
                waterLeft--;
                splashingWater = maxSplashingWater;
                bucketWater.transform.localPosition = bucketWaterLocalPositions[waterLeft];
                splashSound.PlaySound();
            }
        }

        if (splashingWater > 0)
        {
            splashingWater--;
        }


        frameCounter++;
        if (frameCounter == 6)
        {
            SendFarmerMovement();
            frameCounter = 0;
        }


    }

    public void FillBucket()
    {
        waterLeft = maxWater;
        bucketWater.transform.localPosition = bucketWaterLocalPositions[waterLeft];
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

        foreach (GameObject chickObject in allChickObjects)
        {
            ChickAI chickAI = chickObject.GetComponent<ChickAI>();
            chickAI.photonView = photonView;
            chickAI.enabled = true;
        }


        myFarmerParent = GameObject.Find("FarmerParent");

        if (myFarmerParent != null)
        {

            myFarmerObject = myFarmerParent.transform.GetChild(0).gameObject;
            myCamera = myFarmerObject.transform.GetChild(0).GetComponent<Camera>();
            waterController = myFarmerObject.transform.GetChild(1).GetComponent<WaterPutOutChick>();

            rb = myFarmerObject.GetComponent<Rigidbody>();

            myCamera.gameObject.SetActive(true);

        }


        GameObject.Find("FarmerName").GetComponent<Text>().text = PhotonNetwork.NickName;
        bucketWater = GameObject.Find("BucketWater");


        splashSound = GameObject.Find("SplashSound").GetComponent<AudioController>();
        catchFireSound = GameObject.Find("Catch Fire Sound").GetComponent<AudioController>();
        onFireSound = GameObject.Find("Chick Aflame Sound").GetComponent<AudioController>();
        wetWalkSound = GameObject.Find("Wet Walk").GetComponent<AudioController>();
        bawkSound = GameObject.Find("Chicken Call").GetComponent<AudioController>();

    }

    void MoveFarmer()
    {
        // This was annoying to play as the farmer, so I removed it. Still works, though.
        /*if (splashingWater == 0)
        {
            myFarmerObject.transform.RotateAround(myFarmerObject.transform.position, myFarmerObject.transform.up, Input.GetAxis("Mouse X") * rotationSpeed);
            rb.velocity = myFarmerObject.transform.forward * moveSpeed;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }*/

        myFarmerObject.transform.RotateAround(myFarmerObject.transform.position, myFarmerObject.transform.up, Input.GetAxis("Mouse X") * rotationSpeed);
        rb.velocity = (myFarmerObject.transform.forward * Input.GetAxis("Vertical") + myFarmerObject.transform.right * Input.GetAxis("Horizontal")) * moveSpeed;
    }

    bool sentName = false;
    void SendFarmerMovement()
    {
        if (!sentName)
        {
            sentName = true;
            photonView.RPC("UpdateFarmer", RpcTarget.Others, myFarmerObject.transform.position, myFarmerObject.transform.rotation, PhotonNetwork.NickName);
        }
        else
        {
            photonView.RPC("UpdateFarmer", RpcTarget.Others, myFarmerObject.transform.position, myFarmerObject.transform.rotation, null);
        }
    }

    [PunRPC]
    public void UpdateChick(string chickName, Vector3 chickPosition, Quaternion chickRotation, string chickNameToUpdate)
    {
        Debug.Log("Updating chick " + chickName);
        int chickIndex = allChicks.Select((chick, index) => chick.name == chickName ? index : -1).Where(index => index != -1).ToArray()[0];
        if (chickNameToUpdate != null){
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

        GameObject catchFireObject = GameObject.Instantiate(catchFireSound.gameObject);
        catchFireObject.transform.position = localChick.transform.position;
        catchFireObject.GetComponent<AudioController>().PlaySound();

    }

    [PunRPC]
    public void StopChickAI(string remoteChick)
    {
        GameObject localChick = allChicks.Where(chick => chick.name == remoteChick).ToArray()[0];

        localChick.transform.GetChild(0).GetComponent<ChickAI>().enabled = false;
    }


    [PunRPC]
    public void BawkAt(Vector3 position)
    {
        GameObject bawkObject = GameObject.Instantiate(bawkSound.gameObject);
        bawkObject.transform.position = position;
        bawkObject.GetComponent<AudioController>().PlaySound();
    }

    [PunRPC]
    public void EndGame(bool propaneIsBurnt){
        if(propaneIsBurnt){
            if(isChick){
                SceneManager.LoadScene("chickWin");
            }
            else{
                SceneManager.LoadScene("FarmerLose");
            }
        }
        else{
            if(isChick){
                SceneManager.LoadScene("chickLose");
            }
            else{
                SceneManager.LoadScene("FarmerWin");
            }
        }
    }
}
