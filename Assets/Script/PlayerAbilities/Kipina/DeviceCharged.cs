using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DeviceCharged : MonoBehaviour
{
    private GameObject moveable;
    private Rigidbody rb;
    private float force = 0.02f;
    private Vector3 targetP;
    private Vector3 startP;
    private int trips = 0;

    // Sounds
    private AudioSource audioSource; // Audiosource for the sounds below
    [Tooltip("The sound the device makes when active")] [SerializeField] private AudioClip chargedDeviceSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Sets values to all needed variables when called
    /// Is called when Kipinä activates the device that the script is attached to
    /// </summary>
    public void ActivateDevice()
    {
        moveable = transform.GetChild(0).gameObject;
        if (moveable != null)
        {            
            rb = moveable.gameObject.GetComponent<Rigidbody>();
            rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
            rb.useGravity = false;
            startP = moveable.transform.position;
            targetP = moveable.transform.position;
            targetP.y = (moveable.transform.position.y + 3);
            audioSource.clip = chargedDeviceSound;
            audioSource.loop = true;
            audioSource.Play();
        }

    }

    /// <summary>
    /// Nulls all variables and changes to them done with this script. Stops script
    /// </summary>
    private void stopScript()
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
            if (true && (Vector3.Distance(rb.transform.position, targetP) < 0.001f))
            {
                float temp = targetP.y;
                targetP.y = startP.y;
                startP.y = temp;
                trips++;
            }
            if (trips == 4)
            {
                stopScript();
            }
        }
        
    }
}
