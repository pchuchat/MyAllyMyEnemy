using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// By: Parviainen P
// Checks if object entering/leaving trigger area is cableEnd. Changes parent's tag to noCharge/Untagged depending on wther cableEnd enters
// or leaves the triggering area. Also gives parent which tag it should use when it is done running script

public class DeviceTargetScript : MonoBehaviour
{
    [SerializeField]
    private GameObject cableTarget;

    [SerializeField]
    private IsInteractable interactScript;

    public bool cable;
    private PlayerCarryCable carryScript;

    void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("player");

        try { carryScript = players[0].GetComponent<PlayerCarryCable>(); }
        catch (Exception e) { Debug.Log(e + "Getting Player CarryCable script failed on [0]"); }

        try { carryScript = players[1].GetComponent<PlayerCarryCable>(); }
        catch (Exception e) { Debug.Log(e + "Getting Player CarryCable script failed on [1]"); }
    }

    /// <summary>
    /// Changes parent's tag to "noCharge" if object entering trigger-area is cableEnd
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("cableEnd"))
        {
            if (carryScript.carrying)
            {
                carryScript.DropCable();
            }

            cable = true;
            other.tag = "Untagged";
            transform.parent.tag = "noCharge";

            GameObject cableObject = other.gameObject;
            Rigidbody rbCable = cableObject.GetComponent<Rigidbody>();

            rbCable.isKinematic = true;
            rbCable.detectCollisions = false;

            cableObject.transform.position = cableTarget.transform.position;
            cableObject.transform.rotation = cableTarget.transform.rotation;

            interactScript.interactHint = "Press O to power up";
        }
    }

    /// <summary>
    /// Determines which tag device should use after it is done
    /// </summary>
    /// <returns></returns>
    public string WhichTag()
    {
        if (cable)
        {
            return "noCharge";
        }
        else return "Untagged";
    }

    /// <summary>
    /// Changes parent's tag to "Untagged" if object leaving trigger-area is cableEnd
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("cableEnd"))
        {
            cable = false;
            transform.parent.tag = "Untagged";
        }
    }
}
