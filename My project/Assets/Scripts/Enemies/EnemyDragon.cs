using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class EnemyDragon : Enemy
{
    [SerializeField] private float DeathDuration;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float animSpeed;
    public bool sight;

    private Quaternion originalRotation;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        UpdateHealth(500);
        deathDuration = DeathDuration;
        originalRotation = transform.rotation;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (sight && active) Rotate(rotationSpeed);
    }

    public IEnumerator Fight()
    {
        PlayerController.isAlive = false;
        pc.anim.SetFloat("Speed", 0);
        anim.Play("Scream");
        yield return new WaitForSeconds(2);
        Debug.Log("Fight Routine Started");
        PlayerController.isAlive = true;
        while (PlayerController.isAlive && active)
        {
            while (sight)
            {
                float currentTime = 0;
                while (currentTime < 3) 
                {
                    Rotate(rotationSpeed);
                    currentTime += Time.deltaTime;
                }
                //yield return null;
                int clip = Random.Range(1, 2);
                if (dist > 20 && active) anim.CrossFade("Horn Attack", animSpeed);
                else if (dist <= 20 && dist > 15 && active)
                {
                    if (clip == 1 && active) anim.CrossFade("Claw Attack", animSpeed);
                    if (clip == 2 && active) anim.CrossFade("Basic Attack", animSpeed);
                }
                else if (dist < 16 && active) anim.CrossFade("Jump", animSpeed);
                //yield return null;
                yield return new WaitForSeconds(4);
            }
            while (!sight && active)
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, rotationSpeed * Time.deltaTime);
        }
        Debug.Log("Fight Routine Ended");
        yield return null;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
