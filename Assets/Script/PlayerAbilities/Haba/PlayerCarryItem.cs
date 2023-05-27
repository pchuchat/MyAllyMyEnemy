using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
// �GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Creator: Kettunen. J
//
// Gives the player the ability to carry and throw items given from spawners
public class PlayerCarryItem : MonoBehaviour
{
    [Header("Throwing attributes")]
    [Tooltip("Throwing distance")] [SerializeField] private float throwdistance = 5f;
    [Tooltip("Defines the upwards angle of the throw")] [SerializeField] private float throwAngle = 45;
    [Tooltip("Layer for target areas")] [SerializeField] private LayerMask targetMask;
    [Tooltip("Radius of targetsnapping area around the aimPoint")] [SerializeField] private float snapRadius = 0.5f;
    [Tooltip("Distance to snapped target when automatic throw is initiated")] [SerializeField] private float autoThrowDistance = 5f;
    [Tooltip("Crosshairprefab for aiming throws")][SerializeField] private GameObject crosshairPrefab;


    [Header("Sounds")]
    [Header("Pickup")]
    [Tooltip("Pickup sounds for light items")] [SerializeField] private List<AudioClip> lightPickupSounds;
    [Tooltip("Pickup sounds for heavy items")] [SerializeField] private List<AudioClip> heavyPickupSounds;
    [Header("Throw")]
    [Tooltip("Throw sounds for light items")] [SerializeField] private List<AudioClip> lightThrowSounds;
    [Tooltip("Throw sounds for heavy items")] [SerializeField] private List<AudioClip> heavyThrowSounds;

    //Player
    private CharacterController controller;
    private PlayerInput input;
    private PlayerMovement move;
    private bool carrying = false;      //whether the player is carrying an object or not
    private InteractableDetection interactor;
    private Vector3 force = new(); //Throwingforce needed to reach target at given angle
    private RandomSoundPlayer randomizer;

    //Movable object
    private MovableObject movableObject;
    private GameObject spawner;
    private GameObject movableObjectGO;
    private List<GameObject> targets;
    private PathSimulation simulation;
    private Rigidbody movableObjectRb;

    //Aiming
    private Vector3 aimPoint;


    // Start is called before the first frame update
    void Start()
    {
        input = GetComponentInParent<PlayerInput>();
        controller = gameObject.GetComponent<CharacterController>();
        interactor = GetComponent<InteractableDetection>();
        randomizer = GetComponent<RandomSoundPlayer>();
        move = GetComponent <PlayerMovement>();
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
                move.carrying = false;
            }
            else
            {
                spawner = interactor.GetInteractable("movable_object_spawner");
                if (spawner != null)
                {
                    movableObjectGO = spawner.GetComponent<MovableObjectSpawner>().GetMovableObject();
                    movableObject = movableObjectGO.GetComponent<MovableObject>();
                    targets = spawner.GetComponent<MovableObjectSpawner>().GetTargets();
                    PickUpObject();
                    move.carrying = true;
                }
            }
        }
    }

    /// <summary>
    /// Picks up the movable object and sets neccessary parameters
    /// </summary>
    private void PickUpObject()
    {
        if (movableObject.IsHeavy()) randomizer.Play(heavyPickupSounds);
        else randomizer.Play(lightPickupSounds);
        //Removing gravity from object and making sure it is held in the right orientation
        movableObjectGO.GetComponent<Rigidbody>().useGravity = false;
        movableObjectGO.transform.forward = controller.transform.forward;

        //Calculating and setting the position for carrying item
        Vector3 targetPos = transform.position + transform.forward * (transform.localScale.z/2 + movableObjectGO.transform.localScale.z/2) + transform.up * (transform.localScale.y + 0.2f);
        movableObjectGO.transform.position = targetPos;

        movableObjectGO.transform.SetParent(transform);
        input.actions.FindAction("Jump").Disable();
        simulation = movableObjectGO.GetComponent<PathSimulation>();
        simulation.SetCrosshair(crosshairPrefab);
        movableObjectRb = movableObjectGO.GetComponent<Rigidbody>();
        carrying = true;
    }

    /// <summary>
    /// Throws the object that player is carrying with given force
    /// </summary>
    private void ThrowObject()
    {
        if (movableObject.IsHeavy()) randomizer.Play(heavyThrowSounds);
        else randomizer.Play(lightThrowSounds);
        simulation.Enabled = false;
        //Playing the sound for throwing item

        //Adding force for the object
        movableObjectRb.AddForce(force * movableObjectRb.mass, ForceMode.Impulse);
        //Setting all the neccessary parameters for the object once it's thrown and performing neccessary "resets"
        movableObjectGO.GetComponent<Rigidbody>().useGravity = true;
        movableObjectGO.transform.SetParent(null);
        movableObjectGO.GetComponent<BoxCollider>().enabled = true;
        if (simulation.targetLocked)
        {
            movableObjectGO.GetComponent<BoxCollider>().isTrigger = true;
            movableObject.RotateToTarget(simulation.GetLockedTarget());
        }
        movableObjectGO = null;
        carrying = false;
        input.actions.FindAction("Jump").Enable();
        targets = null;
        movableObject = null;
        interactor.InteractionFinished();
    }
    private void FixedUpdate()
    {
        if (carrying)
        {
            //Setting aimpoint and calculating the initial throwforce
            aimPoint = transform.position + transform.forward * throwdistance;
            force = simulation.CalculateThrowingForce(aimPoint, throwAngle);
            //Checking if there was targets within snapping distance of throw
            aimPoint = simulation.SimulatePath(movableObjectGO, force, snapRadius, targets);

            //Calculating the throwforce for actual throw and drawing the aiming line
            force = simulation.CalculateThrowingForce(aimPoint, throwAngle);
            simulation.SimulatePath(movableObjectGO, force);
            simulation.Draw();
            //Autothrowing object if the target is close enough
            if (Vector3.Distance(aimPoint, transform.position) <= autoThrowDistance) ThrowObject();
        }
    }
}
