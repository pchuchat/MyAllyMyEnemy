using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// By: Parviainen P and Kettunen J
// Kipinä picks up, drops and carries cableEnd-object around

public class PlayerCarryCable : MonoBehaviour
{
    private InteractableDetection interactor;
    private GameObject cableEnd;
    private Rigidbody cableEndRB;
    private bool carrying = false;

    // Start is called before the first frame update
    void Start()
    {
        interactor = GetComponent<InteractableDetection>();
    }

    public bool IsCarrying()
    {
        return carrying;
    }

    /// <summary>
    /// Checks if object in front is cableEnd and if so sets variables and calls PickUpCable
    /// </summary>
    /// <param name="context"></param>
    public void OnCarryCable(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(carrying == false)
            {
                cableEnd = interactor.GetInteractable("cableEnd");
                if(cableEnd != null)
                {
                    cableEndRB = cableEnd.GetComponent<Rigidbody>();
                    PickUpCable();
                }
            }
            else
            {
                DropCable();
            }
        }
    }

    /// <summary>
    /// Picks up cableEnd, sets cable end gravity to false and disables jump from player
    /// </summary>
    private void PickUpCable()
    {
        carrying = true;
        cableEndRB.useGravity = false;
        cableEnd.transform.forward = transform.forward;
        Vector3 targetPos = transform.position + transform.forward * transform.localScale.x * 0.75f + transform.up * transform.localScale.y;
        cableEndRB.constraints = RigidbodyConstraints.FreezeAll;
        cableEnd.transform.position = targetPos;
        cableEnd.transform.SetParent(transform);
    }

    /// <summary>
    /// Drops cableEnd and nulls variables
    /// </summary>
    public void DropCable()
    {
        if (carrying)
        {
            carrying = false;
            cableEnd.transform.SetParent(null);
            cableEndRB.useGravity = true;
            cableEndRB.constraints &= ~RigidbodyConstraints.FreezePositionY;
            cableEnd = null;
            cableEndRB = null;
            interactor.InteractionFinished();
        }       
    }

}
