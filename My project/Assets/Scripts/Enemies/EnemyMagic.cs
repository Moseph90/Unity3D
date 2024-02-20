using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMagic : Enemy
{
    public float DeathDuration;
    public float enemySpeed;
    public float animTime;
    public float rotationSpeed;

    protected override void Start()
    {
        base.Start();

        UpdateHealth(200);
        deathDuration = DeathDuration;
    }

    private void Update()
    {
        AnimatorClipInfo[] tempClipInfo = anim.GetCurrentAnimatorClipInfo(0);
        AnimatorClipInfo clipInfo = default;

        if (tempClipInfo.Length > 0)
            clipInfo = tempClipInfo[0];

        float dist = Vector3.Distance(transform.position, pc.transform.position);

        if (clipInfo.clip != null && clipInfo.clip.name != "Death" && clipInfo.clip.name != "Attack" 
            && clipInfo.clip.name != "Burst" && PlayerController.isAlive && dist > 5 && dist < 50)
        {
            Vector3 direction = pc.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            agent.isStopped = false;
            anim.CrossFade("Jog", animTime);
            agent.SetDestination(pc.transform.position);

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
        if (clipInfo.clip != null && clipInfo.clip.name == "Attack")
        {
            transform.LookAt(pc.transform.position);
        }
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
