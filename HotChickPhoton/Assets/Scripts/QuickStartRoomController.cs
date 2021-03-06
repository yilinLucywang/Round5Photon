﻿using Photon.Pun; 
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;


public class QuickStartRoomController : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
	public GameObject openingImage; 
	public GameObject buttons;
	//Room info
	public static QuickStartRoomController room; 
	private int multiplayerSceneIndex;
	public PhotonView PV; 

	public bool isGameLoaded; 
	public int currentScene;

	//player info
	private Player[]photonPlayers; 
	private string nickName;
	public int playersInRoom; 
	public int myNumberInRoom; 
	public GameObject userNameBox;
	public GameObject enterButton;
	public GameObject startButton;
	public InputField InputFieldUsername;

	public int playersInGame;

	//Delayed start
	private bool readyToCount;
	private bool readyToStart; 
	public float startingTime;
	private float lessThanMaxPlayers; 
	private float atMaxPlayer; 
	private float timeToStart;

	//video information
    public VideoPlayer startAnim;
    public float videoLength;
    public GameObject quickStartButton;
    public GameObject quickCancelButton;
    public GameObject userName;

	public bool skipVideo = true;


	private bool hasTheImposter = false;

	[SerializeField]
	Text lobbyText;

	private void playAnimVideo()
    {
    	skipVideo = true;
		if (skipVideo)
		{
			if (PhotonNetwork.IsMasterClient) PhotonNetwork.LoadLevel(MultiplayerSettings.multiplayerSettings.multiplayerScene);
		}
		else
		{
			startAnim.Play();
			startAnim.loopPointReached += EndReached;
		}
    }

 	private void EndReached(UnityEngine.Video.VideoPlayer startAnim)
    {
    	if( PhotonNetwork.IsMasterClient ) PhotonNetwork.LoadLevel(MultiplayerSettings.multiplayerSettings.multiplayerScene);
    }

	public override void OnEnable(){
		base.OnEnable();
		PhotonNetwork.AddCallbackTarget(this);
		SceneManager.sceneLoaded += OnSceneFinishedLoading; 
	}

	public override void OnDisable(){
		base.OnDisable();
		PhotonNetwork.RemoveCallbackTarget(this);
		SceneManager.sceneLoaded -= OnSceneFinishedLoading;
	}

	public override void OnJoinedRoom(){
		userNameBox.SetActive(true);
		base.OnJoinedRoom();
		Debug.Log("Joined Room");
		photonPlayers = PhotonNetwork.PlayerList; 
		playersInRoom = photonPlayers.Length; 
		myNumberInRoom = playersInRoom;
		//TODO: change back later
		//PlayerOne.activePlayerIdx = myNumberInRoom-1;
		setUserName();
		//PhotonNetwork.NickName = myNumberInRoom.ToString();
		GameObject.Find("PhotonText").GetComponent<Text>().text = playersInRoom.ToString() + " / " + MultiplayerSettings.multiplayerSettings.maxPlayers.ToString();
		if(playersInRoom > 1){
			readyToCount = true;
		}
		if(playersInRoom == MultiplayerSettings.multiplayerSettings.maxPlayers){
			readyToStart = true; 
			if(!PhotonNetwork.IsMasterClient){
				return;
			}
			else{
				PhotonNetwork.CurrentRoom.IsOpen = false; 
			}
		}
	}

	public void storeNickName(){
		nickName = InputFieldUsername.text; 
		startButton.SetActive(true);
		enterButton.SetActive(false);
	}
	public void setUserName(){
		//Debug.Log("#####"+ InputFieldUsername.text + "#####");
		PhotonNetwork.NickName = nickName; 
	}

	public void claimChickButton(){
		Debug.Log("Claimbutton clicked");
		if(myNumberInRoom != 2){
			string buttonName = EventSystem.current.currentSelectedGameObject.name;
			PV.RPC("ClaimChick", RpcTarget.All, buttonName, nickName);
		}
	}

	private void Awake(){
		if(QuickStartRoomController.room == null){
			QuickStartRoomController.room = this;
		}
		else{
			if(QuickStartRoomController.room != this){
				Destroy(QuickStartRoomController.room.gameObject);
				QuickStartRoomController.room = this;
			}
		}
		DontDestroyOnLoad(this.gameObject);
		openingImage.SetActive(false);
		buttons.SetActive(false);
	}

	void Start(){
		//PV = GetComponent<PhotonView>();
		readyToCount = false; 
		readyToStart = false; 
		lessThanMaxPlayers = startingTime; 
		atMaxPlayer = 3; 
		timeToStart = startingTime; 
	}

	public override void OnPlayerEnteredRoom(Player newPlayer){
		base.OnPlayerEnteredRoom(newPlayer);
		photonPlayers = PhotonNetwork.PlayerList;
		playersInRoom ++;
		if( PhotonNetwork.IsMasterClient )
        {
			PV.RPC("RPC_UpdatePlayerCount", RpcTarget.All, playersInRoom);
        }
		print("Player entered room, current players: " + playersInRoom);
		if(playersInRoom >= 1){
			//readyToCount = true;
		}
		if(playersInRoom == MultiplayerSettings.multiplayerSettings.maxPlayers){
			readyToStart = true; 
			if(!PhotonNetwork.IsMasterClient){
				return;
			}
			else{
				PhotonNetwork.CurrentRoom.IsOpen = false; 
			}
		}
	}

	void Update()
	{

		if(!isGameLoaded)
		{
			if(readyToStart)
			{
				StartGame();
			}
		}


	}

	public void EnterUserName(){
		setUserName();
		enterButton.SetActive(false);
	}

	void StartGame(){
		isGameLoaded = true;
		//setUserName();
		Debug.Log(PhotonNetwork.IsMasterClient);
		PhotonNetwork.CurrentRoom.IsOpen = false;


		if (myNumberInRoom == 1)
		{
			// I am the host.

			//SceneManager.LoadScene("FarmerScene");
			gameObject.AddComponent<HostController>();
		}
		else if (myNumberInRoom == 2)
		{
			// I am the farmer.

			//SceneManager.LoadScene("FarmerScene");
			gameObject.AddComponent<FarmerController>();
		}
		else
		{
			// I am a chick.
			//SceneManager.LoadScene("ChickScene");
			ChickController chickController = gameObject.AddComponent<ChickController>();
			chickController.myChickNumber = myNumberInRoom - 3;
		}

		//This load the starting images for everyone
		//PV.RPC("loadStartingImages", RpcTarget.All);


		SceneManager.LoadScene("BarnScene");

	}

	//No connection here
	void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode){
		Debug.Log("I am here!");
		currentScene = scene.buildIndex; 
		if(currentScene == MultiplayerSettings.multiplayerSettings.multiplayerScene){
			isGameLoaded = true;
			PhotonNetwork.AutomaticallySyncScene = false; 
			PV.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
			//Debug.Log("I am here!");
			//PV.RPC("RPC_CreatePlayer", RpcTarget.All);
		}
	}

	void loadOpeningImages(){
		Debug.Log("loading images");
		List<Texture> textures = new List<Texture>();
		textures.Add(Resources.Load<Texture>("OPENING-01"));
		textures.Add(Resources.Load<Texture>("OPENING-02"));
		textures.Add(Resources.Load<Texture>("OPENING-03"));
		textures.Add(Resources.Load<Texture>("OPENING-04"));
		textures.Add(Resources.Load<Texture>("OPENING-05"));
		textures.Add(Resources.Load<Texture>("OPENING-06"));
		//textures.Add(Resources.Load<Texture>("OPENING-07"));
		StartCoroutine(LoadImagesSlideBySlide(textures));
	}

	IEnumerator LoadImagesSlideBySlide(List<Texture> textures){
		openingImage.SetActive(true);
		buttons.SetActive(false);
		Debug.Log("loading images slide by slide");
		Debug.Log(textures.Count);
		for(int i = 0; i < textures.Count; i++){
			openingImage.GetComponent<RawImage>().texture = textures[i];
			yield return new WaitForSeconds(3.0f);
		}
		openingImage.GetComponent<RawImage>().texture = textures[0];
		buttons.SetActive(true);

		//Only the host and the chick are able to interact with the canvas
		Cursor.visible = true;
		Screen.lockCursor = false;

		//openingImage.SetActive(false);
		//SceneManager.LoadScene("BarnScene");
	}

	[PunRPC]
	private void RPC_UpdatePlayerCount( int newCount )
    {
		playersInRoom = newCount;
		lobbyText.text = newCount + " / " + MultiplayerSettings.multiplayerSettings.maxPlayers;
    }

	[PunRPC]
	private void RPC_LoadedGameScene(){
		playersInGame ++;
		if(playersInGame == PhotonNetwork.PlayerList.Length){
			//TODO: Change here later
			int imposterNumber = 0; 
			//int imposterNumber = Random.Range(0, 4);
			print("Sending RPC with imposter number: " + imposterNumber);
			PV.RPC("RPC_CreatePlayer", RpcTarget.All, imposterNumber);
		}
	}
	[PunRPC]
	private void RPC_ClearScreen(){
		lobbyText.text = "";
		quickStartButton.SetActive(false);
		quickCancelButton.SetActive(false);
		userName.SetActive(false);
		playAnimVideo();
	}

	[PunRPC]
	private void RPC_CreatePlayer( int imposter ){
		//TODO: Change back here later
		/*if( imposter == PlayerOne.activePlayerIdx )
        {
			Debug.Log("You are the imposter!");
			//GameState.imposter = true;
			//PlayerOne.isFarmer = true;
			//PlayerOne.SetText(false);
        }
		else
        {
			Debug.Log("You are good people");
			//GameState.imposter = false;
			//PlayerOne.isFarmer = false;
			//PlayerOne.SetText(true);
        }*/
	}

	public void SendStartGame() 
	{
		photonView.RPC("CanStartGame", RpcTarget.All);
	}

	[PunRPC]
	private void CanStartGame() 
	{
		readyToStart = true;
	}

	[PunRPC]
	private void ClaimChick(string buttonName, string nickName){
		GameObject.Find(buttonName).transform.GetChild(0).GetComponent<Text>().text = nickName;
	}

	[PunRPC]
	private void loadStartingImages(){
		loadOpeningImages();
	}

	[PunRPC]
	private void loadBarnScene(){
		SceneManager.LoadScene("BarnScene");
	}

}
