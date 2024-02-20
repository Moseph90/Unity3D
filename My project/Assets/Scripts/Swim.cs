using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swim : MonoBehaviour
{
    private PlayerController pc;
    public GameObject player;

    private void Start()
    {
        pc = player.GetComponent<PlayerController>();
        if (pc) Debug.Log("PlayerController in the swim script is found");
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            PlayerController.isSwimming = true;
            Debug.Log("Entered the water");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            PlayerController.isSwimming = false;
            Debug.Log("Exited the water");
        }
    }
}
