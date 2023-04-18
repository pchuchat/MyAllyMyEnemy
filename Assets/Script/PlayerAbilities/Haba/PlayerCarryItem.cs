using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
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
    private readonly Collider[] possibleTargets = new Collider[3];
    private int numFound;


    // Start is called before the first frame update
    void Start()
    {
        input = GetComponentInParent<PlayerInput>();
        controller = gameObject.GetComponent<CharacterController>();
        interactor = GetComponent<InteractableDetection>();
        randomizer = GetComponent<RandomSoundPlayer>();
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
                    movableObjectGO = spawner.GetComponent<MovableObjectSpawner>().GetMovableObject();
                    movableObject = movableObjectGO.GetComponent<MovableObject>();
                    targets = spawner.GetComponent<MovableObjectSpawner>().GetTargets();
                    PickUpObject();
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
        Vector3 targetPos = transform.position + transform.forward * (movableObjectGO.transform.localScale.z/2 + transform.localScale.z/2 +0.1f);
        movableObjectGO.transform.position = targetPos;

        movableObjectGO.transform.SetParent(transform);
        input.actions.FindAction("Jump").Disable();
        simulation = movableObjectGO.GetComponent<PathSimulation>();
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
        simulation.Terminate();
        //Playing the sound for throwing item

        //Adding force for the object
        movableObjectRb.AddForce(force * movableObjectRb.mass, ForceMode.Impulse);

        //Setting all the neccessary parameters for the object once it's thrown and performing neccessary "resets"
        movableObjectGO.GetComponent<Rigidbody>().useGravity = true;
        movableObjectGO.transform.SetParent(null);
        movableObjectGO.GetComponent<BoxCollider>().enabled = true;
        movableObjectGO = null;
        carrying = false;
        input.actions.FindAction("Jump").Enable();
        targets = null;
        movableObject = null;
        interactor.InteractionFinished();
    }
    // Update is called once per frame
    void Update()
    {
        //Checking if there is a target area below the movable object while the player is carrying it
        //if said target is found throws the object at needed force to reach center of target area.
        if (carrying && Physics.Raycast(movableObjectGO.transform.position, transform.TransformDirection(Vector3.down), out RaycastHit hit, 5f)
            && hit.collider.gameObject.CompareTag("target_area") && targets.Contains(hit.collider.gameObject))
        {
            force = simulation.CalculateThrowingForce(hit.collider.gameObject.transform.position, throwAngle);
            ThrowObject();
        }
    }
    private void FixedUpdate()
    {
        if (carrying)
        {
            CheckForTargets();
            force = simulation.CalculateThrowingForce(aimPoint, throwAngle);
            simulation.SimulatePath(movableObjectGO, force);
        }
    }
    /// <summary>
    /// Checks if there are targets withing snapping distance of the aimpoing and sets aimpoint accordingly
    /// </summary>
    private void CheckForTargets()
    {
        aimPoint = movableObjectGO.transform.position + transform.forward * throwdistance;
        aimPoint.y = transform.position.y - transform.localScale.y / 2;
        numFound = Physics.OverlapSphereNonAlloc(aimPoint, snapRadius, possibleTargets, targetMask);

        if (numFound > 0 && targets.Contains(possibleTargets[0].gameObject))
        {
            aimPoint = possibleTargets[0].gameObject.transform.position;
        }
    }
}
