using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerController : MonoBehaviour
{

    public bool hasWater = true;

    GameObject[] allChicks;

    GameObject myFarmerParent;
    GameObject myFarmerObject;
    Camera myCamera;
    GameObject waterCollider;
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
        if (myFarmerParent == null) 
        {
            ClaimFarmer();
        }

        if (myFarmerParent == null) 
        {
            return;
        }



        myFarmerObject.transform.RotateAround(myFarmerObject.transform.position, myFarmerObject.transform.up, Input.GetAxis("Mouse X") * rotationSpeed);

        rb.velocity = myFarmerObject.transform.forward * moveSpeed;

        if (hasWater && Input.GetAxis("Fire1") > 0) 
        {
            SplashWater();
        }





    }

    void ClaimFarmer() 
    {
        allChicks = GameObject.FindGameObjectsWithTag("Chick");

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

        yield return new WaitForSeconds(2);

        waterCollider.SetActive(true);

        yield return new WaitForSeconds(1);

        waterCollider.SetActive(false);

    }

}
