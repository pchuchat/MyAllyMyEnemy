
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // katsotaan kohta onko tarpeellinan
using System.Collections;

// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// By: Parviainen P
// TODO: 
// - Haba lifts an object to a certain position and keeps it there by repeatedly clicking the interact-button.
// If the player stops clicking the button, after a certain time Haba will drop the object.

public class LiftObject : MonoBehaviour
{
    // Public attributes (visible in Unity)    
    public float force; //Kuinka paljon esinett‰ nostetaan kerralla
    public float distance;
    public float stopAtHeight;

    // Private attributes
    
    private bool canLift; //a bool to see if you can up the target item
    private GameObject target; // Kohde, mik‰ halutaan nostaa
    private Vector3 maxHeight;// targetin maksiminostokorkeus
    private Vector3 rayDirection = new Vector3(0, 1, 1); // ...k‰‰ntyykˆ t‰m‰ pelaajan mukana....

    // Poistettavia jos ei k‰ytet‰kk‰‰n
    float forcecontrol; // ei k‰ytet‰ atm miss‰‰n mutta ohjeessa updatessa nostovoima kerrottiin viel‰ t‰ll‰
    private CharacterController controller;
    
    private void Start()
    {
        //controller = gameObject.GetComponent<CharacterController>(); //Poista jos turha
    }

    public void OnLift(InputAction.CallbackContext context)
    {
        
        canLift = GetObjectInfront(); // Olisi kiva jos t‰nne ei tarvisi aina menn‰... Nostokin olisi parempi

        if(canLift == true)
        {
            //target.GetComponent<Rigidbody>().AddForce(Vector3.up * force); t‰m‰ ei toimi mutta eh
            target.transform.Translate(0, force, 0);

            if (target.transform.position.y > maxHeight.y)
            {
                target.transform.position = maxHeight;
                // sitten kun ollaan t‰‰ll‰ pit‰isi tarkistaa kuinka kauan painallusten v‰lill‰ aikaa....
            }
        }
    }

    /// <summary>
    /// Checks if there is an object in front of the player within a spesific distance. 
    /// If there is one, then checks if the object has the tag "liftable". If the object that is close has
    /// the correct tag then sets it to be the target-gameobject;
    /// The object has to have a rigidbody.
    /// </summary>
    /// <returns>False if there is no object in front of the player
    ///          False if the object in front of the player doesn't have the correct tag
    ///          True if a gameobject is close enough and has the correct tag</returns>
    private bool GetObjectInfront()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, rayDirection, out hit, distance))
        {
            GameObject hitGameobject = hit.transform.gameObject;
            if (target != null)
            {
                if (hitGameobject.transform.gameObject.GetInstanceID() == target.GetInstanceID())
                {
                    return true;
                }
            }
            if (hitGameobject.tag == "liftable")
            {
                target = hitGameobject;
                target.GetComponent<Rigidbody>().useGravity = false;
                maxHeight = target.transform.position;
                maxHeight.y = (target.transform.position.y) + stopAtHeight;
                return true;
            }

            return false;
        }
        else 
        {
            target = null;// hmm t‰m‰ ei kiva t‰ss‰ mutta on nyt v‰liaikaisesti
            return false;
        }
    }

    private void Update()
    {
        
        
    }
}

