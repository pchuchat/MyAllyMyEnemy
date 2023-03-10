using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Creator: Kettunen. J
// TODO:
//  - Direction calculations not working properly yet
//  - Player input buffer keeps jumps and yeets the player into space if jump is pressed
//      repeatedly while pulling objects
//
// Gives the player the ability to move objects that have the tag pushable_object
public class PlayerPushPull : MonoBehaviour
{
    [SerializeField] private float pushDistance = 1f;
    [SerializeField] private float pushSpeed = 4.0f;

    public PlayerMovement playerMovement;
    private CharacterController controller;
    private PlayerInput input;

    private Vector2 movementInput;
    private bool pushing = false;
    private bool hitDirectionX;
    private GameObject pushableObject;

    void Start()
    {
        //Gets the controller of the parent player
        controller = gameObject.GetComponent<CharacterController>();
        input = GetComponentInParent<PlayerInput>();
        //controls = gameObject.GetComponent<PlayerControl>();
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
        if(context.started && pushableObject != null)
        {
            input.actions.FindAction("Jump").Disable();
            playerMovement.enabled = false;
            controller.transform.SetParent(pushableObject.transform);
            pushing = true;
        }
        if (context.canceled && pushing)
        {
            input.actions.FindAction("Jump").Enable();
            playerMovement.enabled = true;
            controller.transform.parent = null;
            pushing = false;
            movementInput = Vector2.zero;
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
    /// Checks if there is a pushable object in front of the player
    /// and if it is within reach defined by the length of the ray
    /// </summary>
    private void CheckForPushableObject()
    {
        RaycastHit hit;
        // Checking if the object hit with the ray is also pushable
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pushDistance)
            && hit.collider.gameObject.CompareTag("pushable_object"))
        {
            //Drawing a ray for testing purposes
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            Debug.Log("Movable object detected");

            //Getting the object from collision
            pushableObject = hit.collider.gameObject;

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
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward), Color.white);
            Debug.Log("Nothing detected");
            pushableObject = null;
        }
    }

    void Update()
    {
        if (pushing)
        {
            Vector3 move;
            // Calculate the player's movement vector and move the player
            if (hitDirectionX)
            {
                move = new Vector3(movementInput.x, 0, 0) * pushSpeed;
            }
            else
            {
                move = new Vector3(0, 0, movementInput.y) * pushSpeed;
            }
            //move = AdjustToOrientation(move, pushableObject);
            pushableObject.transform.Translate(move * Time.deltaTime, Space.Self);
        }
    }
    void FixedUpdate()
    {
        CheckForPushableObject();
    }

}
