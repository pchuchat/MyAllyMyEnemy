using UnityEngine;

public class DoubleJumpHintTrigger : MonoBehaviour
{

    private InteractionHint hint;
    // Start is called before the first frame update
    void Start()
    {
        hint = GetComponentInParent<InteractionHint>();
    }

    private void OnTriggerStay(Collider other)
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
}
