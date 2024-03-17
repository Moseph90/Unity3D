using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class PlayerInstance : MonoBehaviour
{
    [Header("Collider Game Objects")]
    public GameObject leftPunchCollider;

    [Header("SpawnPoints")]
    [SerializeField] private GameObject leftPunchSpawnPoint;
    [SerializeField] private GameObject buffSpawnPoint;

    public float pushBackSpeed;

    private PlayerController pc;
    private float tempSpeed;
    private float tempAnimSpeed;
    private float tempAnimSlowDown;

    private CharacterController cc;

    private bool lava;
    private bool pushBack;
    private bool pushHit;
    private float time = 0;

    [SerializeField] private GameObject Dragon;
    [SerializeField] private GameObject Buff;
    private EnemyDragon dragon;
    private void Start()
    {
        dragon = Dragon.GetComponent<EnemyDragon>();
        pc = GetComponent<PlayerController>();
        tempSpeed = pc.speed;
        tempAnimSpeed = pc.anim.speed;
        tempAnimSlowDown = pc.anim.speed / 2;
    }

    private void Update()
    {
        if (pushBack)
        {
            if (time <= 0.5)
            {
                PushBack();
                time += Time.deltaTime;
            }
            else pushBack = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EndCoin"))
        {
            Destroy(other.gameObject);
            GameManager.Instance.Quit();
        }
        if (LayerMask.LayerToName(other.gameObject.layer) == "Fire")
        {
            pc.anim.SetFloat("Speed", 0);
            if (other.CompareTag("GreenFire")) StartCoroutine(PushBack("Green"));
            else if (other.CompareTag("BlueFire"))
            {
                Debug.Log("Blue On Trigger Enter");
                StartCoroutine(PushBack("Blue"));
            }
        }
 
        if (other.CompareTag("Lava"))
        {
            Debug.Log("Lava has been collided with");
            lava = true;
            StartCoroutine(SlowDamage(10));
        }
        if ((other.CompareTag("Enemy") || other.CompareTag("DragonHorn")))
        {
            PlayerController.playerHealth -= 20;
            if (PlayerController.playerHealth < 0) PlayerController.playerHealth = 0;

            if (PlayerController.playerHealth > 0) pc.anim.SetTrigger("Hit");
        }
        if (other.CompareTag("DragonDetect"))
        {
            dragon.sight = true;
            StartCoroutine(dragon.Fight());
            Debug.Log("Dragon Detect Working");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Fog") || other.CompareTag("Water"))
        {
            pc.speed = 5f;
            pc.anim.speed = tempAnimSlowDown;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fog") || other.CompareTag("Water"))
        {
            pc.speed = tempSpeed;
            pc.anim.speed = tempAnimSpeed;
        }
        if (other.CompareTag("Lava"))
        {
            lava = false;
            StopCoroutine(SlowDamage(10));
        }
        if (other.CompareTag("DragonDetect")) dragon.sight = false;
    }

    private void LeftPunchCollider()
    {
        GameObject collider = Instantiate(leftPunchCollider, leftPunchSpawnPoint.transform.position, Quaternion.identity);
    }
    private void DisableCollider()
    {
        GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag("PlayerHit");
        
        foreach(GameObject obj in objectsToDestroy)
            Destroy(obj);
    }
    private void PushBack()
    {
        if (pushHit)
        {
            pc.anim.SetFloat("Speed", 0);
            pushHit = false;
            pc.anim.StopPlayback();
            pc.anim.Play("GetHit");
        }
        Vector3 pushback = -transform.forward.normalized - new Vector3(1, 0, 1);
        Vector3 newPosition = transform.position + pushback;
        transform.position = Vector3.Lerp(transform.position, newPosition, pushBackSpeed * Time.deltaTime);
        if (transform.position == newPosition)
        {
            pushBack = false;
            time = 0;
        }
    }
    private IEnumerator freeze()
    {
        PlayerController.isAlive = false;
        pc.speed -= tempSpeed;
        yield return new WaitForSeconds(0.5f);
        PlayerController.isAlive = true;
        pc.speed += tempSpeed;
    }
    private IEnumerator PushBack(string tag)
    {   
        if (tag == "Green" || tag == "Purple" || tag == "Pink" || tag == "Yellow")
        {
            pushBack = true;
            StartCoroutine(freeze());
            yield return new WaitForSeconds(0.5f);
            if (tag == "Green") 
            {
                pc.anim.speed = 0.5f;
                pc.speed = 5f;
                yield return new WaitForSeconds(10);
                pc.anim.speed = 1;
                pc.speed = tempSpeed;
                yield return null;
            }
        }
        else if (tag == "Blue")
        {
            Debug.Log("Blue Fire Registered");
            PlayerController.isAlive = false;
            pc.anim.SetTrigger("Powerup");
            PlayerController.playerMana += 25;
            yield return new WaitForSeconds(1);
            PlayerController.isAlive = true;
        }
    }
    private IEnumerator SlowDamage(int damage)
    {
        while (lava)
        {
            if (PlayerController.isAlive)
            {
                PlayerController.playerHealth -= damage;
                pc.anim.SetTrigger("Hit");
                yield return new WaitForSeconds(2);
            }
            yield return null;
        }
        yield return null;
    }

    private void CreateBuff()
    {
        GameObject buffer = Instantiate(Buff, buffSpawnPoint.transform.position, Quaternion.identity);
    }
    private void DestroyBuff()
    {
        GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag("Buff");

        foreach (GameObject obj in objectsToDestroy)
            Destroy(obj);
    }
}