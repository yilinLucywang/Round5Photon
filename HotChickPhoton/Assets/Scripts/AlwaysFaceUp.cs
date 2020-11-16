using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysFaceUp : MonoBehaviour
{
    void Update()
    {
        transform.forward = Vector3.up;
    }
}
