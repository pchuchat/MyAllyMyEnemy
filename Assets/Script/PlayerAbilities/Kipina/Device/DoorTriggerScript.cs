using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// By: Parviainen P
// Sees if player enters trigger-area. If both players have entered the area calls DeviceOpenDoor and tells it to stop

public class DoorTriggerScript : MonoBehaviour
{
    private bool kipinaTrigger;
    private bool habaTrigger;

    /// <summary>
    /// If something enters trigger area checks if it is one of the players. If it is one of the players changes the trigger for the 
    /// corresponding player to be true
    /// </summary>
    /// <param name="other">Collider of the player that hit trigger-area</param>
    private void OnTriggerExit(Collider other)
    {
        if(other.name == "Kipina3D(Clone)")
        {
            kipinaTrigger = true;
        }
        if (other.name == "Haba3D(Clone)")
        {
            habaTrigger = true;
        }        
        //GetComponentInParent<DeviceOpenDoor>().DoorTriggered(); // Tämä on vielä tässä väliaikaisesti jos ei toimikkaan 2 pelaajalla
    }

    /// <summary>
    /// When called from other trigger area sets the trigger of the corresponding player to false
    /// DoorExitTrigger calls this with either "Haba" or "Kipina" NOT THE ACTUAL PLAYER NAME
    /// </summary>
    /// <param name="player">String with the player's name that exited trigger area</param>
    public void DoorExit(string player)
    {
        switch (player)
        {
            case "Kipina":
                kipinaTrigger = false;
                break;

            case "Haba":
                habaTrigger = false;
                break;
        }
    }

    /// <summary>
    /// If both player triggers are true calls DeviceOpenDoor and tells it that the door was triggered
    /// </summary>
    private void FixedUpdate()
    {
        if (kipinaTrigger == true && habaTrigger == true)
        {
            GetComponentInParent<DeviceOpenDoor>().DoorTriggered();
        }
    }
}
