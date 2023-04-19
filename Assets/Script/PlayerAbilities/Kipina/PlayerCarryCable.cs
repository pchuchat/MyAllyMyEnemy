using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCarryCable : MonoBehaviour
{
    private InteractableDetection interactor;
    private GameObject cableEnd;
    private Rigidbody cableEndRB;
    private bool carrying = false;
    private PlayerInput input;
    private CharacterController controller; // Playercharacter

    // Start is called before the first frame update
    void Start()
    {
        interactor = GetComponent<InteractableDetection>();
        controller = gameObject.GetComponent<CharacterController>();
    }

    public void OnCarryCable(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(carrying == false)
            {
                cableEnd = interactor.GetInteractable("cableEnd");
                input = GetComponent<PlayerInput>();
                cableEndRB = cableEnd.GetComponent<Rigidbody>();
                PickUpCable();
            }
            else
            {
                DropCable(); // Tarviiko erikseen plug in vai riitt‰‰kˆ ett‰ pudottaa l‰helle
            }
        }
    }

    private void PickUpCable()
    {
        carrying = true;

        cableEndRB.useGravity = false;
        cableEnd.transform.forward = transform.forward; // T‰‰ jannelta
        Vector3 targetPos = transform.position + transform.forward * (cableEnd.transform.localScale.z / 2 + transform.localScale.z / 2 + 0.1f); // T‰‰ jannelta
        cableEnd.transform.position = targetPos; // T‰‰ jannelta
        input.actions.FindAction("Jump").Disable(); //Miten vaan tuplahyppy eik‰ kokonaa hyppy en jaksa ajatella :(((
        cableEnd.transform.SetParent(transform);// t‰m‰ jannelta
    }

    private void DropCable()
    {
        carrying = false;
        cableEnd.transform.SetParent(null);
        input.actions.FindAction("Jump").Enable();
        cableEndRB.useGravity = true;
        cableEnd = null;
        cableEndRB = null;
        interactor.InteractionFinished();
    }

    private void FixedUpdate()
    {
    }
}
