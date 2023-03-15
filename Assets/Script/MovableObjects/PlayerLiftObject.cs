
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
    [Tooltip("How high Haba can lift an object")] [SerializeField] private float stopAtHeight = 1f;
    [Tooltip("How long until Haba drops the object")] [SerializeField] private int time = 20;

    // Private attributes    
    private bool canLift; // a bool to see if you can up the target item
    private GameObject target; // The object Haba is lifting
    private Vector3 maxHeight;// The height where object stops lifting
    private Vector3 ogHeight;
    private int timer;
    private CharacterController controller;

    PlayerInput input; // todo


    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    /// <summary>
    /// Lifts the target object and stops at a certain height
    /// </summary>
    /// <param name="_1">Interact button click(Unused parameter that can not be removed)</param>
    public void OnLift(InputAction.CallbackContext _1)
    {

        if (target == null)
        {
            canLift = GetObjectInfront();
        }
        
        if(canLift == true)
        {
            input.actions.FindAction("Movement").Disable();
            input.actions.FindAction("Jump").Disable();
            timer = time;
            target.transform.Translate(0, force, 0);

            if (target.transform.position.y > maxHeight.y)
            {
                target.transform.position = maxHeight;
                target.tag = "atMaxHeight";
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
                target.GetComponent<Rigidbody>().useGravity = false;
                ogHeight = target.transform.position;
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

        if (target != null)
        {
            if (timer <= 0)
            {
                target.GetComponent<Rigidbody>().useGravity = true;
                canLift = false;

                if (Vector3.Distance(target.transform.position, ogHeight) <= 0.1f)
                {
                    input.actions.FindAction("Movement").Enable();
                    input.actions.FindAction("Jump").Enable();
                    target.tag = "liftable";
                    target = null;
                }
            }
        }       
    }
}

