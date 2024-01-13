using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject playerPrefab;

    public float speed;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        Camera mainCamera = Camera.main;

        GameObject playerInstance = Instantiate(playerPrefab);
        playerInstance.transform.position = new Vector3(266.1221f, 100.2352f, 62.3831f);
        rb = playerInstance.GetComponent<Rigidbody>();
        GameManager.Instance.TestGameManagers();

        if (mainCamera != null) 
        {
            mainCamera.transform.parent = playerInstance.transform;
            Debug.Log("Camera Has Been Found");
            mainCamera.transform.localPosition = new Vector3(0, 1.5f, -3);
        }

        if (playerInstance) Debug.Log("Player Has Been Created");
    }

    // Update is called once per frame
    void Update()
    {
        float hInput = Input.GetAxisRaw("Horizontal");
        float fInput = Input.GetAxisRaw("Vertical");

        Vector3 moveInput = new Vector3(hInput * speed, rb.velocity.y, fInput * speed);
        rb.velocity = moveInput;
    }
}
