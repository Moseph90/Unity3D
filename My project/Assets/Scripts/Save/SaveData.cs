using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.RestService;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int playerMana;
    public int playerHealth;
    public float[] playerPosition;

    public SaveData(PlayerController player)
    {
        playerHealth = PlayerController.playerHealth;
        playerMana = PlayerController.playerMana;
        playerPosition = new float[3];

        playerPosition[0] = player.transform.position.x;
        playerPosition[1] = player.transform.position.y;
        playerPosition[2] = player.transform.position.z;
    }
}