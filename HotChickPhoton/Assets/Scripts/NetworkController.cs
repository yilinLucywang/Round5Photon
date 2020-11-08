//stunned; items; status

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*documentation: https://doc.photonengine.com/en-us/pun/current/getting-started/pun-intro 
  scriptingAPI: https://doc-api.photonengine.com/en/pun/v2/index.html*/

public class NetworkController : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster(){
        base.OnConnectedToMaster();
    	//Debug.Log("We are now connected to the " + PhotonNetwork.CloudRegion + " server!");
    }
}


















