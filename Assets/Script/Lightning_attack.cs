using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Creator: PC, Phatchanon Chuchat 
// Player's Ligthning Attack
public class Lightning_attack : MonoBehaviour
{
    // The particle system that represents the lightning strike
    public ParticleSystem lightningStrike;

    // The amount of damage that the lightning strike does
    [SerializeField] int damageAmount = 25;

    // A list of sound clips that will play randomly each time the lightning strike is performed
    [SerializeField] List<AudioClip> attackSounds;

    private AudioSource audioSource;

    private void Start()
    {
        // Stop the particle system immediately when the character is created
        lightningStrike.Stop();

        // Get the AudioSource component on this gameobject
        audioSource = GetComponent<AudioSource>();
    }

    // Function that is called when the lightning strike is performed
    public void OnAttack()
    {
        Debug.Log("OnAttack called");

        // Play a random attack sound
        if (attackSounds.Count > 0)
        {
            int randomIndex = Random.Range(0, attackSounds.Count);
            audioSource.clip = attackSounds[randomIndex];
            audioSource.Play();
        }

        LaunchLightningStrike();
    }

    private void LaunchLightningStrike()
    {
        Debug.Log("Launching lightning strike");
        // Start the particle system
        lightningStrike.Play();

        // Find all enemies in the area
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5.0f);
        foreach (Collider collider in colliders)
        {
            // Check if the current object has the "Enemy" tag
            if (collider.CompareTag("Enemy"))
            {
                // Get the enemy's health component
                EnemyHealth enemyHealth = collider.GetComponent<EnemyHealth>();
                // Deal damage to the enemy
                enemyHealth.TakeDamage(damageAmount);
            }
        }
    }
}