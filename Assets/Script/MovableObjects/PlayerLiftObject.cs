
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // katsotaan kohta onko tarpeellinan
using System.Collections;

// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// By: Parviainen P
// TODO: 
// - Haba lifts an object to a certain position and keeps it there by repeatedly clicking the interact-button.
// If the player stops clicking the button, after a certain time Haba will drop the object.

public class PlayerLiftObject : MonoBehaviour
{
    // Attributes visible in Unity    
    [Tooltip("How much the Haba lifts an object per buttonpress")] [SerializeField] private float force = 0.2f;
    [Tooltip("How far away the object can be from Haba to lift")] [SerializeField] private float distance = 1f;
    [Tooltip("How high Haba can lift an object")] [SerializeField] private float stopAtHeight = 1.5f;
    [Tooltip("How long until Haba drops the object")] [SerializeField] private int time = 20;

    // Sounds
    private AudioSource audioSource; // Audiosource for the sounds below
    [Tooltip("The sound Haba makes when lifting object")] [SerializeField] private AudioClip liftSound;
    [Tooltip("The sound Haba makes when drops lifted object")] [SerializeField] private AudioClip dropSound;
    [Tooltip("The sound the dropped object makes when hitting the ground")] [SerializeField] private AudioClip objectGroundSound;

    // Private attributes    
    private bool canLift; // a bool to see if you can up the target item
    private GameObject target; // The object Haba is lifting
    private Vector3 maxHeight; // The height where object stops lifting
    //private Vector3 ogHeight; // The original position of target object     //Tämä otettiin pois kun target sijainnin sijaan katsotaan nyt liikkuuko target
    private int timer;
    private CharacterController controller;
    private PlayerInput input; // Used to disable movement and jump while lifting

    private Vector3 movementUp;
    private Rigidbody rb;

    private float speed;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();        
    }

    /// <summary>
    /// Lifts the target object a certain amount per button press and stops at a certain height
    /// </summary>
    /// <param name="_1">Interact button click(Unused parameter that can not be removed)</param>
    public void OnLift(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (target == null)
            {
                canLift = GetObjectInfront();
            }

            if (canLift == true)
            {
                input.actions.FindAction("Movement").Disable();
                input.actions.FindAction("Jump").Disable();
                timer = time;
                audioSource.clip = liftSound;
                audioSource.Play();

                
                movementUp = new Vector3 (0, force, 0);
                rb.AddForce(movementUp - rb.velocity, ForceMode.VelocityChange);
                //target.transform.Translate(0, force, 0);

                if (target.transform.position.y > maxHeight.y)
                {
                    rb.position = maxHeight;
                    rb.constraints = RigidbodyConstraints.FreezeAll;
                    //gamepad.SetMotorSpeeds(0.123f, 0.234f);
                    //target.transform.position = maxHeight;
                    target.tag = "atMaxHeight";
                }
            }
        }   
    }

    /// <summary>
    /// Checks if there is an object in front of the player within a spesific distance and returns a bool value. 
    /// If there is one, then checks if the object has the tag "liftable". If the object that is close has
    /// the correct tag then sets it to be the target-gameobject;
    /// The object has to have a rigidbody.
    /// </summary>
    /// <returns>False if there is no object in front of the player
    ///          False if the object in front of the player doesn't have the correct tag
    ///          True if a gameobject is close enough and has the correct tag</returns>
    private bool GetObjectInfront()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, distance) && controller.isGrounded) //TODO ray vain eteen
        {                                                                         
            GameObject hitGameobject = hit.transform.gameObject;

            if (hitGameobject.CompareTag("liftable"))
            {
                input = GetComponent<PlayerInput>();
                target = hitGameobject;
                rb = target.gameObject.GetComponent<Rigidbody>();
                rb.useGravity = false;
                rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
                //ogHeight = target.transform.position; //Tämä otettiin pois kun target sijainnin sijaan katsotaan nyt liikkuuko target
                maxHeight = target.transform.position;                      
                maxHeight.y = (target.transform.position.y) + stopAtHeight;
                return true;
            }

            return false;
        }
        else
        {
            return false;
        }
    }

    private void FixedUpdate()
    {
        timer--;
        speed = rb.velocity.magnitude;

        if (target != null)
        {
            if (timer == 0)
            {
                rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
                rb.useGravity = true;
                canLift = false;
                controller.Move(transform.forward*-0.5f);
                audioSource.clip = dropSound;
                audioSource.Play();
            }
            if (timer <= 0)
            {
                //if (Vector3.Distance(target.transform.position, ogHeight) <= 0.2f) // TÄÄ PAREMMAKSI!!! Nyt esine noisee koko ajan! Liian pieni ja haba jää jumiin
                if(speed <= 0.01 && timer <= -5) // tämäkään ei toimi
                {
                    input.actions.FindAction("Movement").Enable();
                    input.actions.FindAction("Jump").Enable();
                    rb.constraints = RigidbodyConstraints.FreezeAll;
                    target.tag = "liftable";
                    target = null;
                    rb = null;
                    audioSource.clip = objectGroundSound;
                    audioSource.Play();

                }
            }
        }       
    }
}

