using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// By: Parviainen P
// Edited: Kettunen J
// Kipinä charges the device in front and activates it.
// Activation of devices cause some action in the game. The action depends on what script is attached to the device.

public class PlayerChargeDevice : MonoBehaviour
{
    // Attributes visible in Unity
    [Tooltip("How far away the object can be from Kipinä to be charged")] [SerializeField] private float distance = 1f;

    // Sounds
    private AudioSource audioSource; // Audiosource for the sounds below
    [Tooltip("The sound Kipinä makes when charging devices")] [SerializeField] private AudioClip chargeSound;

    private CharacterController controller; // Playercharacter
    private GameObject device;              // The device in front of the player
    private InteractableDetection interactor;

    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        interactor = GetComponent<InteractableDetection>();
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
                device = interactor.GetInteractable("noCharge");
                if (device != null)
                {
                    device.tag = "charged";
                    device.GetComponent<DeviceCharged>().enabled = true;
                    device.GetComponent<DeviceCharged>().ActivateDevice();
                    audioSource.clip = chargeSound;
                    audioSource.Play();
                    device = null;
                    interactor.InteractionFinished();
                }
            }
        }           
    }
}
