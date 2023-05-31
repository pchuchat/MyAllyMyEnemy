using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Creator: PC, Phatchanon Chuchat
// TODO:
//  
//
// 


public class StickyPlatform : MonoBehaviour
{
    // Called when a collision occurs
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object has the "player" tag
        if (collision.gameObject.tag == "player")
        {
            // Set the parent of the player object to this platform
            collision.gameObject.transform.SetParent(transform);
        }
    }

    // Called when a collision ends
    private void OnCollisionExit(Collision collision)
    {
        // Check if the colliding object has the "player" tag
        if (collision.gameObject.tag == "player")
        {
            // Remove the parent of the player object
            collision.gameObject.transform.SetParent(null);
        }
    }
}