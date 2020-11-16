using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionController : MonoBehaviour
{
    public Rigidbody[] enableGravityOnDestruction;
    public float timeToDestruction = 10.0f;

    bool onFire = false;
    GameObject myFire;
    float timeLeft;

    // Start is called before the first frame update
    void Start()
    {
        myFire = transform.GetChild(0).gameObject;
        timeLeft = timeToDestruction;
    }

    // Update is called once per frame
    void Update()
    {
        // control fire effects
        if (onFire && !myFire.activeInHierarchy)
        {
            myFire.SetActive(true);
        }

        if (!onFire && myFire.activeInHierarchy)
        {
            myFire.SetActive(false);
        }

        // destroy
        if (onFire)
        {
            timeLeft -= Time.deltaTime;
        }

        if (timeLeft <= 0) 
        {
            foreach (Rigidbody rb in enableGravityOnDestruction)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }

            // TODO: Replace with smaller burnt chunks?
            Destroy(this.gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        // flaming chick
        if (other.tag == "ChickCollider" && other.transform.GetChild(1).gameObject.activeInHierarchy)
        {
            onFire = true;
        }

        // gets put out
        if (other.tag == "Water") 
        {
            onFire = false;
            timeLeft = timeToDestruction;
        }
    }
}
