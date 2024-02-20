using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void UpdateHealthBar(int health, int maxHealth)
    {
        Debug.Log("Health being changed");
        slider.value = health;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
