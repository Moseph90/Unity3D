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
    public CharacterController cc;
    public Animator anim;
    private ProjectileController pc;
    private CameraController cam;

    public float speed;
    public float gravity = 9.81f;
    public float jumpSpeed = 3f;
    public float YVelocity;
    public float projectileSpeed;
    public LayerMask enemyCheck;
    public GameObject projectile;
    public GameObject spawnPoint;

    private Scenes scenes;

    public static int playerHealth;
    public static int playerMana;

    public HealthBar healthBar;
    public ManaBar manaBar;

    private PlayerInstance pi;

    public static bool isAlive;
    public static bool isSwimming;
    public static bool load;

    public static bool Load
    {
        set { load = value; }
    }

    // Start is called before the first frame update
    private void Awake()
    {
        playerHealth = 100;
        playerMana = 100;
        healthBar.SetMaxHealth(playerHealth);
        manaBar.SetMaxMana(playerMana);
        isAlive = true;
        try
        {
            scenes = FindObjectOfType<Scenes>();
            cam = FindObjectOfType<CameraController>();
            cc = GetComponent<CharacterController>();
            anim = GetComponent<Animator>();
            pi = GetComponent<PlayerInstance>();

            if (speed < 0) speed = 10;
                throw new ArgumentException("Default Value has been set for speed");
        }
        catch(NullReferenceException e) {
        }
        catch(ArgumentException e) {
            Debug.Log(e.ToString());
        }
        GameManager.Instance.TestGameManagers();
        if (load)
        {
            load = false;
            LoadPlayer();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth < 0) playerHealth = 0;
        else if (playerHealth > 100) playerHealth = 100;
        if (playerMana < 0) playerMana = 0;
        else if (playerMana > 100) playerMana = 100;
        healthBar.SetHealth(playerHealth);
        manaBar.SetMana(playerMana);

        if (playerHealth <= 0 && isAlive)
        {
            isAlive = false;
            Death();
        }
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
            if (!isSwimming) desiredMoveDirection.y -= gravity;

            YVelocity = (!cc.isGrounded) ? YVelocity -= gravity * Time.deltaTime : 0;

            if (cc.isGrounded && Input.GetButtonDown("Jump")) YVelocity = jumpSpeed;

            desiredMoveDirection.y = YVelocity;
            if (!isSwimming)
            {
                speed = 15;
                anim.SetFloat("Speed", dir.magnitude);
            }
            else if (isSwimming)
            {
                anim.SetFloat("Speed", 0.3f);
                speed = 5f;
                desiredMoveDirection.y = 0;
                //anim.SetFloat("SwimSpeed", dir.magnitude);
            }
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
        if (!isSwimming)
        {
            if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.F)) && clipInfo[0].clip.name != "SpellCast")
            {
                anim.SetTrigger("SpellCast");
                FindObjectOfType<AudioManager>().Play("PlayerCharge");
                if (playerMana >= 20)
                {
                    playerMana -= 20;
                    if (playerMana < 0) playerMana = 0;
                    manaBar.SetMana(playerMana);
                    GameObject newProjectile = Instantiate(projectile, spawnPoint.transform.position, Quaternion.identity);
                    pc = newProjectile.GetComponent<ProjectileController>();
                }
            }
            if (clipInfo[0].clip.name == "SpellCast" && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f)
            {
                pc.SetDirection(transform.forward);
                pc.projectileSpeed = projectileSpeed;
            }
            if ((Input.GetKeyDown(KeyCode.RightControl) || Input.GetMouseButtonDown(0)) && clipInfo[0].clip.name != "Punch")
                anim.SetTrigger("Punch");
            if ((Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(1)) && clipInfo[0].clip.name != "OverHandPunch")
            {
                anim.SetTrigger("OverHandPunch");
                FindObjectOfType<AudioManager>().Play("AxeSwing");
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        int temp = UnityEngine.Random.Range(0, 3);

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (isAlive)
            {
                switch (temp)
                {
                    case 0:
                        FindObjectOfType<AudioManager>().Play("Hurt1");
                        break;
                    case 1:
                        FindObjectOfType<AudioManager>().Play("Hurt2");
                        break;
                    case 2:
                        FindObjectOfType<AudioManager>().Play("Hurt3");
                        break;
                    case 3:
                        FindObjectOfType<AudioManager>().Play("Hurt4");
                        break;
                }
                playerHealth -= 20;
                if (playerHealth < 0) playerHealth = 0;

                if (playerHealth > 0) anim.SetTrigger("Hit");
            }
        }
    }
    private void GameOver()
    {
        Debug.Log("GameOver is being triggered");
        Scenes.GameOver();
    }

    public void Death()
    {
        isAlive = false;
        anim.SetFloat("Speed", 0);
        anim.StopPlayback();
        anim.SetTrigger("Death");
        Invoke("GameOver", 3);
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }
    public void LoadPlayer()
    {
        SaveData data = SaveSystem.LoadPlayer();

        playerHealth = data.playerHealth;
        playerMana = data.playerMana;

        Vector3 position;
        position.x = data.playerPosition[0];
        position.y = data.playerPosition[1];
        position.z = data.playerPosition[2];
        
        transform.position = position;
    }
}