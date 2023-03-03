using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // katsotaan kohta onko tarpeellinan

// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// By: Parviainen P
// TODO: 
// - Haba lifts an object by repeatedly clicking the interact-button
// Huom huom huom muista tarkistaa ett‰ esineell‰ on tag "Nostettava"

public class LiftObject : MonoBehaviour
{
    // Public attributes (visible in Unity)    
    public float force; //Kuinka paljon esinett‰ nostetaan kerralla
    public float distance; //kuinka l‰hell‰ nostettava esine saa olla (TODO: tarkista onko n‰m‰ metreiss‰?)

    // Private attributes
    private CharacterController controller;
    private bool canLift = false; //a bool to see if you can or cant pick up the item
    private GameObject target; // Kohde, mik‰ halutaan nostaa

    // Poistettavia jos ei k‰ytet‰kk‰‰n
    float forcecontrol; // ei k‰ytet‰ atm miss‰‰n mutta ohjeessa updatessa nostovoima kerrottiin viel‰ t‰ll‰

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    public void OnLift(InputAction.CallbackContext context)
    {

        // Mieluummin jonnekin muualle? Muuten tarkistaa joka napinpainalluksella. Pelaajalle state?
        // Todo: Rajaa distance niin, ett‰ se ottaa vain pelaajan edest‰
        if (2 < distance) //todo: pelaajan et‰isyys objektiin < distance
        {
            //target = l‰hell‰ oleva gameobject
        }

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

