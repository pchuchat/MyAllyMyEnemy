using System.Collections.Generic;
using UnityEditor.Rendering;
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
    [Tooltip("The sounds Haba makes when starting pushing")] [SerializeField] private List<AudioClip> startPushingSounds;
    [Tooltip("The sounds Haba makes when stopping pushing")] [SerializeField] private List<AudioClip> stopPushingSounds;

    private Transform cameraRig;

    //Player
    private CharacterController controller;
    private PlayerInput input;
    private PlayerMovement playerMovement;
    private Vector2 movementInput;
    private bool pushing = false;
    private RandomSoundPlayer randomizer;
    private Animator animator;

    //Pushable object
    private bool hitDirectionX;
    private GameObject pushableObject;
    private Rigidbody pushableObjectRb;
    private AudioSource pushableObjAudioSource;
    private InteractableDetection interactor;
    private bool groundedPlayer;
    private float groundedDelay;

    void Start()
    {
        //Getting the neccessary components from player
        playerMovement = GetComponent<PlayerMovement>();
        controller = GetComponent<CharacterController>();
        input = GetComponent<PlayerInput>();
        interactor = GetComponent<InteractableDetection>();
        randomizer = GetComponent<RandomSoundPlayer>();
        cameraRig = Camera.main.GetComponentInParent<Transform>();
        animator = gameObject.GetComponentInChildren<Animator>();
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
        if (context.started && pushableObject == null && groundedDelay > 0)
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
            input.actions.FindAction("Attack").Disable();
            playerMovement.enabled = false;
            pushing = true;
        }
        if (context.canceled && pushing)
        {
            StopPushing();
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

    private void StopPushing()
    {

        if (pushableObjAudioSource.isPlaying)
        {
            pushableObjAudioSource.Stop();
            randomizer.Play(stopPushingSounds);
        }
        pushableObjectRb.constraints = RigidbodyConstraints.FreezeAll;
        input.actions.FindAction("Jump").Enable();
        input.actions.FindAction("Attack").Enable();
        playerMovement.enabled = true;
        pushing = false;
        pushableObject = null;
        pushableObjAudioSource = null;

        if (movementInput != Vector2.zero)
        {
            animator.SetTrigger("Walk");
        }
        else
        {
            animator.SetTrigger("Idle");
        }

        interactor.InteractionFinished();
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
    private void Update()
    {
        // Check if the player is currently grounded
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer)
        {
            groundedDelay = 0.2f;
        }
        if (groundedDelay > 0)
        {
            groundedDelay -= Time.deltaTime;
        }

        if (pushing)
        {
            if (movementInput != Vector2.zero)
            {
                animator.ResetTrigger("Walk");
                animator.ResetTrigger("Idle");
                animator.SetTrigger("Pushwalk");
            }
            else
            {
                animator.ResetTrigger("Walk");
                animator.ResetTrigger("Idle");
                animator.SetTrigger("Push");
            }
        }
    }
    private void FixedUpdate()
    {
        if (pushing)
        {
            //Player movement input to vector3 and converted to camerarelative direction
            Vector3 cameraForward = cameraRig.forward;
            Vector3 cameraRight = cameraRig.right;

            cameraForward.y = 0;
            cameraRight.y = 0;

            //Relative camera direction
            Vector3 relativeForward = movementInput.y * cameraForward;
            Vector3 relativeRight = movementInput.x * cameraRight;

            Vector3 relativeMove = relativeForward + relativeRight;

            Vector3 movementInputV3Relative = new(relativeMove.x, 0, relativeMove.z);

            float magnitude;
            Vector3 direction;

            //Calculating the player input magnitude along the given axis and movin the object.
            if (hitDirectionX)
            {
                direction = Vector3.right;
                magnitude = Vector3.Dot(movementInputV3Relative, pushableObjectRb.transform.right);
            }
            else
            {
                direction = Vector3.forward;
                magnitude = Vector3.Dot(movementInputV3Relative, pushableObjectRb.transform.forward);
            }
            //Moving the object and player
            controller.Move(pushableObjectRb.velocity * Time.fixedDeltaTime);
            pushableObjectRb.AddRelativeForce(magnitude * pushSpeed * direction - pushableObject.transform.InverseTransformDirection(pushableObjectRb.velocity), ForceMode.VelocityChange);
            
            //Playing the sounds for pushing
            if (magnitude != 0 && !pushableObjAudioSource.isPlaying)
            {
                randomizer.Play(startPushingSounds);
                pushableObjAudioSource.Play();
            }
            else if (magnitude == 0 && pushableObjAudioSource.isPlaying)
            {
                pushableObjAudioSource.Stop();
                randomizer.Play(stopPushingSounds);
            }

            RaycastHit hit;
            if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 0.1f))
            {
                StopPushing();
            }
        }
    }
}