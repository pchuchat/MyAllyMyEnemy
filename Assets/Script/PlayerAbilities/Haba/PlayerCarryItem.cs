using System.Collections.Generic;
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
    [Header("Carrying attributes")]
    [Tooltip("Throwing force of the player")] [SerializeField] private float throwForce = 5f;
    [Tooltip("Defines the upwards component of throwforce")] [SerializeField] private float throwForceUp = 1f;

    //Player
    private CharacterController controller;     
    private PlayerInput input;
    private bool carrying = false;      //whether the player is carrying an object or not
    private InteractableDetection interactor;

    //Movable object
    private GameObject spawner;
    private GameObject movableObject;
    private List<GameObject> targets;

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponentInParent<PlayerInput>();
        controller = gameObject.GetComponent<CharacterController>();
        interactor = GetComponent<InteractableDetection>();
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
                spawner = interactor.GetInteractable("movable_object_spawner");
                if (spawner != null)
                {
                    movableObject = spawner.GetComponent<MovableObjectSpawner>().GetMovableObject();
                    targets = spawner.GetComponent<MovableObjectSpawner>().GetTargets();
                    PickUpObject();
                }
            }
            //if the spawner was found and it succesfully gave an object -> player picks up the object
            if (movableObject != null)
            {
                PickUpObject();
            }

        }
    }

    /// <summary>
    /// Picks up the movable object and sets neccessary parameters
    /// </summary>
    private void PickUpObject()
    {
        //Removing gravity from object and making sure it is held in the right orientation
        movableObject.GetComponent<Rigidbody>().useGravity = false;
        movableObject.transform.forward = controller.transform.forward;

        //Calculating and setting the position for carrying item
        Vector3 targetPos = transform.position + transform.forward * (movableObject.transform.localScale.z/2 + transform.localScale.z/2);
        movableObject.transform.position = targetPos;

        movableObject.transform.SetParent(transform);
        carrying = true;
        input.actions.FindAction("Jump").Disable();
        movableObject.GetComponent<MovableObject>().PlayPickUpSound();
    }

    /// <summary>
    /// Throws the object that player is carrying with given force
    /// </summary>
    private void ThrowObject()
    {
        //Playing the sound for throwing item
        movableObject.GetComponent<MovableObject>().PlayThrowSound();

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
        targets = null;
        interactor.InteractionFinished();
    }


    // Update is called once per frame
    void Update()
    {

        //Checking if there is a target area below the movable object while the player is carrying it
        //if said target is found removes the object from the player and snaps it in the middle of the target area.
        if (carrying && Physics.Raycast(movableObject.transform.position, transform.TransformDirection(Vector3.down), out RaycastHit hit, 5f)
            && hit.collider.gameObject.CompareTag("target_area") && targets.Contains(hit.collider.gameObject))
        {
            movableObject.transform.SetParent(null);
            movableObject.GetComponent<BoxCollider>().enabled = true;
            input.actions.FindAction("Jump").Enable();
            movableObject.GetComponent<Rigidbody>().useGravity = true;
            carrying = false;
            movableObject = null;
            interactor.InteractionFinished();
        }
    }

}
