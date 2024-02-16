using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInstance : MonoBehaviour
{
    [Header("Collider Game Objects")]
    public GameObject leftPunchCollider;

    [Header("SpawnPoints")]
    public GameObject leftPunchSpawnPoint;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EndCoin"))
        {
            Destroy(other.gameObject);
            GameManager.Instance.Quit();
        }
    }

    private void LeftPunchCollider()
    {
        GameObject collider = Instantiate(leftPunchCollider, leftPunchSpawnPoint.transform.position, Quaternion.identity);
    }
    private void DisableCollider()
    {
        GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag("PlayerHit");
        
        foreach(GameObject obj in objectsToDestroy)
            Destroy(obj);
    }
}
