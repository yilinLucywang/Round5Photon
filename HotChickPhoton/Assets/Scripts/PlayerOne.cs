using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            textBox.GetComponent<Text>().text = "Chick";
        }
        else
        {
            textBox.GetComponent<Text>().text = "Farmer";
        }
    }

}
