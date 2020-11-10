﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ChickStorage : MonoBehaviour
{
    class ChickCompare : IComparer
    {
        int IComparer.Compare(object x, object y)
        {
            return string.Compare(((GameObject)x).name, ((GameObject)y).name);
        }
    }


    public GameObject[] allChicks;
    bool[] chickIsClaimed;

    // Start is called before the first frame update
    void Start()
    {
        allChicks = GameObject.FindGameObjectsWithTag("Chick");
        Array.Sort(allChicks, new ChickCompare());

        chickIsClaimed = new bool[allChicks.Length];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject ClaimChick(out int myChickIndex) 
    {
        for (int chickIndex = 0; chickIndex < allChicks.Length; chickIndex++) 
        {
            if (!chickIsClaimed[chickIndex]) 
            {
                chickIsClaimed[chickIndex] = true;
                myChickIndex = chickIndex;
                return allChicks[chickIndex];
            }
        }

        myChickIndex = -1;
        return null;
    }

}