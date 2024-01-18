using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawn : MonoBehaviour
{
    SpriteRenderer sr;

    public Transform[] PUSpawn;
    public GameObject[] pickUpPrefab;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            sr = GetComponent<SpriteRenderer>();
            if (!sr) throw new ArgumentException("Sprite Renderer Present");
        }
        catch (ArgumentException e)
        {
            Debug.Log(e.ToString());
        }

        for (int i = 0; i < PUSpawn.Length; i++)
        {
            //if (!PUSpawn[i] || !pickUpPrefab[i])
            //{
            //    Debug.Log("Please Set Default Values On " + gameObject.name);
            //}
            int pick = UnityEngine.Random.Range(0, 5);
            if (pick < 3)
            {
                GameObject pickups = Instantiate(pickUpPrefab[pick], PUSpawn[i].position, PUSpawn[i].rotation);
            }
        }
    }
}