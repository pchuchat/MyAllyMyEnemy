
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// By: Parviainen P
// Edited: Kettunen J
//  
// Haba lifts an object to a certain position and keeps it there by repeatedly clicking the interact-button.
// If the player stops clicking the button, after a certain time Haba will drop the object.
//
// TODO:
// - Better timer for dropping the object. Current "timer" actually counts frames instead of time
// - Object will not be dropped if anything is underneath it
// - When something is underneatht the dropped object it will act as if it hit the ground -> Sound plays, Haba is able to move and object locks in space
// (This will be fixed when object can not be dropped if something is underneath it)

public class PlayerLiftObject : MonoBehaviour
{
    // Attributes visible in Unity    
    [Tooltip("How much the Haba lifts an object per buttonpress")] [SerializeField] private float force = 0.2f;
    [Tooltip("How high Haba can lift an object")] [SerializeField] private float stopAtHeight = 1.5f;
    [Tooltip("How long until Haba drops the object")] [SerializeField] private int time = 20;

    // Sounds
    private AudioSource audioSource; // Audiosource for the sounds below
    [Tooltip("The sound Haba makes when lifting object")] [SerializeField] private AudioClip liftSound;
    [Tooltip("The sound Haba makes when drops lifted object")] [SerializeField] private AudioClip dropSound;
    [Tooltip("The sound the dropped object makes when hitting the ground")] [SerializeField] private AudioClip objectGroundSound;

    // Private attributes    
    private bool canLift;       // A bool to see if you can lift up the target item
    private GameObject target;  // The object Haba is lifting
    private Rigidbody rb;       // Rigidbody of the target-object
    private Vector3 maxHeight;  // The height where object stops lifting
    private Vector3 movementUp; // Used to give the target upwards force  
    private float speed;        // Speed of the target-object
    private int timer;          // "Timer" that counts if buttonpressing stops (actually counts frames)
    private PlayerInput input; // Used to disable movement and jump while lifting
    private CharacterController controller; // Playercharacter
    private InteractableDetection interactor;

    //This is here just in case. It was removed because now lifting stops if dropped object is no longer moving
    //Earlier lifting stopped if the lifted object was in the same position (or close enough) to it's original position before lifting
    //private Vector3 ogHeight; // The original position of target object
    // This was removed at the same time. This was in GetObjectInFront() where all variables are given values
    //ogHeight = target.transform.position;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        interactor = GetComponent<InteractableDetection>();
    }

    /// <summary>
    /// Lifts the target object a certain amount per button press and stops at a certain height
    /// </summary>
    /// <param name="context">Interact button click</param>
    public void OnLift(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (target == null)
            {
                target = interactor.GetInteractable("liftable");
                if (target != null)
                {
                    input = GetComponent<PlayerInput>();
                    rb = target.GetComponent<Rigidbody>();
                    rb.useGravity = false;
                    rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
                    maxHeight = target.transform.position;
                    maxHeight.y = (target.transform.position.y) + stopAtHeight;
                    input.actions.FindAction("Movement").Disable();
                    input.actions.FindAction("Jump").Disable();
                    canLift = true;
                }
            }

            if (canLift == true)
            {
                timer = time;
                audioSource.clip = liftSound;
                audioSource.Play();

                
                movementUp = new Vector3 (0, force, 0);
                rb.AddForce(movementUp - rb.velocity, ForceMode.VelocityChange);

                if (target.transform.position.y > maxHeight.y)
                {
                    rb.position = maxHeight;
                    rb.constraints = RigidbodyConstraints.FreezeAll;
                    target.tag = "atMaxHeight";
                }
            }
        }   
    }

    private void FixedUpdate()
    {
        timer--;

        if (target != null)
        {
            speed = rb.velocity.magnitude;
            if (timer == 0)
            {
                rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
                rb.useGravity = true;
                canLift = false;
                controller.Move(transform.forward * -0.5f);
                audioSource.clip = dropSound;
                audioSource.Play();
            }           
            if (timer <= 0)
            {
                if(speed <= 0.01 && timer <= -5)
                {
                    input.actions.FindAction("Movement").Enable();
                    input.actions.FindAction("Jump").Enable();
                    rb.constraints = RigidbodyConstraints.FreezeAll;
                    target.tag = "liftable";
                    target = null;
                    rb = null;
                    audioSource.clip = objectGroundSound;
                    audioSource.Play();
                    interactor.InteractionFinished();
                }
            }
        }       
    }
}

