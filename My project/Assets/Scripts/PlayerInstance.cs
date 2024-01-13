using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInstance : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EndCoin"))
        {
            Destroy(other.gameObject);
            GameManager.Instance.Quit();
        }
    }
}
