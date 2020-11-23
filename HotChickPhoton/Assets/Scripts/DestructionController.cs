using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionController : MonoBehaviour
{
    public Rigidbody[] enableGravityOnDestruction;
    public float timeToDestruction = 5.0f;
    public int pointValue = 500;

    bool onFire = false;
    GameObject myFire;
    float timeLeft;

    bool isLighting = false;
    public float lightingTime = 10; 
    float lightingTimeLeft;
    int lighterCount = 0;
    List<ChickController> chicksBurning;

    MeshRenderer[] mrs;

    GameObject pointText;

    // Start is called before the first frame update
    void Start()
    {
        myFire = transform.GetChild(0).gameObject;
        timeLeft = timeToDestruction;
        lightingTimeLeft = lightingTime;

        mrs = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mr in mrs)
        {
            if (mr.material.HasProperty("_EmissionColor"))
            {
                Debug.Log(mr.material.name);
                mr.material.SetFloat("_EnableEmission", 1.0f);
                mr.material.SetColor("_EmissionColor", new Color(0, 0, 0, 0));
                Debug.Log(mr.material.GetFloat("_EmissionColor"));
            }
        }

        transform.tag = "CannotSpreadFire";

        pointText = (GameObject)Resources.Load("PointTextPrefab");
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

            transform.tag = "CanSpreadFire";
        }
        else
        {
            transform.tag = "CannotSpreadFire";
        }

        if (timeLeft <= 0) 
        {
            foreach (Rigidbody rb in enableGravityOnDestruction)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }

            Destroy(this.gameObject);
        }

        if (isLighting)
        {
            if (lightingTimeLeft > 0) 
            { 
                lightingTimeLeft -= Time.deltaTime * lighterCount; 
            }
            else
            {
                foreach (ChickController chickController in chicksBurning) 
                {
                    chickController.updateCurrentScore(pointValue);
                }

                onFire = true;
                isLighting = false;
            }

        }
        else 
        {
            if (lightingTimeLeft + Time.deltaTime < lightingTime)
            {
                lightingTimeLeft += Time.deltaTime;
            }
            else 
            {
                lightingTimeLeft = lightingTime;
            }
        }

        foreach (MeshRenderer mr in mrs)
        {
            if (mr.material.HasProperty("_EmissionColor"))
            {
                mr.material.SetColor("_EmissionColor", new Color((1 - (lightingTimeLeft / lightingTime)) / 3, 0, 0, 0));
                if (onFire)
                {
                    mr.material.SetColor("_EmissionColor", new Color(2.0f / 3, 0, 0, 0));
                }
                Debug.Log(mr.material.GetFloat("_EnableEmission") + " " + mr.material.GetColor("_EmissionColor"));
            }
        }

        if (chicksBurning.Count <= 0) 
        {
            isLighting = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // lighting chick
        if (other.tag == "ChickCollider" && other.transform.GetChild(1).gameObject.activeInHierarchy)
        {
            isLighting = true;
            ChickColliderController otherCCC = other.transform.GetComponent<ChickColliderController>();
            chicksBurning.Add(otherCCC.chickController);
            //lighterCount += 1;
        }

        // gets put out
        if (other.tag == "Water") 
        {
            onFire = false;
            timeLeft = timeToDestruction;
            lightingTimeLeft = lightingTime;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "ChickCollider" && other.transform.GetChild(1).gameObject.activeInHierarchy)
        {

            ChickColliderController otherCCC = other.transform.GetComponent<ChickColliderController>();

            ChickController chickController = chicksBurning.Find(cc => cc == otherCCC.chickController);

            if (chickController != null)
            {

                chicksBurning.Remove(chickController);


                if (chicksBurning.Count == 0)
                {
                    isLighting = false;
                    lightingTimeLeft = lightingTime;
                }

            }
        }
    }

}
