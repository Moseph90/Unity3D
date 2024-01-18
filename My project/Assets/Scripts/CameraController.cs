using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Attach the camera to this in the inspector
    Camera mainCamera;

    //Change these values in the inspector to represent the proper bounds of the level
    //Values are temporary for the given test scene
    public float minXClamp;
    public float maxXClamp;
    public float minZClamp;
    public float maxZClamp;

    //This is to get the correct height of the camera
    public float cameraHeight;

    //This is to get the correct angle of the camera, I initially set this to be 45
    public float cameraAngle;

    //Attach player object in inspector
    GameObject playerObject;

    void Start()
    {
        mainCamera = GetComponent<Camera>();

        if (mainCamera) mainCamera.transform.parent = playerObject.transform;
            
        else if (!mainCamera) Debug.Log("Camera Has Not Been Found");
    }

    private void LateUpdate()
    {
        if (playerObject == null) return;
        Vector3 cameraPos;

        cameraPos = transform.position;
        cameraPos.x = Mathf.Clamp(playerObject.transform.position.x, minXClamp, maxXClamp);
        cameraPos.y = cameraHeight;
        cameraPos.z = Mathf.Clamp(playerObject.transform.position.z, minZClamp, maxZClamp);
        transform.position = cameraPos;

        Quaternion newRotation = Quaternion.Euler(cameraAngle, 0.0f, 0.0f);
        transform.rotation = newRotation;
    }
}
