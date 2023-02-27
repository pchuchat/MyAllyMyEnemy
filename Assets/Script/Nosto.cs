using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // katsotaan kohta onko tarpeellinan

// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Tekijä: Parviainen P
// TODO: 
// - Haba nostaa esineen ränkyttämällä interact näppäintä
// Huom huom huom muista tarkistaa että esineellä on tag "Nostettava"

public class Nosto : MonoBehaviour
{
    private CharacterController controller;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    public void onNosto(InputAction.CallbackContext context)
    {
        Debug.Log("Nostetaan!");
    }
}
