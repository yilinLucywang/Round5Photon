using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowChick : MonoBehaviour
{
    Camera camera;

    public GameObject chick;
    public Vector3 relativePositionOffset;
    public Vector3 relativeRotationOffset;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = chick.transform.localPosition + relativePositionOffset;
        transform.rotation = chick.transform.rotation * Quaternion.Euler(relativeRotationOffset);
    }
}
