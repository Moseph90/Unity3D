using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject playerPrefab;
    CharacterController cc;

    public float speed;
    public float gravity = 9.81f;
    public float jumpSpeed = 10.0f;
    public float YVelocity;
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            cc = GetComponent<CharacterController>();

            if (speed < 0) speed = 10;
                throw new ArgumentException("Default Value has been set for speed");
        }
        catch(NullReferenceException e) {
        }
        catch(ArgumentException e) {
            Debug.Log(e.ToString());
        }


        //Camera mainCamera = Camera.main;

        GameObject playerInstance = Instantiate(playerPrefab);
        playerInstance.transform.position = new Vector3(266.1221f, 100.2352f, 62.3831f);
        GameManager.Instance.TestGameManagers();

        //if (mainCamera != null) 
        //{
        //    mainCamera.transform.parent = playerInstance.transform;
        //    Debug.Log("Camera Has Been Found");
        //    mainCamera.transform.localPosition = new Vector3(0, 1.5f, -3);
        //}

        if (playerInstance) Debug.Log("Player Has Been Created");
    }

    // Update is called once per frame
    void Update()
    {
        float hInput = Input.GetAxisRaw("Horizontal");
        float fInput = Input.GetAxisRaw("Vertical");

        Vector3 moveInput = new Vector3(hInput, 0, fInput).normalized;
        moveInput *= speed * Time.deltaTime;
        //moveInput.y -= gravity;

        YVelocity = (!cc.isGrounded) ? YVelocity -= gravity * Time.deltaTime : YVelocity = 0;

        if (cc.isGrounded && Input.GetButtonDown("Jump")) YVelocity = jumpSpeed;

        moveInput.y = YVelocity;

        cc.Move(moveInput);
       // rb.velocity = new Vector3(moveInput.x * speed, rb.velocity.y, moveInput.z * speed);
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        
    }
}
