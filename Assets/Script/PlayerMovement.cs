using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController hahmoohjain;
    public float speed = 3.0f;
    Vector2 movementInput;

    private PlayerControls ohjain;

    private PlayerControls Ohjain
    {
        get
        {
            if(ohjain != null)
            {
                return ohjain;
            }
            return ohjain = new PlayerControls();
              
            
        }
    }

    private void SetMovement(Vector2 inputVector) => movementInput = inputVector;

    private void CancelMovement() => movementInput = Vector2.zero;


    private void Start()
    {
        hahmoohjain = GetComponent<CharacterController>();

        var renderColor = GetComponent<Renderer>();
        renderColor.material.SetColor("Color", Color.blue);

        Ohjain.Player.Move.performed += ctx => SetMovement(ctx.ReadValue<Vector2>());
        Ohjain.Player.Move.canceled += ctx => CancelMovement();
    }

    private void OnEnable()
    {
        Ohjain.Enable();
    }

    private void OnDisable()
    {
        Ohjain.Disable();
    }
    void FixedUpdate()
    {

        Vector3 moveVector = new Vector3(movementInput.x, 0.0f, movementInput.y);

        hahmoohjain.Move(moveVector * speed * Time.deltaTime);


    }
    

  
}
