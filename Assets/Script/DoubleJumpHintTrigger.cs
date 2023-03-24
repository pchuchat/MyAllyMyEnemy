using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpHintTrigger : MonoBehaviour
{

    private InteractionHint hint;
    // Start is called before the first frame update
    void Start()
    {
        hint = GetComponentInParent<InteractionHint>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            hint.Activate();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("player"))
        {
            hint.DeActivate();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
