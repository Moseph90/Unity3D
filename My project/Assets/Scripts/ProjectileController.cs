using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private float _projectileSpeed;
    public float projectileSpeed
    {
        set
        {
            _projectileSpeed = value;
        }
    }
    
    private Vector3 moveDirection;

    // Method to set the direction of the projectile
    public void SetDirection(Vector3 direction)
    {
        //Debug.Log("Direction is set " + direction);
        moveDirection = direction.normalized;
    }

    void FixedUpdate()
    {
        // Move the projectile in the specified direction
        transform.Translate(moveDirection * _projectileSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger Enter Called");
        if (!other.CompareTag("Player") && !other.CompareTag("Water")
            && !other.CompareTag("Fog") && !other.CompareTag("DragonDetect"))
        {
            //Debug.Log("Projectile Has Collided");
            Destroy(gameObject);
        }
    }
}