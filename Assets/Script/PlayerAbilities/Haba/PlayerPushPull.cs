using UnityEngine;
using UnityEngine.InputSystem;
// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Creator: Kettunen. J
//
// Gives the player the ability to push and pull objects that have the tag pushable_object
public class PlayerPushPull : MonoBehaviour
{
    [Tooltip("From how far the player can push objects")] [SerializeField] private float interactDistance = 1f;
    [Tooltip("The speed that the player can push objects")] [SerializeField] private float pushSpeed = 4.0f;

    public PlayerMovement playerMovement;
    private CharacterController controller;
    private PlayerInput input;

    private Vector2 movementInput;
    private bool pushing = false;
    private bool hitDirectionX;
    private GameObject pushableObject;
    private Rigidbody pushableObjectRb;

    private InteractionHint hint;

    void Start()
    {
        //Gets the controller of the parent player
        controller = gameObject.GetComponent<CharacterController>();
        input = GetComponentInParent<PlayerInput>();
    }
    /// <summary>
    /// Callback for Interact button pressed:
    /// If the Raycast detects a pushable object, and interact button is pressed
    /// sets the player as parent of the movable object, so the object determines the direction.
    /// And disable player movement for the duration of the press.
    /// When player releases interact button, removes the child/parent relation and activates player movement again.
    /// </summary>
    /// <param name="context">interact button</param>
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started && pushableObject == null)
        {
            CheckForPushableObject();
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
            pushableObjectRb.constraints = RigidbodyConstraints.FreezeAll;
            input.actions.FindAction("Jump").Enable();
            playerMovement.enabled = true;
            pushing = false;
            pushableObject = null;
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
    private void SetDirectionOfHit(RaycastHit hit)
    {
        //Determining the axis of the hit, to move accordingly.
        //True means the object was hit along the x axis,
        //and false means the object was hit along the z axis
        Vector3 impactPoint = hit.transform.InverseTransformPoint(hit.point);
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

    /// <summary>
    /// Checks if there is a pushable object in front of the player
    /// and if it is within reach defined by the length of the ray
    /// </summary>
    private void CheckForPushableObject()
    {
        // Checking if the object hit with the ray is also pushable
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, interactDistance)
            && hit.collider.gameObject.CompareTag("pushable_object") && controller.isGrounded)
        {
            //Getting the object from collision
            pushableObject = hit.collider.gameObject;
            pushableObjectRb = pushableObject.GetComponent<Rigidbody>();
            SetDirectionOfHit(hit);
        }
        else
        {
            pushableObject = null;
        }
    }

    void Update()
    {
        if (hint != null)
        {
            hint.DeActivate();
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, interactDistance)
               && hit.collider.gameObject.CompareTag("pushable_object") && controller.isGrounded && !pushing)
        {
            hint = hit.collider.gameObject.GetComponentInChildren<InteractionHint>();
            hint.Activate();
        }
        if (pushing)
        {
        }
    }
    private void FixedUpdate()
    {

        if (pushing)
        {
            //Player movement input to vector3
            Vector3 movementInputV3 = new(movementInput.x, 0, movementInput.y);
            Vector3 direction;

            //The magnitude of player input towards the direction of pushing
            float magnitude;

            //Calculating the player input magnitude along the given axis,
            //and movin the object together with the player.
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
            controller.Move(pushableObjectRb.velocity * Time.fixedDeltaTime);
            pushableObjectRb.AddRelativeForce(direction * magnitude * pushSpeed - pushableObject.transform.InverseTransformDirection(pushableObjectRb.velocity), ForceMode.VelocityChange);
        }
    }
}