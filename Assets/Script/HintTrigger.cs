using UnityEngine;
// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Creator: Kettunen. J
//
// Gives a trigger the ability to show hints when player is inside

public class HintTrigger : MonoBehaviour
{
    [Tooltip("The content of the textfield")] [SerializeField] private string message;
    [Tooltip("Fontsize")] [SerializeField] private float fontSize = 5f;
    [Tooltip("Distance above target")] [SerializeField] private float offsetY = 1f;

    private InteractionHint hint;

    // Start is called before the first frame update
    void Start()
    {
        hint = gameObject.AddComponent<InteractionHint>();
        hint.SetValues(message, fontSize, offsetY);
    }

    /// <summary>
    /// Shows the hint while player is inside the trigger
    /// </summary>
    /// <param name="other">the player that hit trigger</param>
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("player"))
        {
            hint.Activate();
        }
    }

    /// <summary>
    /// Hides the hint when player goes out of triggerzone
    /// </summary>
    /// <param name="other">the player that hit trigger</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("player"))
        {
            hint.DeActivate();
        }
    }
}
