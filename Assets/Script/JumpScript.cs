using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpScript : MonoBehaviour
{
    private CharacterController hahmoOhjain;
    private Vector3 pelaajankiittyvyys;
    private bool pelaajaMaassa;
    [SerializeField] private float hyppyKorkeus = 5.0f;

    private bool hyppyPainettu = false;
    private float painovoima = -9.81f;

    void Start()
    {
        hahmoOhjain = GetComponent<CharacterController>();
    }
    void Update()
    {
        MovementJump();
      
    }


    void MovementJump()
    {
       pelaajaMaassa = hahmoOhjain.isGrounded;
        if(pelaajaMaassa)
        {
            pelaajankiittyvyys.y = 0.0f;
        }



        if(hyppyPainettu && pelaajaMaassa)
        {
            pelaajankiittyvyys.y += Mathf.Sqrt(hyppyKorkeus * -1.0f * painovoima);
            hyppyPainettu = false;
        }

        pelaajankiittyvyys.y += painovoima * Time.deltaTime;
        hahmoOhjain.Move(pelaajankiittyvyys * Time.deltaTime);
    }

    public void onJump()
    {
        if (hahmoOhjain.velocity.y == 0) { Debug.Log("Hyppaskin");
            hyppyPainettu = true;
        } else {
            Debug.Log("‰l‰ hypp‰‰");
        
        }
       
    }
}

