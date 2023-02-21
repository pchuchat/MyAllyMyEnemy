using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class kokeilu : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private AudioSource aaniLahto; //Lis‰sin hyppy‰‰nt‰ varten J.K.
    [SerializeField]
    private AudioClip hyppyAani; //Lis‰sin hyppy‰‰nt‰ varten J.K.

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private Vector2 movementInput = Vector2.zero;
    private bool canDoubleJump = false;


    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        aaniLahto = GetComponent<AudioSource>(); //Lis‰sin hyppy‰‰nt‰ varten J.K.
    }

    public void onMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void onJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (groundedPlayer)
            {
                HyppyAani(); //Lis‰sin hyppy‰‰nt‰ varten J.K.
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
                canDoubleJump = true;
            }
            else if (canDoubleJump)
            {
                HyppyAani(); //Lis‰sin hyppy‰‰nt‰ varten J.K.
                playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
                canDoubleJump = false;
            }
        }
    }

    //Lis‰sin hyppy‰‰nt‰ varten J.K.
    public void HyppyAani()
    {
        aaniLahto.clip = hyppyAani;
        aaniLahto.Play();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            canDoubleJump = false;
        }

        Vector3 move = new Vector3(movementInput.x, 0, movementInput.y);
        controller.Move(move * Time.deltaTime * playerSpeed);

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
