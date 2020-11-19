using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TextFaceCamera : MonoBehaviour
{
    Camera activeCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (activeCamera == null) 
        {
            GameObject[] cameraObjects = GameObject.FindGameObjectsWithTag("Camera");
            activeCamera = cameraObjects.Select(go => go.GetComponent<Camera>()).Where(cam => cam.gameObject.activeInHierarchy).ToArray()[0];
        }


        float xRot = transform.localEulerAngles.x;
        float zRot = transform.localEulerAngles.z;

        transform.LookAt(activeCamera.transform);
        transform.localEulerAngles = new Vector3(xRot, transform.localEulerAngles.y + 180, zRot);

    }
}
