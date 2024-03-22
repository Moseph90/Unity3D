using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    protected int health;
    protected int maxHealth;
    protected Animator anim;
    protected float deathDuration { set; get; }

    protected EnemyHealth enemyHealth;
    protected Rigidbody rb;
    protected PlayerController pc;
    protected NavMeshAgent agent;
    protected new Collider collider;

    protected AnimatorClipInfo[] tempClipInfo;
    protected AnimatorClipInfo clipInfo;
    protected float dist;
    protected bool active;

    protected virtual void Start()
    {
        pc = FindObjectOfType<PlayerController>();
        this.anim = GetComponent<Animator>();
        this.enemyHealth = GetComponentInChildren<EnemyHealth>();
        this.rb = GetComponent<Rigidbody>();
        this.active = true;
    }

    protected virtual void Update()
    {
        if (this.active)
        {
            this.tempClipInfo = this.anim.GetCurrentAnimatorClipInfo(0);
            this.clipInfo = default;

            if (this.tempClipInfo.Length > 0)
                this.clipInfo = this.tempClipInfo[0];

            this.dist = Vector3.Distance(this.transform.position, pc.transform.position);
        }
    }
    protected void Damage(int value)
    {
        AudioSource audio = GetComponent<AudioSource>();
        if (this.active)
        {
            this.health -= value;
            this.enemyHealth.UpdateHealthBar(this.health, this.maxHealth);
            if (this.health <= 0)
            {
                switch (this.maxHealth)
                {
                    case 100:
                        StartCoroutine(DeathRoutine(deathDuration, "Zombie"));
                        break;
                    case 200:
                        StartCoroutine(DeathRoutine(deathDuration, "Magic"));
                        break;
                    case 500:
                        StartCoroutine(DeathRoutine(deathDuration, "Dragon"));
                        if (audio) audio.Stop(); 
                        break;
                }
            }
        }
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (this.active)
        {
            if (other.CompareTag("PlayerHit") || other.CompareTag("PlayerHitAxe"))
            {
                if (other.CompareTag("PlayerHit")) FindObjectOfType<AudioManager>().Play("PunchHit");
                else FindObjectOfType<AudioManager>().Play("AxeHit");
                Damage(100);
            }
            else if (other.CompareTag("PlayerProjectile"))
            {
                FindObjectOfType<AudioManager>().Play("FireHit");
                Damage(50);
            }
        }
    }
    private IEnumerator DeathRoutine(float duration, string enemy)
    {
        this.active = false;
        if (this.health <= 0)
        {
            if (this.anim)
            {
                if (enemy == "Zombie") FindObjectOfType<AudioManager>().Play("ZombieDeath");
                else if (enemy == "Magic") FindObjectOfType<AudioManager>().Play("MagicEnemyDeath");
                else if (enemy == "Dragon") FindObjectOfType<AudioManager>().Play("DragonRoar");
                Debug.Log("Playing death animation...");
                this.anim.StopPlayback();
                this.anim.CrossFade("Death", 0.5f);
            }
            yield return new WaitForSeconds(duration);
            Destroy(gameObject);
        }
    }
    protected void UpdateHealth(int value)
    {
        if (this.gameObject)
        {
            this.health = value;
            this.maxHealth = value;
            this.enemyHealth.UpdateHealthBar(health, maxHealth);
        }
    }
    protected void Rotate(float speed)
    {
        if (this.gameObject)
        {
            Vector3 direction = pc.transform.position - this.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, speed * Time.deltaTime);
        }
    }
    protected bool IsTargetObjectInFront()
    {
        if (this.gameObject)
        {
            float angle = Vector3.Angle(this.transform.forward, pc.transform.forward);
            return angle < 45.0f;
        }
        else return false;
    }
    protected void AgentInit()
    {
        this.agent = GetComponent<NavMeshAgent>();
    }
}