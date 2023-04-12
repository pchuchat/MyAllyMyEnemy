
using UnityEngine;
using UnityEngine.InputSystem;

// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Creator: PC, Phatchanon Chuchat 
// Kipinä's ElectricLine interaction
public class KipinaElectricLineAbility : MonoBehaviour
{

    [SerializeField] private float electricLineSpeed = 5.0f; // The speed at which the player moves along the electric line
    [SerializeField] private GameObject electricBall; // The electric ball object that follows the player along the electric line
    [SerializeField] private GameObject electricBallHolder; // The holder object for the electric ball
    [SerializeField] private AudioClip electricLineLoopSound; // The sound played while the player is on the electric line
    [SerializeField] private AudioClip electricLineEnterSound; // The sound played when the player enters the electric line
    [SerializeField] private AudioClip electricLineExitSound; // The sound played when the player exits the electric line



    // Private variables
    private CharacterController characterController;
    private PlayerMovement playerMovement;
    private bool isOnElectricLine = false;
    private LineRenderer currentElectricLine;
    private float currentLineProgress = 0.0f;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float electricLineDistance;
    private GameObject interactableObject;
    private InteractableDetection interactor;
    private bool hasInteracted = false;
    private bool hasJumpedOffLine = false;
    private AudioSource audioSource;







    private void Start()
    {
        // Initialize variables
        electricBall = electricBallHolder.transform.GetChild(0).gameObject;
        SetElectricBallActive(false);
        characterController = GetComponent<CharacterController>();
        playerMovement = GetComponent<PlayerMovement>();
        interactor = GetComponent<InteractableDetection>();
        Time.fixedDeltaTime = 0.02f;
        audioSource = gameObject.AddComponent<AudioSource>();



    }


    // Set the electric ball object's visibility and position
    private void SetElectricBallActive(bool active)
    {
        electricBall.GetComponent<MeshRenderer>().enabled = active;
        electricBall.transform.position = startPosition;

    }
    // Called when the player interacts with the environment
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && !isOnElectricLine && !hasInteracted)
        {
            // Get the interactable object
            interactableObject = interactor.GetInteractable("ElectricLine");

            if (interactableObject != null)
            {
                // Get the current electric line
                currentElectricLine = interactableObject.GetComponent<LineRenderer>() ?? interactableObject.transform.parent.GetComponent<LineRenderer>();

                // Set the start and end positions of the electric line
                startPosition = currentElectricLine.GetPosition(0);
                endPosition = currentElectricLine.GetPosition(1);

                // Check which point is closer to the player
                float distToStart = Vector3.Distance(transform.position, startPosition);
                float distToEnd = Vector3.Distance(transform.position, endPosition);

                if (distToStart < distToEnd)
                {
                    electricLineDistance = Vector3.Distance(startPosition, endPosition);
                }
                else
                {
                    electricLineDistance = Vector3.Distance(endPosition, startPosition);
                    Vector3 temp = startPosition;
                    startPosition = endPosition;
                    endPosition = temp;
                }

                // Start moving the player along the electric line
                isOnElectricLine = true;
                playerMovement.enabled = false;
                characterController.enabled = false;

                // Activate the electric ball object
                SetElectricBallActive(true);

                // Play the electric line enter sound
                audioSource.PlayOneShot(electricLineEnterSound);

                // Play the electric line loop sound
                audioSource.clip = electricLineLoopSound;
                audioSource.loop = true;
                audioSource.Play();

                // Aseta hahmon sijainti linjan alkuun
                transform.position = startPosition;

                // Aseta currentLineProgress oikeaan arvoon suhteessa hahmon sijaintiin linjalla
                float closestPointDistance = Mathf.Clamp(Vector3.Distance(transform.position, startPosition), 0, electricLineDistance);
                currentLineProgress = closestPointDistance;
                hasInteracted = true; // Lisää tämä rivi
                isOnElectricLine = true;

            }
        }

    }

    private void FixedUpdate()
    {
        // Check if the player is currently on the electric line
        if (isOnElectricLine)
        {
            // Set the player's position to the electric line's starting point
            transform.position = startPosition;

            // Update the player's position along the electric line using linear interpolation
            transform.position = Vector3.Lerp(startPosition, endPosition, currentLineProgress / electricLineDistance);

            // Increment the current line progress based on the electric line's speed and fixed delta time
            currentLineProgress += electricLineSpeed * Time.fixedDeltaTime;

            // Check if the player has reached the end of the electric line
            if (currentLineProgress >= electricLineDistance)
            {
                // Reset the electric line variables
                currentElectricLine = null;
                startPosition = Vector3.zero;
                endPosition = Vector3.zero;
                electricLineDistance = 0.0f;
                isOnElectricLine = false;

                // Re-enable the player's movement and character controller
                playerMovement.enabled = true;
                characterController.enabled = true;

                // Set the ElectricBall object to inactive
                SetElectricBallActive(false);

                // Free up the interactor
                interactor.InteractionFinished();

                // Set the hasJumpedOffLine and hasInteracted flags to false
                hasJumpedOffLine = true;
                isOnElectricLine = false;
                hasInteracted = false;

                // Stop playing the electric line loop sound
                audioSource.Stop();
            }
        }
    }
}