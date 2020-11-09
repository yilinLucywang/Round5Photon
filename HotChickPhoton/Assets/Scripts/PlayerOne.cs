using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerOne : MonoBehaviour
{

    public static int activePlayerIdx = 0;

    [SerializeField]
    int index;
    void Start()
    {

    }

    public static void SetText(bool isChick){
        GameObject textBox = GameObject.Find("Text");
        if(isChick){
            //textBox.GetComponent<Text>().text = "Chick";
            Debug.Log("You are chick");
            SceneManager.LoadScene("ChickScene");
        }
        else
        {
            //textBox.GetComponent<Text>().text = "Farmer";
            Debug.Log("You are farmer");
            SceneManager.LoadScene("FarmerScene");
        }
    }

}
