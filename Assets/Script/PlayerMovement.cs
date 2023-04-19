
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Creator: PC, Phatchanon Chuchat and J.K, Janne Kettunen
// PlayerMovement

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    // Movement speed of the player
    [SerializeField] private float playerSpeed = 2.0f;

    // Height of the player's jump
    [SerializeField] private float jumpHeight = 1.0f;

    // Ratio of the height of the players double jump
    [SerializeField] private float doublejumpRatio = 1.0f;

    // Strength of gravity affecting the player
    [SerializeField] private float gravityValue = -9.81f;

    // AudioClips for jumping
    [Header("Sounds")]
    [Tooltip("Chance to play sounds, 100% to play always")] [SerializeField] private float chanceToPlay = 80;
    [Tooltip("Audioclips for single jump")] [SerializeField] private List<AudioClip> jumpSounds;
    [Tooltip("Audioclips for double jump")] [SerializeField] private List<AudioClip> doublejumpSounds;
    [Tooltip("Jump sounds that play always when player jumps")] [SerializeField] private List<AudioClip> alwaysPlayJumpSounds;



    // Reference to the CharacterController component
    private CharacterController controller;

    // Current velocity of the player
    private Vector3 playerVelocity;

    // Whether or not the player is currently grounded
    private bool groundedPlayer;

    // Input value for movement direction
    private Vector2 movementInput = Vector2.zero;

    // Whether or not the player can double jump
    private bool canDoubleJump = false;

    // Random sound player for effects
    private RandomSoundPlayer randomizer;

    private float coyoteTimer;


    private void Start()
    {
        // Get the CharacterController component on this object
        controller = gameObject.GetComponent<CharacterController>();
        // Get random sound player for soundeffects
        randomizer = GetComponent<RandomSoundPlayer>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // Read the value of the movement input and store it in movementInput
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (coyoteTimer > 0)
            {
                coyoteTimer = 0;
                // Play single jump sound
                randomizer.Play(jumpSounds, chanceToPlay);
                randomizer.Play(alwaysPlayJumpSounds);

                // Set player velocity for single jump
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);

                // Allow double jump
                canDoubleJump = true;
            }
            else if (canDoubleJump)
            {
                // Play double jump sound
                randomizer.Play(doublejumpSounds, chanceToPlay);
                randomizer.Play(alwaysPlayJumpSounds);

                // Set player velocity for double jump when going up
                if (playerVelocity.y >= 0)
                {
                    playerVelocity.y = Mathf.Sqrt(jumpHeight * doublejumpRatio * -3.0f * gravityValue);
                }
                // Set player velocity for double jump when going down (only lowers downwards momentum to it's square root before adding the updwards momentum)
                else
                {
                    playerVelocity.y = -Mathf.Sqrt(-playerVelocity.y) + Mathf.Sqrt(jumpHeight * doublejumpRatio * -3.0f * gravityValue);
                }

                // Disable double jump until next grounded state
                canDoubleJump = false;
            }
        }
    }

    public bool PlayerGrounded()
    {
        if (coyoteTimer > 0) return true;
        else return false;
    }

    void Update()
    {
        // Check if the player is currently grounded
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer)
        {
            coyoteTimer = 0.2f;
        }
        if (coyoteTimer > 0)
        {
            coyoteTimer -= Time.deltaTime;
        }
        // Reset player velocity to zero when grounded
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;

            // Disable double jump until next jump
            canDoubleJump = false;
        }

        // Calculate the player's movement vector and move the player
        Vector3 move = new(movementInput.x, 0, movementInput.y);
        controller.Move(playerSpeed * Time.deltaTime * move);

        // Face the player in the direction of movement
        if (move != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(move);
        }

        // Apply gravity to the player's velocity and move the player
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

}