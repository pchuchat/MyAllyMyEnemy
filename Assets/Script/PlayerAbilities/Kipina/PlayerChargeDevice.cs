using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// �GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// By: Parviainen P
// Edited: Kettunen J
// Kipin� charges the device in front and activates it.
// Activation of devices cause some action in the game. The action depends on what script is attached to the device.

public class PlayerChargeDevice : MonoBehaviour
{
    // Attributes visible in Unity
    [Tooltip("How far away the object can be from Kipin� to be charged")] [SerializeField] private float distance = 1f;

    // Sounds
    private AudioSource audioSource; // Audiosource for the sounds below
    [Tooltip("The sound Kipin� makes when charging devices")] [SerializeField] private AudioClip chargeSound;

    private CharacterController controller; // Playercharacter
    private bool canCharge;                 // Wether or not the device in front can be activated
    private GameObject device;              // The device in front of the player
    private InteractionHint hint;

    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Activates the device in front of the player and calls the script attached to the devie
    /// Also plays the sound of the charge-ability
    /// Is called when interact button is clicked
    /// </summary>
    /// <param name="context">Interact button click</param>
    public void OnCharge(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(device == null)
            {
                canCharge = GetObjectInfront();
            }           
            if (canCharge == true)
            {
                device.tag = "charged";
                device.GetComponent<DeviceCharged>().enabled = true;
                device.GetComponent<DeviceCharged>().ActivateDevice();
                audioSource.clip = chargeSound;
                audioSource.Play();
                canCharge = false;
                device = null;
            }
        }           
    }

    /// <summary>
    /// Checks if there is an object in front of the player within a spesific distance and returns a bool value. 
    /// If there is one, then checks if the object has the tag "noCharge". If the object that is close has
    /// the correct tag then sets it to be the device-gameobject;
    /// The object has to have a rigidbody.
    /// </summary>
    /// <returns>False if there is no object in front of the player
    ///          False if the object in front of the player doesn't have the correct tag
    ///          True if a gameobject is close enough and has the correct tag</returns>
    private bool GetObjectInfront()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, distance) && controller.isGrounded)
        {
            GameObject hitGameobject = hit.transform.gameObject;

            if (hitGameobject.CompareTag("noCharge"))
            {
                device = hitGameobject;
                return true;
            }
            return false;
        }
        else
        {
            return false;
        }
    }
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), 
            out RaycastHit hit, distance) && hit.collider.gameObject.CompareTag("noCharge") && controller.isGrounded)
        {
            hint = hit.collider.gameObject.GetComponentInChildren<InteractionHint>();
            hint.Activate();
        }
        else if (hint != null)
        {
            hint.DeActivate();
        }
    }
}
