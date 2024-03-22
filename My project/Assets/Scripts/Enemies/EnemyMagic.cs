using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMagic : Enemy
{
    [SerializeField] private float DeathDuration;
    [SerializeField] private float enemySpeed;
    [SerializeField] private float animTime;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject aura;
    [SerializeField] private GameObject explosion;
    private GameObject auraObj;
    private GameObject explosionObj;

    [SerializeField] private GameObject fireBall;
    [SerializeField] private GameObject fireBallSpawn;
    private GameObject fireBallObj;

    protected override void Start()
    {
        base.Start();

        UpdateHealth(200);
        AgentInit();
        deathDuration = DeathDuration;
    }

    protected override void Update()
    {
        base.Update();

        if (clipInfo.clip != null && clipInfo.clip.name != "Death" && clipInfo.clip.name != "Attack" 
            && clipInfo.clip.name != "Burst" && PlayerController.isAlive && dist < 50)
        {
            if (dist >= 25)
            {
                Rotate(rotationSpeed);

                agent.isStopped = false;
                anim.CrossFade("Jog", animTime);
                agent.SetDestination(pc.transform.position);
            }

            if (dist < 25 && dist > 10)
            {
                agent.isStopped = true;
                transform.LookAt(pc.transform.position);
                anim.CrossFade("Attack", animTime);
            }
            else if (dist <= 10)
            {

                anim.CrossFade("Burst", animTime);
                agent.isStopped = true;
            }
        }
        if (clipInfo.clip != null && (clipInfo.clip.name == "Attack" || clipInfo.clip.name == "Burst"))
            transform.LookAt(pc.transform.position);
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
    private void CreateAura()
    {
        auraObj = Instantiate(aura, spawnPoint.transform.position, transform.rotation);
        FindObjectOfType<AudioManager>().Play("EnemyCharge");
    }
    private void Explode()
    {
        Destroy(auraObj);
        explosionObj = Instantiate(explosion, spawnPoint.transform.position, transform.rotation);
        FindObjectOfType<AudioManager>().Play("EnemyBurst");
    }
    private void DestroyExplostion()
    {
        Destroy(explosionObj);
    }
    private void FireBall()
    {
        fireBallObj = Instantiate(fireBall, fireBallSpawn.transform.position, transform.rotation);
        FindObjectOfType<AudioManager>().Play("ElectricAttack");
    }
}
