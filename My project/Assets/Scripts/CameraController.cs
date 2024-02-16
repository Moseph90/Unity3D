using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CinemachineFreeLook camera;

    public GameObject playerObject;
    public float playerX;
    public float playerY;
    public float playerZ;

    private GameObject playerInstance;
    private Vector3 playerPos;
    void Start()
    {
        camera = GetComponent<CinemachineFreeLook>();
        playerPos = new Vector3(playerX, playerY, playerZ);

    }

    private void LateUpdate()
    {
        if (playerObject == null) return;

    }
}
