
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // katsotaan kohta onko tarpeellinan
using System.Collections;

// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// By: Parviainen P
// TODO: 
// - Haba lifts an object by repeatedly clicking the interact-button

public class LiftObject : MonoBehaviour
{
    // Public attributes (visible in Unity)    
    public float force; //Kuinka paljon esinett‰ nostetaan kerralla
    public float distance; //kuinka l‰hell‰ nostettava esine saa olla (TODO: tarkista onko n‰m‰ metreiss‰?)
    // Onko float n‰ihin fiksu vai kannattaisiko olla joku muu?

    // Private attributes
    private CharacterController controller;
    private bool canLift = false; //a bool to see if you can up the target item
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
        // Kˆmpelˆ eka ratkaisu? -> Jos target null -> Aliohjelmaan joka laittaa targettiin l‰hell‰ olevan esineen
        // ===> Samalla kun laitetaan targettiin esine tarkistetaan onko oikea tag?
        // Ongelmia: Jos on aiemmin nostettu toista esinett‰ target ei ole null --> Nullaa target kun esine pudotetaan?
        //           Samalla myˆs nullataan canlift?
        // Plussia: Ei tarvitsisi joka napinpainalluksella tarkistaa et‰isyytt‰ gameobjectiin, objectin tagia ja onko canlift true
        //          Tarkistettaisiin vain onko target TAI canlift null/false (vai molemmat hmm)

        // Todo: Rajaa distance niin, ett‰ se ottaa vain pelaajan edest‰
        if (2 < distance) //todo: pelaajan et‰isyys objectiin < distance
        {
            //target = l‰hell‰ oleva gameobject --> hae l‰hell‰ olevat gameobjectit? Ellei voi ottaa if-lauseen omaa
        }

        target = GameObject.FindWithTag("liftable");

        Debug.Log("Nostetaanko?");
        if (string.Equals(target.tag,"liftable"))
        {
            canLift = true;
            Debug.Log("oikea tag!");
        }

        if(canLift == true)
        {
            //target.GetComponent<Rigidbody>().AddForce(Vector3.up * force);
            target.transform.Translate(1, 1, 1); //Nyt vaan siirt‰‰ sit‰ jotenkin lol
            Debug.Log("Nostetaan!");
        }
    }

    private void Update()
    {

    }
}

