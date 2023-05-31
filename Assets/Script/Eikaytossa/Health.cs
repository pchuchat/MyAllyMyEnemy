using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Creator: PC, Phatchanon Chuchat
// TODO:
//  - 
//
// 

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

 
    void Start()
    {
        currentHealth = maxHealth;
        // Find the GameManager in the scene and assign it to the gameManager variable
    
        
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " took " + damage + " damage. Current health: " + currentHealth);

        // Check if the player has died
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Do anything you want when the player dies
     

        // Destroy the player object
        Destroy(gameObject);

     
    }
}