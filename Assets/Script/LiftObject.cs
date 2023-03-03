using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // katsotaan kohta onko tarpeellinan

// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// By: Parviainen P
// TODO: 
// - Haba lifts an object by repeatedly clicking the interact-button
// Huom huom huom muista tarkistaa että esineellä on tag "Nostettava"

public class LiftObject : MonoBehaviour
{
    // Public attributes visible in unity
    private CharacterController controller;

    private bool canLift = false; //a bool to see if you can or cant pick up the item
    public GameObject target; // Kohde, mikä halutaan nostaa
    public float force; //Kuinka paljon esinettä nostetaan kerralla
    float forcecontrol;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    public void onLift(InputAction.CallbackContext context)
    {
        Debug.Log("Nostetaanko?");
        if (string.Equals(target.tag,"liftable"))
        {
            canLift = true;
            Debug.Log("oikea tag!");
        }

        if(canLift == true)
        {
            target.GetComponent<Rigidbody>().AddForce(Vector3.up * force);
            Debug.Log("Nostetaan!");
        }
    }

    private void Update()
    {
        
    }
}

