using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickStorage : MonoBehaviour
{
    public GameObject[] allChicks;
    bool[] chickIsClaimed;

    // Start is called before the first frame update
    void Start()
    {
        allChicks = GameObject.FindGameObjectsWithTag("Chick");
        chickIsClaimed = new bool[allChicks.Length];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject ClaimChick() 
    {
        for (int chickIndex = 0; chickIndex < allChicks.Length; chickIndex++) 
        {
            if (!chickIsClaimed[chickIndex]) 
            {
                chickIsClaimed[chickIndex] = true;
                return allChicks[chickIndex];
            }
        }

        return null;
    }

}
