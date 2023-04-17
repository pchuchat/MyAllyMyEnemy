using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceOpenDoor : MonoBehaviour
{
    //Atributes visible in unity
    //[Tooltip("How many times activated device moves child-object")] [SerializeField] private int times = 4;
    [Tooltip("How high the target-position is from the start-position")] [SerializeField] private float height = 3f;
    [Tooltip("How much child-object is moved per frame")] [SerializeField] private float force = 0.02f;

    // Sounds
    private AudioSource audioSource; // Audiosource for the sound below
    [Tooltip("The sound the device makes when active (Plays on repeat until device is no longer active)")] [SerializeField] private AudioClip chargedDeviceSound;

    //Private attributes
    private GameObject moveable;    // The object that device moves when activated (= child-object)
    private GameObject doorTrigger;
    private Rigidbody rb;           // The rigidbidy of the moveable-object
    private Vector3 targetP;        // Target position where object is moved
    private Vector3 startP;         // Start position for the object
    private int trips = 0;          // How many trips the object has made between positions so far

    /// <summary>
    /// Sets audiosource 
    /// Disables this script
    /// Is called on the first frame when the game is loaded
    /// </summary>
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        transform.GetComponent<DeviceCharged>().enabled = false;
    }

    /// <summary>
    /// Sets values to all needed variables when called
    /// Plays audioclip on repeat
    /// Is called when Kipinä activates the device that this script is attached to
    /// </summary>
    public void ActivateDevice()
    {
        moveable = transform.GetChild(0).gameObject;
        if (moveable != null)
        {
            rb = moveable.GetComponent<Rigidbody>();
            rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
            rb.useGravity = false;
            startP = moveable.transform.position;
            targetP = moveable.transform.position;
            targetP.y = (moveable.transform.position.y + height);
            doorTrigger = moveable.transform.GetChild(0).gameObject;
            audioSource.clip = chargedDeviceSound;
            audioSource.loop = true;
            audioSource.Play();
        }

    }

    /// <summary>
    /// Deactivates the device this script is attached to.
    /// Nulls all variables and changes to them done with this script. Stops script
    /// </summary>
    private void StopScript()
    {
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        transform.tag = "noCharge";
        trips = 0;
        moveable = null;
        rb = null;
        audioSource.Stop();
        transform.GetComponent<DeviceCharged>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (moveable != null)
        {
            if(Vector3.Distance(rb.transform.position, targetP) < 0.001f)
            {
                StopScript();
            }
        }
    }

    /// <summary>
    /// Moves the moveable-object front and back between it's start position and target position.
    /// Stops when object has travelled the distance 4 times.
    /// Is called every frame
    /// </summary>
    void FixedUpdate()
    {
        if (moveable != null)
        {
            rb.transform.position = Vector3.MoveTowards(rb.transform.position, targetP, force);
            /*if (true && (Vector3.Distance(rb.transform.position, targetP) < 0.001f)) // Hups onkohan tässä jotain kesken koska mitä toi true tossa on???
            {
                float temp = targetP.y;
                targetP.y = startP.y;
                startP.y = temp;
                trips++;
            }*/
        }

    }
}
