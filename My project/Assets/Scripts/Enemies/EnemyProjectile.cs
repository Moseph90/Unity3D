using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyProjectile : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] private float speed;
    private Vector3 moveDirection;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        rb = GetComponent<Rigidbody>();
        moveDirection = (player.transform.position - transform.position).normalized;

        if (!player) Debug.Log("Player not found in the enemy projectile script");
        else Debug.Log("Player found in the enemy projectile script");

        rb.velocity = moveDirection * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) Destroy(gameObject);
    }
}
