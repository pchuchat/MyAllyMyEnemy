
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
    [SerializeField] private AudioClip hyppyAani;//Lisäsin hyppyääntä varten J.K.

    // AudioClip for double jump
    [SerializeField] private AudioClip tuplahyppyAani;//Lisäsin hyppyääntä varten J.K.

    // Reference to the CharacterController component
    private CharacterController controller;

    // Current velocity of the player
    private Vector3 playerVelocity;

    // Whether or not the player is currently grounded
    private bool groundedPlayer;

    // AudioSource for jump sounds
    private AudioSource aaniLahto; //Lisäsin hyppyääntä varten J.K.

    // Input value for movement direction
    private Vector2 movementInput = Vector2.zero;

    // Whether or not the player can double jump
    private bool canDoubleJump = false;


    private void Start()
    {
        // Get the CharacterController component on this object
        controller = gameObject.GetComponent<CharacterController>();

        // Get the AudioSource component on this object
        aaniLahto = GetComponent<AudioSource>();  //Lisäsin hyppyääntä varten J.K.
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
                aaniLahto.clip = hyppyAani;
                aaniLahto.Play();

                // Set player velocity for single jump
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);

                // Allow double jump
                canDoubleJump = true;
            }
            else if (canDoubleJump)
            {
                // Play double jump sound
                //Lisäsin hyppyääntä varten J.K.
                aaniLahto.clip = tuplahyppyAani;
                aaniLahto.Play();

                // Set player velocity for double jump
                //Lisäsin hyppyääntä varten J.K.
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

        // Apply gravity to the player's velocity and move the player
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
