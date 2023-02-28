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
    private CharacterController controller;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    public void onLift(InputAction.CallbackContext context)
    {
        Debug.Log("Nostetaan!");
    }
}

