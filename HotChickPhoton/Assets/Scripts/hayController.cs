using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class hayController : MonoBehaviour
{
	public GameObject[] allHay;
	//TODO; may need to change the size of this later
	bool[] isOnFire = new bool[]{false, false}; 
	GameObject[] allChicks;
	GameObject[] allChickObjects;
	public static GameObject myChickObject;
	public float distanceToLightAHay = 0.15f;
	public float timeToLightHay = 2.0f; 
	public PhotonView photonView;
	    // Start is called before the first frame update
    void Start()
    {
        photonView = GameObject.Find("QuickStartRoomController").GetComponent<PhotonView>();
        foreach(GameObject hay in allHay){
        	Debug.Log(hay.transform.GetChild(0).gameObject);
        	hay.transform.GetChild(0).gameObject.SetActive(false);
    	}
    }

    void Update()
    {
    	int hayIndex = 0;
    	foreach(GameObject hay in allHay){
    		lightHay(hayIndex, hay.transform.position);
    		hayIndex ++;
    	}
    	for(int i = 0; i < allHay.Length; i++){
    		if(isOnFire[i]){
    			allHay[i].transform.GetChild(0).gameObject.SetActive(true);
    		}
    	}

    }

    void lightHay(int hayIndex, Vector3 currentHayLocation){
    	//chicken position
    	allChicks = GameObject.FindGameObjectsWithTag("Chick");
    	allChickObjects = allChicks.Select(chickParent => chickParent.transform.GetChild(0).gameObject).ToArray();
    	int lightedChickCount = 0;
    	foreach(GameObject chick in allChickObjects){
    		Vector3 position = chick.transform.position;
    		float distance =  Vector3.Distance(position, currentHayLocation);
    		if(distance <= distanceToLightAHay){
    			lightedChickCount ++;
    		}
    	}
    	if(lightedChickCount > 0){
    		StartCoroutine(lightHay(hayIndex, lightedChickCount));
    		isOnFire[hayIndex] = true;
    		string indexToPass = hayIndex.ToString();
    		//photonView.RPC("RPC_lightHay", RpcTarget.All, indexToPass);
    	}
    	else{
    		isOnFire[hayIndex] = false;
    	}
    }

    IEnumerator lightHay(int hayIndex, int lightedChickCount){
    	float timeToLightHayMulti = timeToLightHay/lightedChickCount;
    	yield return new WaitForSeconds(timeToLightHayMulti);
    }



    // [PunRPC]
    // public void RPC_lightHay(string hayIndexString)
    // {
    // 	int hayIndex = int.Parse(hayIndexString);
    //     isOnFire[hayIndex] = true;
    // }



















}