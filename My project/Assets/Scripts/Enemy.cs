using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected int health;
    protected int maxHealth;
    protected Rigidbody rb;
    protected Animator anim;
    protected float deathDuration { set; get; }

    protected EnemyHealth enemyHealth;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        if (!rb)
            Debug.Log("Rigidbody Not Found");
    }
    protected void Damage(int value)
    {
        health -= value;
        this.enemyHealth.UpdateHealthBar(health, maxHealth);
        if (health <= 0) 
            StartCoroutine(DeathRoutine(deathDuration, this.anim));
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHit"))
            Damage(100);
        else if (other.CompareTag("PlayerProjectile"))
            Damage(50);
    }
    private IEnumerator DeathRoutine(float duration, Animator a)
    {
        if (health <= 0)
        {
            if (a)
            {
                Debug.Log("Playing death animation...");
                a.StopPlayback();
                a.Play("Death");
            }
            yield return new WaitForSeconds(duration);
            Destroy(gameObject);
        }
    }
}
