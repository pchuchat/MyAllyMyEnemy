using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTriggerScript : MonoBehaviour
{
    public bool triggered = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        GetComponentInParent<DeviceOpenDoor>().DoorTriggered();
        Debug.Log("Ovi triggeröity");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
