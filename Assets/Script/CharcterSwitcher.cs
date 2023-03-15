using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Creator: PC, Phatchanon Chuchat

// spaw two different fighter

public class CharcterSwitcher : MonoBehaviour
{
    // Index of the currently selected fighter in the list
    int index = 0;

    // List of fighter GameObjects to be spawned
    [SerializeField] List<GameObject> fighters = new();

    // Reference to the PlayerInputManager component
    PlayerInputManager manager;

    // Start is called before the first frame update
    void Start()
    {
        // Get the PlayerInputManager component on this object
        manager = GetComponent<PlayerInputManager>();

        // Set the initial player prefab to the first fighter in the list
        manager.playerPrefab = fighters[index];
    }

    // Method that switches to the next fighter in the list
    public void SwitchNextSpawnCharacter(PlayerInput input)
    {
        if (input is null)
        {
            throw new ArgumentNullException(nameof(input));
        }
        // Increment the index to select the next fighter in the list
        index++;

        // If the index is greater than or equal to the number of fighters in the list, wrap around to the first fighter
        if (index >= fighters.Count)
        {
            index = 0;
        }

        // Set the player prefab to the selected fighter
        manager.playerPrefab = fighters[index];
    }
}