using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
// �GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Creator: Hopeasaari J.
// TODO:
//  - Let players pair their gamepads by themselves, rather than assigning them arbitrary ones (before starting a level on the main menu, once it's implemented)
//  - Dynamic checking and pairing of gamepads connected during gameplay, now only pairs on loading the main scene
//
// Instantiates the player objects, assigning them prefabs, control schemes and paired devices

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> playerPrefabs = new List<GameObject>();

    void Start()
    {
        // Creates the player instances and assigns them prehabs
        PlayerInput player1 = PlayerInput.Instantiate(playerPrefabs[0]);
        PlayerInput player2 = PlayerInput.Instantiate(playerPrefabs[1]);

        // Unpairs any automatically assigned devices to prevent odd behavior
        player1.user.UnpairDevices();
        player2.user.UnpairDevices();

        player1.neverAutoSwitchControlSchemes = true;
        player2.neverAutoSwitchControlSchemes = true;

        player1.user.ActivateControlScheme("Player1");
        player2.user.ActivateControlScheme("Player2");

        // Pairs the last used keyboard to both players; both have separate controls because of their control schemes
        InputUser.PerformPairingWithDevice(Keyboard.current, player1.user);
        InputUser.PerformPairingWithDevice(Keyboard.current, player2.user);

        // Pairs gamepads, if present, to the players
        // Currently pairs the first connected gamepad to player 1 and the second to player 2, can later be changed to pair based on gamepad input
        if (Gamepad.all.Count >= 1)
        {
            InputUser.PerformPairingWithDevice(Gamepad.all[0], player1.user);
        }

        if (Gamepad.all.Count >= 2)
        {
            InputUser.PerformPairingWithDevice(Gamepad.all[1], player2.user);
        }

        // Moving the players slightly so they don't spawn on the same spot
        // When actual levels are implemented the players should be moved to their designated starting coordinates for a level on entering one
        player1.GetComponent<Transform>().position -= Vector3.right * 3;
        player2.GetComponent<Transform>().position += Vector3.right * 3;
    }
}
