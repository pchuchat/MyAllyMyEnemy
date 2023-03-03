using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // katsotaan kohta onko tarpeellinan

// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// By: Parviainen P
// TODO: 
// - Haba lifts an object by repeatedly clicking the interact-button

public class LiftObject : MonoBehaviour
{
    // Public attributes (visible in Unity)    
    public float force; //Kuinka paljon esinettä nostetaan kerralla
    public float distance; //kuinka lähellä nostettava esine saa olla (TODO: tarkista onko nämä metreissä?)
    // Onko float näihin fiksu vai kannattaisiko olla joku muu?

    // Private attributes
    private CharacterController controller;
    private bool canLift = false; //a bool to see if you can up the target item
    private GameObject target; // Kohde, mikä halutaan nostaa

    // Poistettavia jos ei käytetäkkään
    float forcecontrol; // ei käytetä atm missään mutta ohjeessa updatessa nostovoima kerrottiin vielä tällä

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    public void OnLift(InputAction.CallbackContext context)
    {

        // Mieluummin jonnekin muualle? Muuten tarkistaa joka napinpainalluksella. Pelaajalle state?
        // Kömpelö eka ratkaisu? -> Jos target null -> Aliohjelmaan joka laittaa targettiin lähellä olevan esineen
        // ===> Samalla kun laitetaan targettiin esine tarkistetaan onko oikea tag?
        // Ongelmia: Jos on aiemmin nostettu toista esinettä target ei ole null --> Nullaa target kun esine pudotetaan?
        //           Samalla myös nullataan canlift?
        // Plussia: Ei tarvitsisi joka napinpainalluksella tarkistaa etäisyyttä gameobjectiin, objectin tagia ja onko canlift true
        //          Tarkistettaisiin vain onko target TAI canlift null/false (vai molemmat hmm)

        // Todo: Rajaa distance niin, että se ottaa vain pelaajan edestä
        if (2 < distance) //todo: pelaajan etäisyys objectiin < distance
        {
            //target = lähellä oleva gameobject --> hae lähellä olevat gameobjectit? Ellei voi ottaa if-lauseen omaa
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

