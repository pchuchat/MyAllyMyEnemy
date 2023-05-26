using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.SceneManagement;
// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
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

    [SerializeField]
    private GameObject cameraRig;

    [SerializeField]
    private Checkpoint[] checkpoints;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private AudioSource music;

    [SerializeField]
    private AudioClip[] screams;

    private AudioSource screamSource;

    private int lastCheckpoint = -1;

    private PlayerInput p1;
    private PlayerInput p2;

    private bool loading = false;

    void Start()
    {
        screamSource = GetComponent<AudioSource>();

        // Creates the player instances and assigns them prefabs
        PlayerInput player1 = PlayerInput.Instantiate(playerPrefabs[0]);
        PlayerInput player2 = PlayerInput.Instantiate(playerPrefabs[1]);

        p1 = player1;
        p2 = player2;

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

        // Moves the players to their proper starting coordinates in level 1
        //player1.GetComponent<Transform>().position = new Vector3(-2.0f, -0.5f, 0.0f);
        //player2.GetComponent<Transform>().position = new Vector3(3.0f, -0.23f, -3.0f);

        p1.GetComponent<Transform>().rotation = Quaternion.LookRotation(new Vector3(1f, -10f, 1f));
        p2.GetComponent<Transform>().rotation = Quaternion.LookRotation(new Vector3(2f, -10f, -1f));

        player1.GetComponent<Transform>().position = new Vector3(-119.0f, 18.0f, 178.0f);
        player2.GetComponent<Transform>().position = new Vector3(-121.0f, 18.0f, 179.0f);
        //player1.GetComponent<Transform>().position = new Vector3(-8.0f, 15.0f, 191.0f);
        //player2.GetComponent<Transform>().position = new Vector3(-5.0f, 15.0f, 191.0f);
    }

    void Update()
    {
        if (p1.GetComponent<Transform>().position.y < -10f && !loading)
        {
            loading = true;
            lastCheckpoint = 1;
            screamSource.mute = false;
            screamSource.clip = screams[0];
            screamSource.Play();
            Invoke("LoadCheckpoint", 1f);
        }

        if (p2.GetComponent<Transform>().position.y < -10f && !loading)
        {
            loading = true;
            lastCheckpoint = 1;
            screamSource.mute = false;
            screamSource.clip = screams[1];
            screamSource.Play();
            Invoke("LoadCheckpoint", 1f);
        }
    }

    // Makes the screen black
    public void LoadCheckpoint()
    {
        animator.SetTrigger("FadeOut");
    }

    // Loads current checkpoint
    // If no checkpoints reached, restarts level
    public void OnFadeComplete()
    {
        screamSource.mute = true;

        if (lastCheckpoint == -1)
        {
            SceneManager.LoadScene(1);
            loading = false;
        }
        else
        {
            Checkpoint check = checkpoints[lastCheckpoint];

            for (int i=0; i<check.scenesToReload.Length; i++)
            {
                SceneManager.UnloadSceneAsync(check.scenesToReload[i]);
                SceneManager.LoadScene(check.scenesToReload[i], LoadSceneMode.Additive);
            }

            p1.GetComponent<Transform>().position = check.p1Position;
            p2.GetComponent<Transform>().position = check.p2Position;
            cameraRig.GetComponent<Transform>().position = check.cameraPosition;

            p1.GetComponent<Transform>().rotation = Quaternion.LookRotation(check.forward);
            p2.GetComponent<Transform>().rotation = Quaternion.LookRotation(check.forward);

            loading = false;

            animator.SetTrigger("FadeIn");
        }
    }
}
