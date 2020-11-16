using Photon.Pun; 
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickStartLobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject quickStartButton; 
    [SerializeField]
    private GameObject quickCancelButton; 
    [SerializeField]
    private int RoomSize; 
    public GameObject userNameBox; 
    private bool connectedToMaster = false;

    private void Awake(){
        userNameBox.SetActive(true);
        //quickStartButton.GetComponent<Image>().color = new Color(0,0,0,0);
        //quickCancelButton.GetComponent<Image>().color = new Color(0,0,0,0);
        quickCancelButton.SetActive(false);
        quickStartButton.SetActive(false);
    }
    public override void OnConnectedToMaster(){
    	PhotonNetwork.AutomaticallySyncScene = false; 
    	//quickStartButton.SetActive(true);
        //quickCancelButton.SetActive(false);
    	connectedToMaster = true;
    }

    public void QuickStart(){
    	if(connectedToMaster){
    		quickStartButton.SetActive(false);
    		quickCancelButton.SetActive(true);
    		PhotonNetwork.JoinRandomRoom();
    		Debug.Log("Quick Start");
    	}
    }

    public override void OnJoinRandomFailed(short returnCode, string message){
    	Debug.Log("Failed to join a room");
    	CreateRoom();
    }

    void CreateRoom(){
    	Debug.Log("Creating room now!");
    	int randomRoomNumber = Random.Range(0,10000);
    	RoomOptions roomOps = new RoomOptions(){IsVisible = true, IsOpen = true, MaxPlayers = (byte)MultiplayerSettings.multiplayerSettings.maxPlayers};
    	PhotonNetwork.CreateRoom("Room" + randomRoomNumber, roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message){
    	Debug.Log("Failed to create room ... trying again");
    	CreateRoom();
    }

    public void QuickCancel(){
    	quickCancelButton.SetActive(false);
    	quickStartButton.SetActive(true);
    	PhotonNetwork.LeaveRoom();
    }
}
