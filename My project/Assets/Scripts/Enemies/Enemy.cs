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
        if (active)
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
        this.health -= value;
        this.enemyHealth.UpdateHealthBar(this.health, this.maxHealth);
        if (this.health <= 0) 
            StartCoroutine(DeathRoutine(deathDuration));
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHit") || other.CompareTag("PlayerHitAxe"))
            Damage(100);
        else if (other.CompareTag("PlayerProjectile"))
            Damage(50);
    }
    private IEnumerator DeathRoutine(float duration)
    {
        this.active = false;
        if (this.health <= 0)
        {
            if (this.anim)
            {
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
        this.health = value;
        this.maxHealth = value;
        this.enemyHealth.UpdateHealthBar(health, maxHealth);
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
        float angle = Vector3.Angle(this.transform.forward, pc.transform.forward);
        return angle < 45.0f;
    }
    protected void AgentInit()
    {
        this.agent = GetComponent<NavMeshAgent>();
    }
}