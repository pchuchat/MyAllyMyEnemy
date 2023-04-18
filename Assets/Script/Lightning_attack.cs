using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Creator: PC, Phatchanon Chuchat 
// Edited: JK, Janne Kettunen
// Player's Ligthning Attack
public class Lightning_attack : MonoBehaviour
{
    // The particle system that represents the lightning strike
    public ParticleSystem lightningStrike;

    // The amount of damage that the lightning strike does
    [SerializeField] int damageAmount = 25;

    // AudioClips for attacking
    [Header("AttackSounds")]
    [Tooltip("Audioclips for attack")] [SerializeField] private List<AudioClip> attackSounds;

    // Random sound player for effects
    private RandomSoundPlayer randomizer;

    private void Start()
    {
        // Stop the particle system immediately when the character is created
        lightningStrike.Stop();

        // Get the AudioSource component on this gameobject
        randomizer = GetComponent<RandomSoundPlayer>();
    }

    // Function that is called when the lightning strike is performed
    public void OnAttack()
    {
        Debug.Log("OnAttack called");

        // Play a random attack sound
        randomizer.Play(attackSounds);

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