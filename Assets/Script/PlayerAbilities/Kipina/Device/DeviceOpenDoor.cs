using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// By: Parviainen P
// Lifts door up and keeps it there. Drops the door when both players have gone through it to the other side

public class DeviceOpenDoor : MonoBehaviour
{
    //Atributes visible in unity
    [Tooltip("How high the target-position is from the start-position")] [SerializeField] private float height = 3f;
    [Tooltip("How much child-object is moved per frame")] [SerializeField] private float force = 0.02f;

    // Sounds
    private AudioSource audioSource; // Audiosource for the sound below
    [Tooltip("The sound the device makes when active (Plays on repeat until device is no longer active)")] [SerializeField] private AudioClip chargedDeviceSound;
    [Tooltip("The sound the door makes when it is dropped and hits the floor")] [SerializeField] private AudioClip droppedDoorSound;

    //Private attributes
    private GameObject moveable;    // The object that device moves when activated (= child-object)
    private Rigidbody rb;           // The rigidbidy of the moveable-object
    private Vector3 startP;
    private Vector3 targetP;        // Target position where object is moved

    /// <summary>
    /// Sets audiosource 
    /// Disables this script
    /// Is called on the first frame when the game is loaded
    /// </summary>
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        transform.GetComponent<DeviceOpenDoor>().enabled = false;
    }

    /// <summary>
    /// Sets values to all needed variables when called
    /// Plays audioclip on repeat
    /// Is called when Kipinä activates the device that this script is attached to
    /// </summary>
    public void ActivateDevice()
    {
        moveable = transform.parent.gameObject.transform.GetChild(1).gameObject;
        if (moveable != null)
        {
            rb = moveable.GetComponent<Rigidbody>();
            rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
            rb.useGravity = false;
            startP = moveable.transform.position;
            targetP = moveable.transform.position;
            targetP.y = (moveable.transform.position.y + height);
            audioSource.clip = chargedDeviceSound;
            audioSource.loop = true;
            audioSource.Play();
        }

    }

    /// <summary>
    /// Sets door's gravity to true and stops audio
    /// Is called from DoorTriggerScript when both players have triggered the script
    /// </summary>
    public void DoorTriggered()
    {
        if (moveable != null)
        {
            rb.useGravity = true;
        }
    }

    /// <summary>
    /// Moves the moveable-object up to a certain height
    /// Plays audio and calls StopScript if door is back on the floor
    /// Is called every frame
    /// </summary>
    void FixedUpdate()
    {
        if (moveable != null)
        {
            rb.transform.position = Vector3.MoveTowards(rb.transform.position, targetP, force);
            if ((Vector3.Distance(rb.transform.position, targetP) < 0.001f) && rb.useGravity == true)
            {
                audioSource.Stop();
            }
                if ((Vector3.Distance(rb.transform.position, startP) < 0.001f) && rb.useGravity == true)
            {
                audioSource.clip = droppedDoorSound;
                audioSource.Play();
                StopScript();
            }
        }  
    }

    /// <summary>
    /// Deactivates the device this script is attached to.
    /// Nulls all variables and changes to them done with this script. Stops script
    /// </summary>
    private void StopScript()
    {
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        transform.tag = transform.GetComponentInChildren<DeviceTargetScript>().WhichTag(); // Asks trigger area which tag it should use
        moveable = null;
        rb = null;
        transform.GetComponent<DeviceOpenDoor>().enabled = false;
        Debug.Log("Hep");
    }
}
