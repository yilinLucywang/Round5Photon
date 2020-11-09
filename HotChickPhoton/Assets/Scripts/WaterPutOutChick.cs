using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPutOutChick : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "ChickCollider") 
        {
            // Chick -> ChickParent -> ChickWrapper
            other.transform.parent.parent.GetComponent<ChickController>().onFire = false;
        }
    }
}
