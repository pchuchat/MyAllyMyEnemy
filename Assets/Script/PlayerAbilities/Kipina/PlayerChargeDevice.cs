using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// By: Parviainen P
//
// TODO:
// - Kipinä can charge devices that activate some action in the level

public class PlayerChargeDevice : MonoBehaviour
{
    // Attributes visible in Unity
    [Tooltip("How far away the object can be from Kipinä to be charged")] [SerializeField] private float distance = 1f;

    // Sounds
    private AudioSource audioSource; // Audiosource for the sounds below
    [Tooltip("The sound Haba makes when lifting object")] [SerializeField] private AudioClip chargeSound;

    private CharacterController controller; // Playercharacter
    private bool canCharge;
    private GameObject device;

    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
    }

    public void OnCharge(InputAction.CallbackContext context)
    {
        canCharge = GetObjectInfront();
        if(canCharge == true)
        {

        }
    }

    private bool GetObjectInfront()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, distance) && controller.isGrounded)
        {
            GameObject hitGameobject = hit.transform.gameObject;

            if (hitGameobject.CompareTag("noCharge"))
            {
                Debug.Log("Chargeable device in front of player");
                device = hitGameobject;
                return true;
            }
            Debug.Log("Some other object in front of player");
            return false;
        }
        else
        {
            Debug.Log("No object in front of player");
            return false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
