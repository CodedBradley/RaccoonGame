using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Health : MonoBehaviour
{
    [Range(0, 200)] public int startHealth = 100, currentHealth;


    public static bool isDead;
    private void Start()
    {
        currentHealth = startHealth;
    }

    private void Update()

    {
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            Debug.Log("Deaded");
        }

    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
    }


}
