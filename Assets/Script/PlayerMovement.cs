
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

    // Strength of gravity affecting the player
    [SerializeField] private float gravityValue = -9.81f;

    // AudioClip for single jump
    [SerializeField] private AudioClip jumpSound;

    // AudioClip for double jump
    [SerializeField] private AudioClip doublejumpSound;

    private bool facingRight = true; // added variable to keep track of facing direction


    // Reference to the CharacterController component
    private CharacterController controller;

    // Current velocity of the player
    private Vector3 playerVelocity;

    // Whether or not the player is currently grounded
    private bool groundedPlayer;

    // AudioSource for jump sounds
    private AudioSource audioSource;

    // Input value for movement direction
    private Vector2 movementInput = Vector2.zero;

    // Whether or not the player can double jump
    private bool canDoubleJump = false;


    private void Start()
    {
        // Get the CharacterController component on this object
        controller = gameObject.GetComponent<CharacterController>();

        // Get the AudioSource component on this object
        audioSource = GetComponent<AudioSource>();
    }

    public void onMove(InputAction.CallbackContext context)
    {
        // Read the value of the movement input and store it in movementInput
        movementInput = context.ReadValue<Vector2>();
    }

    public void onJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (groundedPlayer)
            {
                // Play single jump sound
                audioSource.clip = jumpSound;
                audioSource.Play();

                // Set player velocity for single jump
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);

                // Allow double jump
                canDoubleJump = true;
            }
            else if (canDoubleJump)
            {
                // Play double jump sound
                audioSource.clip = doublejumpSound;
                audioSource.Play();

                // Set player velocity for double jump
                playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);

                // Disable double jump until next grounded state
                canDoubleJump = false;
            }
        }
    }

    void Update()
    {
        // Check if the player is currently grounded
        groundedPlayer = controller.isGrounded;

        // Reset player velocity to zero when grounded
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;

            // Disable double jump until next jump
            canDoubleJump = false;
        }

        // Calculate the player's movement vector and move the player
        Vector3 move = new Vector3(movementInput.x, 0, movementInput.y);
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero) // added code to face the player in the direction of movement
        {
            transform.forward = move.normalized;
            if (facingRight && move.x < 0)
            {
                Flip();
            }
            else if (!facingRight && move.x > 0)
            {
                Flip();
            }
        }

        // Apply gravity to the player's velocity and move the player
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void Flip() // added function to flip the character horizontally
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}