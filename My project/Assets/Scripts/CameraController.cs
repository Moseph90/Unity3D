using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float mouseSensitivity = 100.0f;
    [SerializeField]
    private Vector3 offset;

    private float xRotation = 0.0f;
    private float yRotation = 0.0f;

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Adjust the x rotation based on the mouse Y input
        xRotation -= mouseX;

        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, -90.0f, 90.0f);

        // Rotate the camera around the x-axis
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0.0f);

        // Rotate the camera around the player's position
        transform.RotateAround(player.transform.position, Vector3.up, mouseX);
        transform.RotateAround(player.transform.position, Vector3.right, mouseY);

        // Apply the vertical rotation
        transform.localRotation = Quaternion.Euler(xRotation, transform.eulerAngles.x, 0.0f);
        transform.localRotation = Quaternion.Euler(yRotation, transform.eulerAngles.y, 0.0f);

        // Maintain the offset distance from the player
        Vector3 desiredPosition = player.transform.position - transform.forward * offset.z + transform.up * offset.y;
        transform.position = desiredPosition;
    }
}
