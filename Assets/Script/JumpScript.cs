using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
 [RequireComponent(typeof(CharacterController))]
public class JumpScript : MonoBehaviour
{
    private CharacterController Hyppy;
    // Start is called before the first frame update

    //  hypyn arvo
    private Vector3 PelaajanNopeus;
    private bool PelaajaMaassa;
    private Vector2 JumpInput;

    [SerializeField] private float HypynKorkeus = 5.0f;
    private bool PainettuHyppy = false;
    private float PainoVoima = -9.81f;
    void Start()
    {
        Hyppy = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        MovementJump();
    }

    public void MovementJump()
    {
        PelaajaMaassa = Hyppy.isGrounded;

        // jos pelaaaja on maassa pysäytä liike
        if (PelaajaMaassa)
        {
            PelaajanNopeus.y = 0.0f;
        }
        // on painettu hyppyä ja maassa
        if (PainettuHyppy && PelaajaMaassa)
        {
            PelaajanNopeus.y += Mathf.Sqrt(HypynKorkeus * (-1.0f *PainoVoima));
            PainettuHyppy = false;
        }

        PelaajanNopeus.y += PainoVoima * Time.deltaTime;
        Hyppy.Move(PelaajanNopeus * Time.deltaTime);
    }

    public void OnJump(InputAction.CallbackContext ctx) 
    {
        // tarkistaa onko verticaalisia liiketta
        if( Hyppy.velocity.y == 0 )
        {
            Debug.Log("Jippiii");
            PainettuHyppy = true;
        }
        else
        {
            Debug.Log("Älä Jaksa");
        }
    }
}
