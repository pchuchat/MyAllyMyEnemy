using UnityEngine;
using UnityEngine.InputSystem;
// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Creator: Kettunen. J
//
// Gives the player the ability to push and pull objects that have the tag pushable_object
public class PlayerPushPull : MonoBehaviour
{
    [Header("Pushing attributes")]
    [Tooltip("The speed that the player can push objects")] [SerializeField] private float pushSpeed = 4.0f;

    [Header("Pushing sounds")]
    [Tooltip("The sound Haba makes when starting pushing")] [SerializeField] private AudioClip startPushingSound;
    [Tooltip("The sound Haba makes when stopping pushing")] [SerializeField] private AudioClip stopPushingSound;

    //Player
    private CharacterController controller;
    private PlayerInput input;
    private AudioSource playerAudioSource;
    private PlayerMovement playerMovement;
    private Vector2 movementInput;
    private bool pushing = false;

    //Pushable object
    private bool hitDirectionX;
    private GameObject pushableObject;
    private Rigidbody pushableObjectRb;
    private AudioSource pushableObjAudioSource;
    private InteractableDetection interactor;

    void Start()
    {
        //Getting the neccessary components from player
        playerMovement = GetComponent<PlayerMovement>();
        controller = GetComponent<CharacterController>();
        input = GetComponent<PlayerInput>();
        playerAudioSource = GetComponent<AudioSource>();
        interactor = GetComponent<InteractableDetection>();
    }

    /// <summary>
    /// Callback for Interact button pressed:
    /// If the Raycast detects a pushable object, and interact button is pressed
    /// sets the player as parent of the movable object, so the object determines the direction.
    /// And disable player movement for the duration of the press.
    /// </summary>
    /// <param name="context">interact button</param>
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started && pushableObject == null && controller.isGrounded)
        {
            //CheckForPushableObject();
            pushableObject = interactor.GetInteractable("pushable_object");
            if (pushableObject != null)
            {
                pushableObjAudioSource = pushableObject.GetComponent<AudioSource>();
                pushableObjAudioSource.loop = true;
                pushableObjectRb = pushableObject.GetComponent<Rigidbody>();
                SetDirectionOfPush();
            }
        }
        if (context.started && pushableObject != null)
        {
            pushableObjectRb.constraints = RigidbodyConstraints.FreezeRotation;
            input.actions.FindAction("Jump").Disable();
            playerMovement.enabled = false;
            pushing = true;
        }
        if (context.canceled && pushing)
        {
            if(pushableObjAudioSource.isPlaying)
            {
                pushableObjAudioSource.Stop();
                playerAudioSource.clip = stopPushingSound;
                playerAudioSource.Play();
            }
            pushableObjectRb.constraints = RigidbodyConstraints.FreezeAll;
            input.actions.FindAction("Jump").Enable();
            playerMovement.enabled = true;
            pushing = false;
            pushableObject = null;
            pushableObjAudioSource = null;
            interactor.InteractionFinished();
        }
    }
    /// <summary>
    /// Callback for move input.
    /// </summary>
    /// <param name="context">Player input</param>
    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Calculating the axis of an object that was hit
    /// Assigns the value true to hitDirection x if the object was hit along the x axis
    /// Assigns the value false to hitDirection x if the object was hit along the z axis
    /// </summary>
    /// <param name="hit">hit from raycast ect.</param>
    private void SetDirectionOfPush()
    {
        //Determining the axis of the hit, to move accordingly.
        //True means the object was hit along the x axis,
        //and false means the object was hit along the z axis
        Vector3 impactPoint = pushableObject.transform.InverseTransformPoint(transform.position);
        Vector3 localDir = impactPoint.normalized;

        float xDot = Vector3.Dot(localDir, Vector3.forward);
        float zDot = Vector3.Dot(localDir, Vector3.right);

        float x = Mathf.Abs(xDot);
        float z = Mathf.Abs(zDot);
        if (x < z)
        {
            hitDirectionX = true;
        }
        else
        {
            hitDirectionX = false;
        }
    }

    private void FixedUpdate()
    {
        if (pushing)
        {
            //Player movement input to vector3
            Vector3 movementInputV3 = new(movementInput.x, 0, movementInput.y);

            float magnitude;
            Vector3 direction;

            //Calculating the player input magnitude along the given axis and movin the object.
            if (hitDirectionX)
            {
                direction = Vector3.right;
                magnitude = Vector3.Dot(movementInputV3, pushableObjectRb.transform.right);
            }
            else
            {
                direction = Vector3.forward;
                magnitude = Vector3.Dot(movementInputV3, pushableObjectRb.transform.forward);
            }
            //Moving the object and player
            controller.Move(pushableObjectRb.velocity * Time.fixedDeltaTime);
            pushableObjectRb.AddRelativeForce(magnitude * pushSpeed * direction - pushableObject.transform.InverseTransformDirection(pushableObjectRb.velocity), ForceMode.VelocityChange);
            
            //Playing the sounds for pushing
            if (magnitude != 0 && !pushableObjAudioSource.isPlaying)
            {
                playerAudioSource.clip = startPushingSound;
                playerAudioSource.Play();
                pushableObjAudioSource.Play();
            }
            else if (magnitude == 0 && pushableObjAudioSource.isPlaying)
            {
                pushableObjAudioSource.Stop();
                playerAudioSource.clip = stopPushingSound;
                playerAudioSource.Play();
            }
        }
    }
}