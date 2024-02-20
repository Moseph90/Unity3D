using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class EnemyGhost : Enemy
{
    public float DeathDuration;

    public float enemySpeed;

    public float animTime;

    public float rotationSpeed;

    protected override void Start()
    {
        base.Start();
        UpdateHealth(100);

        //if (!player) Debug.Log("Player is missing");
        deathDuration = DeathDuration;
    }

    void Update()
    {
        AnimatorClipInfo[] tempClipInfo = anim.GetCurrentAnimatorClipInfo(0);
        AnimatorClipInfo clipInfo = default;

        if (tempClipInfo.Length > 0)
            clipInfo = tempClipInfo[0];

        float dist = Vector3.Distance(transform.position, pc.transform.position);


        if (clipInfo.clip != null && clipInfo.clip.name != "Death" && clipInfo.clip.name != "Attack" && PlayerController.isAlive && dist > 5 && dist < 50)
        {
            Vector3 direction = pc.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (IsTargetObjectInFront())
            {
                //Vector3 moveDirection = pc.gameObject.transform.position;
                agent.isStopped = false;
                anim.CrossFade("Walk", animTime);
                //transform.position = Vector3.Lerp(transform.position, moveDirection, enemySpeed * Time.deltaTime);
                agent.SetDestination(pc.transform.position);
            }
            else if (!IsTargetObjectInFront())
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
            Vector3 moveDirection = pc.gameObject.transform.position;
            transform.position = Vector3.Lerp(transform.position, moveDirection, (enemySpeed) * Time.deltaTime);
        }
    }

    private bool IsTargetObjectInFront()
    {
        //Vector3 referenceToTarget = player.transform.position - transform.position;

        float angle = Vector3.Angle(transform.forward, pc.transform.forward);
        //Debug.Log("Angle: " + angle);
        return angle < 45.0f;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}