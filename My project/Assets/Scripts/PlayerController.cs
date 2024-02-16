using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController cc;
    private Animator anim;
    private ProjectileController pc;

    public float speed;
    public float gravity = 9.81f;
    public float jumpSpeed = 10.0f;
    public float YVelocity;
    public float projectileSpeed;
    public LayerMask enemyCheck;
    public GameObject projectile;
    public GameObject spawnPoint;

    private Scenes scenes;

    private int playerHealth;

    public static bool isAlive;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = 10;
        isAlive = true;
        try
        {
            scenes = FindObjectOfType<Scenes>();
            cc = GetComponent<CharacterController>();
            anim = GetComponent<Animator>();

            if (speed < 0) speed = 10;
                throw new ArgumentException("Default Value has been set for speed");
        }
        catch(NullReferenceException e) {
        }
        catch(ArgumentException e) {
            Debug.Log(e.ToString());
        }
        GameManager.Instance.TestGameManagers();
    }

    // Update is called once per frame
    void Update()
    {
        float hInput = Input.GetAxis("Horizontal");
        float fInput = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(hInput, 0, fInput);

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 desiredMoveDirection = cameraForward * fInput + cameraRight * hInput;

        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);

        if (clipInfo[0].clip.name != "SpellCast" && clipInfo[0].clip.name != "PunchLeft" && clipInfo[0].clip.name != "OverHandPunch" && isAlive)
        {
            if (desiredMoveDirection.magnitude > 0)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), 1.0f);

            desiredMoveDirection *= speed * Time.deltaTime;
            desiredMoveDirection.y -= gravity;

            YVelocity = (!cc.isGrounded) ? YVelocity -= gravity * Time.deltaTime : 0;

            if (cc.isGrounded && Input.GetButtonDown("Jump")) YVelocity = jumpSpeed;

            desiredMoveDirection.y = YVelocity;

            anim.SetFloat("Speed", dir.magnitude);
            cc.Move(desiredMoveDirection);
        }
        Vector3 higherPos = new Vector3(transform.position.x, transform.position.y + 4, transform.position.z);

        Ray ray = new Ray(higherPos, transform.forward);
        RaycastHit hitInfo;

        Debug.DrawLine(higherPos, higherPos + transform.forward * 100.0f, Color.red);

        if (Physics.Raycast(ray, out hitInfo, 100.0f, enemyCheck))
        {
            Debug.Log(hitInfo);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && clipInfo[0].clip.name != "SpellCast")
        {
            anim.SetTrigger("SpellCast");
            GameObject newProjectile = Instantiate(projectile, spawnPoint.transform.position, Quaternion.identity);
            pc = newProjectile.GetComponent<ProjectileController>();
        }
        if (clipInfo[0].clip.name == "SpellCast" && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f)
        {
            pc.SetDirection(transform.forward);
            pc.projectileSpeed = projectileSpeed;
        }
        if (Input.GetKeyDown(KeyCode.RightControl) && clipInfo[0].clip.name != "Punch")
            anim.SetTrigger("Punch");
        if (Input.GetKeyDown(KeyCode.Return) && clipInfo[0].clip.name != "OverHandPunch")
            anim.SetTrigger("OverHandPunch");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && isAlive)
        {
            playerHealth -= 2;
            if (playerHealth <= 0)
            {
                isAlive = false;
                anim.SetFloat("Speed", 0);
                anim.StopPlayback();
                anim.SetTrigger("Death");
                Invoke("GameOver", 3);
            }
            else anim.SetTrigger("Hit");
        }
    }
    private void GameOver()
    {
        Debug.Log("GameOver is being triggered");
        Scenes.GameOver();
    }
}