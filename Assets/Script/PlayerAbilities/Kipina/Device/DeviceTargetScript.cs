using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// By: Parviainen P
// Checks if object entering/leaving trigger area is cableEnd. Changes parent's tag to noCharge/Untagged depending on wther cableEnd enters
// or leaves the triggering area. Also gives parent which tag it should use when it is done running script

public class DeviceTargetScript : MonoBehaviour
{
    public bool cable;

    /// <summary>
    /// Changes parent's tag to "noCharge" if object entering trigger-area is cableEnd
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("cableEnd"))
        {
            cable = true;
            transform.parent.tag = "noCharge";                              
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
