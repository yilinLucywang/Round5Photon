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

    bool isLighting = false;
    float flamingTime = 10; 
    float flamingTimeLeft;
    int lighterCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        myFire = transform.GetChild(0).gameObject;
        timeLeft = timeToDestruction;
        flamingTimeLeft = flamingTime;
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

        if(isLighting)
        {
            flamingTimeLeft -= Time.deltaTime * lighterCount;
            if(flamingTimeLeft <= 0)
            {
                onFire = true;
                isLighting = false;
                flamingTimeLeft = flamingTime;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        // flaming chick
        if (other.tag == "ChickCollider" && other.transform.GetChild(1).gameObject.activeInHierarchy)
        {
            isLighting = true;
            lighterCount += 1;
        }

        // gets put out
        if (other.tag == "Water") 
        {
            onFire = false;
            timeLeft = timeToDestruction;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "ChickCollider" && other.transform.GetChild(1).gameObject.activeInHierarchy)
        {
            lighterCount = lighterCount - 1; 
            if(lighterCount <= 0)
            {
                isLighting = false;
                flamingTimeLeft = flamingTime;
            }
        }
    }
}
