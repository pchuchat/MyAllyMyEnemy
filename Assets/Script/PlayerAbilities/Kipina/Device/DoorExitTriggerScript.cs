using UnityEngine;

// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// By: Kettunen J
// Finds the players in the scene and checks which way they exited from the trigger, closes door when both have exited the right way


public class DoorExitTriggerScript : MonoBehaviour
{
    private GameObject[] players;
    private bool player1Exited = false;
    private bool player2Exited = false;
    /// <summary>
    /// Called when player exits triggerarea, checks which way the player 
    /// exited and closes the door if they both exited from 
    /// positive z direction as their last exit.
    /// </summary>
    /// <param name="other">Collider of the player that has exited</param>
    private void OnTriggerExit(Collider other)
    {
        //Added for testing with one player
        if (players.Length == 1)
        {
            if (other.name == players[0].name)
            {
                Vector3 exitDirection = transform.InverseTransformDirection(players[0].transform.position - transform.position);
                if (exitDirection.z > 0) transform.parent.GetComponentInParent<DeviceOpenDoor>().DoorTriggered();
            }
        }
        // if 2 players are found makes sure that both have their last exit in the right direction
        if (players.Length == 2)
        {
            if (other.name == players[0].name)
            {
                Vector3 exitDirection = transform.InverseTransformDirection(players[0].transform.position - transform.position);
                if (exitDirection.z > 0) player1Exited = true;
                else player1Exited = false;
            }
            if (other.name == players[1].name)
            {
                Vector3 exitDirection = transform.InverseTransformDirection(players[1].transform.position - transform.position);
                if (exitDirection.z > 0) player2Exited = true;
                else player2Exited = false;
            }
        }
        if (player1Exited && player2Exited)transform.parent.GetComponentInParent<DeviceOpenDoor>().DoorTriggered();
    }
    private void Update()
    {
        players = GameObject.FindGameObjectsWithTag("player");
    }
}
