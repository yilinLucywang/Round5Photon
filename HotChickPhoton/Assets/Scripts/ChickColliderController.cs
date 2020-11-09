using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickColliderController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ChickCollider") 
        {
            if (other.transform.parent.parent.GetComponent<ChickController>().onFire) 
            {
                transform.parent.parent.GetComponent<ChickController>().onFire = true;
            }
        }
    }
}
