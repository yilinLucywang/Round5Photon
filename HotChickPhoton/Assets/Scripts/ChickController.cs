using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChickController : MonoBehaviour
{
    [SerializeField]
    GameObject fireObject;

    public bool onFire = true;
    public PhotonView PV;
    int pvID;

    bool claimedChick = false;
    GameObject[] otherChicks;
    GameObject myChickParent;
    public static GameObject myChickObject;
    Camera myCamera;
    public static Rigidbody rb;

    public float moveSpeed = 1;
    public float rotationSpeed = 1;


    // Start is called before the first frame update
    void Start()
    {

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

        if (myChickParent == null) 
        {
            return;
        }



        //myChickObject.transform.RotateAround(myChickObject.transform.position, myChickObject.transform.up, Input.GetAxis("Mouse X") * rotationSpeed);
        //rb.velocity = myChickObject.transform.forward * moveSpeed;
        PlayerOne.updateChicken(PlayerOne.activePlayerIdx, myChickObject.transform.position, myChickObject.transform.up, Input.GetAxis("Mouse X") * rotationSpeed);



        if (onFire)
        {
            fireObject.SetActive(true);
        }
        else
        {
            fireObject.SetActive(false);
        }


    }

    void ClaimChick() 
    {
        otherChicks = GameObject.FindGameObjectsWithTag("Chick");

        if (otherChicks.Length == 0) 
        {
            return;
        }


        ChickStorage chickStorage = GameObject.Find("ChickStorage").GetComponent<ChickStorage>();

        myChickParent = chickStorage.ClaimChick();

        if (myChickParent != null) 
        {
            claimedChick = true;

            myChickObject = myChickParent.transform.GetChild(0).gameObject;
            myCamera = myChickObject.transform.GetChild(0).GetComponent<Camera>();
            fireObject = myChickObject.transform.GetChild(1).gameObject;

            rb = myChickObject.GetComponent<Rigidbody>();

            myCamera.gameObject.SetActive(true);

            myChickParent.transform.parent = this.transform;
        }

    }

    public void moveChick(int source, Vector3 locationParam, Vector3 directionParam, float angle){
        myChickObject.transform.RotateAround(locationParam, directionParam, angle);
        rb.velocity = myChickObject.transform.forward * moveSpeed;
    }
}
