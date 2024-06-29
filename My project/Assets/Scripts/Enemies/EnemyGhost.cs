using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class EnemyGhost : Enemy
{
    [SerializeField] private float DeathDuration;
    [SerializeField] private float enemySpeed;
    [SerializeField] private float animTime;
    [SerializeField] private float rotationSpeed;

    protected override void Start()
    {
        base.Start();

        UpdateHealth(100);
        AgentInit();

        deathDuration = DeathDuration;
    }
    protected override void Update()
    {
        base.Update();

        if (clipInfo.clip != null && clipInfo.clip.name != "Death" && clipInfo.clip.name != "Attack" && PlayerController.isAlive && dist > 5 && dist < 50 && active)
        {
            Rotate(rotationSpeed);

            if (IsTargetObjectInFront() && active)
            {
                //Vector3 moveDirection = pc.gameObject.transform.position;
                agent.isStopped = false;
                anim.CrossFade("Walk", animTime);
                //transform.position = Vector3.Lerp(transform.position, moveDirection, enemySpeed * Time.deltaTime);
                agent.SetDestination(pc.transform.position);
            }
            else if (!IsTargetObjectInFront() && active)
            {
                anim.CrossFade("Idle", animTime);
                agent.isStopped = true;
            }
        }
        if (!pc) Debug.Log("Player Not Found");

        if (PlayerController.isAlive && dist <= 5 && clipInfo.clip != null && clipInfo.clip.name != "Death") 
        {
            agent.isStopped = true;
            anim.StopPlayback();
            anim.CrossFade("Attack", animTime);
            FindObjectOfType<AudioManager>().Play("ZombieAttack");
            Vector3 moveDirection = pc.gameObject.transform.position;
            transform.position = Vector3.Lerp(transform.position, moveDirection, enemySpeed * Time.deltaTime);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}