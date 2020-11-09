using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickController : MonoBehaviour
{
    [SerializeField]
    GameObject fireObject;

    public bool onFire = true;

    bool claimedChick = false;
    GameObject[] otherChicks;
    GameObject myChickParent;
    GameObject myChickObject;
    Camera myCamera;
    Rigidbody rb;

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



        myChickObject.transform.RotateAround(myChickObject.transform.position, myChickObject.transform.up, Input.GetAxis("Mouse X") * rotationSpeed);

        rb.velocity = myChickObject.transform.forward * moveSpeed;




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

}
