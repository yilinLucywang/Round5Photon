using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class PlayerOne : MonoBehaviour
{

    public static PhotonView PV;
    int pvID;
    public static int activePlayerIdx = 0;
    public static bool isFarmer = false; 
    [SerializeField]
    public ChickController[] chicks;
    [SerializeField]
    public FarmerController[] farmers;

    [SerializeField]
    int index;
    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    public static void SetText(bool isChick){
        if(isChick){
            SceneManager.LoadScene("ChickScene");
        }
        else
        {
            SceneManager.LoadScene("FarmerScene");
        }
    }

    public static void updateChicken(int source, Vector3 locationParam, Vector3 directionParam, float angle){
        PV.RPC("RPC_UpdateChick", RpcTarget.All, source, locationParam, directionParam, angle);
    }

    public static void updateFarmer(int source, Vector3 locationParam, Vector3 directionParam, float angle){
        PV.RPC("RPC_UpdateFarmer", RpcTarget.All, source, locationParam, directionParam, angle);
    }

    [PunRPC]
    private void RPC_UpdateChick(int source, Vector3 locationParam, Vector3 directionParam, float angle){
        chicks[source].moveChick(source, locationParam, directionParam, angle);
    }

    [PunRPC]
    private void RPC_UpdateFarmer(int source, Vector3 locationParam, Vector3 directionParam, float angle){
        farmers[source].moveFarmer(source, locationParam, directionParam, angle);
    }

}
