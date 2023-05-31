using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Creator: PC, Phatchanon Chuchat 
// Enemy for Testing
public class EnemyHealth : MonoBehaviour
{
    // The starting health of the enemy
    public int startingHealth = 100;

    // The current health of the enemy
    public int currentHealth;

    private void Start()
    {
        // Set the current health to the starting health when the enemy is created
        currentHealth = startingHealth;
    }

    // Function that is called when the enemy takes damage
    public void TakeDamage(int amount)
    {
        Debug.Log("Taking damage: " + amount);
        Debug.Log("HP left:" + currentHealth);
        // Reduce the current health by the damage amount
        currentHealth -= amount;
        // Check if the current health is less than or equal to 0
        if (currentHealth <= 0)
        {
            // Call the Die function if the enemy's health is 0 or less
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy died");
        // Destroy the enemy game object
        Destroy(gameObject);
    }
}