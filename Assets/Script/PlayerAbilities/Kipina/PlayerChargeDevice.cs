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

    // Sounds
    private AudioSource audioSource; // Audiosource for the sounds below
    [Tooltip("The sound Kipinä makes when charging devices")] [SerializeField] private AudioClip chargeSound;

    private GameObject device;              // The device in front of the player
    private InteractableDetection interactor;

    void Start()
    {
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
            if (audioSource == null || interactor == null)
            {
                Debug.LogError("Audio source or interactor is null!");
                return;
            }

            if (device == null)
            {
                device = interactor.GetInteractable("noCharge");
                if (device != null)
                {
                    switch (device.name)
                    {
                        case "ChargeableDevice":
                            device.tag = "charged";
                            device.GetComponent<DeviceCharged>().enabled = true;
                            device.GetComponent<DeviceCharged>().ActivateDevice();
                            audioSource.clip = chargeSound;
                            audioSource.Play();
                            device = null;
                            interactor.InteractionFinished();
                            break;

                        case "kapine_kauko":
                            device.tag = "charged";
                            device.GetComponent<DeviceOpenDoor>().enabled = true;
                            device.GetComponent<DeviceOpenDoor>().ActivateDevice();
                            audioSource.clip = chargeSound;
                            audioSource.Play();
                            device = null;
                            interactor.InteractionFinished();
                            break;
                    }
                }
            }
        }
    }
}
