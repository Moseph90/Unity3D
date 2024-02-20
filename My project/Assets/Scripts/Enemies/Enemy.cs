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

    protected virtual void Start()
    {
        this.pc = FindObjectOfType<PlayerController>();
        this.anim = GetComponent<Animator>();
        this.enemyHealth = GetComponentInChildren<EnemyHealth>();
        this.rb = GetComponent<Rigidbody>();
        this.agent = GetComponent<NavMeshAgent>();
        this.collider = GetComponentInChildren<Collider>();
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
        if (this.health <= 0)
        {
            if (this.anim)
            {
                Debug.Log("Playing death animation...");
                this.anim.StopPlayback();
                this.anim.Play("Death");
                Destroy(this.collider);
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
}
