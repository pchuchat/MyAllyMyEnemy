using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5;
    private Vector2 movementInput;
    public float jumpForce = 5f;





    private void Update()
    {
        transform.Translate(new Vector3(movementInput.x, 0, movementInput.y) * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump"))
        {
            transform.Translate(new Vector3(movementInput.x, jumpForce,movementInput.y));
        }
    }

 

      
    
    public void OnMove(InputAction.CallbackContext ctx) =>movementInput = ctx.ReadValue<Vector2>();
    

  
}
