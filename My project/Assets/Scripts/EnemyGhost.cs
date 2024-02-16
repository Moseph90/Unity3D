using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyGhost : Enemy
{
    public GameObject player;
    private Transform referenceObject;
    public float DeathDuration;

    public float enemySpeed;

    public float animTime;

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        if (!player) Debug.Log("Player is missing");
        deathDuration = DeathDuration;
    }

    void Update()
    {
        AnimatorClipInfo[] tempClipInfo = anim.GetCurrentAnimatorClipInfo(0);
        AnimatorClipInfo clipInfo = default;

        if (tempClipInfo.Length > 0)
            clipInfo = tempClipInfo[0];

        float dist = Vector3.Distance(transform.position, player.transform.position);


        if (player && clipInfo.clip != null && clipInfo.clip.name != "Death" && clipInfo.clip.name != "Attack" && PlayerController.isAlive && dist > 5)
        {
            transform.LookAt(player.transform);

            if (IsTargetObjectInFront())
            {
                Vector3 moveDirection = player.gameObject.transform.position;
                anim.CrossFade("Walk", animTime);
                transform.position = Vector3.Lerp(transform.position, moveDirection, enemySpeed * Time.deltaTime);
            }
            else if (!IsTargetObjectInFront())
                anim.CrossFade("Idle", animTime);
        }
        if (!player) Debug.Log("Player Not Found");

        if (PlayerController.isAlive && dist <= 5 && clipInfo.clip != null && clipInfo.clip.name != "Death") 
        {
            anim.StopPlayback();
            anim.CrossFade("Attack", animTime);
            Vector3 moveDirection = player.gameObject.transform.position;
            transform.position = Vector3.Lerp(transform.position, moveDirection, (enemySpeed/2) * Time.deltaTime);
        }
    }

    private bool IsTargetObjectInFront()
    {
        //Vector3 referenceToTarget = player.transform.position - transform.position;

        float angle = Vector3.Angle(transform.forward, player.transform.forward);
        //Debug.Log("Angle: " + angle);
        return angle < 45.0f;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}