using UnityEngine;
using UnityEngine.InputSystem;
// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Creator: Kettunen. J
//  TODO:
//      -Croshair/aiming
//      -Advanced target snap with croshair
//      -Rotate object to match target area orientation
//
// Gives the player the ability to carry and throw items given from spawners
public class PlayerCarryItem : MonoBehaviour
{
    [Tooltip("From how far the player can pick up objects")] [SerializeField] private float interactDistance = 1f;
    [Tooltip("Throwing force of the player")] [SerializeField] private float throwForce = 5f;
    [Tooltip("Defines the upwards component of throwforce")] [SerializeField] private float throwForceUp = 1f;

    private GameObject movableObject;
    private PlayerInput input;
    private CharacterController controller;
    private bool carrying = false;
    private InteractionHint hint;



    // Start is called before the first frame update
    void Start()
    {
        input = GetComponentInParent<PlayerInput>();
        controller = gameObject.GetComponent<CharacterController>();

    }

    /// <summary>
    /// Callback for pickup input event
    /// </summary>
    /// <param name="context">input event</param>
    public void OnPickUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //if player taps interact while carrying an item -> throws item
            //if player taps interact while not carrying an item -> checks if there is a spawner in front
            if (carrying)
            {
                ThrowObject();
            }
            else
            {
                CheckForMovabeObjectSpawner();
            }
            //if the spawner was found and it succesfully gave an object -> player picks up the object
            if (movableObject != null)
            {
                PickUpObject();
            }

        }
    }

    /// <summary>
    /// Throws the object that player is carrying with given force
    /// </summary>
    private void ThrowObject()
    {
        //Calculating the throwforce and throwing the object
        Vector3 forceOfThrow = transform.forward * throwForce + transform.up * throwForceUp;
        movableObject.GetComponent<Rigidbody>().AddForce(forceOfThrow, ForceMode.Impulse);

        //Setting all the neccessary parameters for the object once it's thrown and performing neccessary "resets"
        movableObject.GetComponent<Rigidbody>().useGravity = true;
        movableObject.transform.SetParent(null);
        movableObject.GetComponent<BoxCollider>().enabled = true;
        movableObject = null;
        carrying = false;
        input.actions.FindAction("Jump").Enable();
    }

    /// <summary>
    /// Picks up the movable object and sets neccessary parameters
    /// </summary>
    private void PickUpObject()
    {
        movableObject.GetComponent<Rigidbody>().useGravity = false;
        Vector3 targetPos = transform.position + transform.forward * 0.5f;
        movableObject.transform.position = targetPos;
        movableObject.transform.SetParent(transform);
        carrying = true;
        input.actions.FindAction("Jump").Disable();
    }

    /// <summary>
    /// Checks if there is a movable object spawner in front of the player
    /// </summary>
    private void CheckForMovabeObjectSpawner()
    {
        // Checking if the object hit with the ray is also a movable object spawner
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward + Vector3.down), out RaycastHit hit, interactDistance)
            && hit.collider.gameObject.CompareTag("movable_object_spawner") && controller.isGrounded)
        {
            //Getting the object from the spawner
            movableObject = hit.collider.gameObject.GetComponent<MovableObjectSpawner>().GetMovableObject();
            PickUpObject();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward + Vector3.down), out RaycastHit hit, interactDistance)
            && hit.collider.gameObject.CompareTag("movable_object_spawner") && controller.isGrounded && !carrying)
        {
            hint = hit.collider.gameObject.GetComponentInChildren<InteractionHint>();
            hint.Activate();
        }
        else if (hint != null)
        {
            hint.DeActivate();
        }

        //Checking if there is a target area below the movable object while the player is carrying it
        //if said target is found removes the object from the player and snaps it in the middle of the target area.
        if (carrying && Physics.Raycast(movableObject.transform.position, transform.TransformDirection(Vector3.down), out RaycastHit hit1, 5f)
            && hit1.collider.gameObject.CompareTag("target_area"))
        {
            movableObject.transform.SetParent(null);
            movableObject.GetComponent<BoxCollider>().enabled = true;
            input.actions.FindAction("Jump").Enable();
            movableObject.GetComponent<Rigidbody>().useGravity = true;
            carrying = false;
            movableObject = null;
        }
    }

}
