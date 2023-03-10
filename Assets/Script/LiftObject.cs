
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // katsotaan kohta onko tarpeellinan
using System.Collections;

// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// By: Parviainen P
// TODO: 
// - Haba lifts an object to a certain position and keeps it there by repeatedly clicking the interact-button.
// If the player stops clicking the button, after a certain time Haba will drop the object.

public class LiftObject : MonoBehaviour
{
    // Public attributes (visible in Unity)    
    public float force; // How much the object is lifted with each click
    public float distance; // How far away the object can be from Haba
    public float stopAtHeight; // How high Haba can lift an object
    public int time; // How long without clicking until Haba drops the object

    // Private attributes    
    private bool canLift; // a bool to see if you can up the target item
    private GameObject target; // The object Haba is lifting
    private Vector3 maxHeight;// The height where object stops lifting
    private Vector3 ogHeight;
    private Vector3 rayDirection = new Vector3(0, 0, 1); // ...kääntyykö tämä pelaajan mukana? TODO tarkista kun päivitetty liikkumisscript
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
    /// <param name="context"></param>
    public void OnLift(InputAction.CallbackContext context)
    {
        if(target == null)
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
        RaycastHit hit;

        if (Physics.Raycast(transform.position, rayDirection, out hit, distance) && controller.isGrounded) 
        {                                                                         
            GameObject hitGameobject = hit.transform.gameObject;

            if (hitGameobject.tag == "liftable")
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

    /// <summary>
    /// Counts time (frames) and drops the object if the player doesn't use the interact button again before timer runs out
    /// </summary>
    private void Update()
    {
        timer--;
        
    }

    private void FixedUpdate()
    {
        if (timer <= 0)
        {           
            target.GetComponent<Rigidbody>().useGravity = true;
            canLift = false;

            if (target.transform.position.y <= ogHeight.y) //TODO: korjaa tämä!!!
            {
                input.actions.FindAction("Movement").Enable();
                input.actions.FindAction("Jump").Enable();
                target.tag = "liftable";
                target = null;
            }            
        }
    }
}

