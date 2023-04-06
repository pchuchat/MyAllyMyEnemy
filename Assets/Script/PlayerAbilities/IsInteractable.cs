using UnityEngine;
// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Creator: Kettunen. J
//
// Gives the object neccessary variables for interactions
[RequireComponent(typeof(Outline))]
public class IsInteractable : MonoBehaviour
{
    private enum Player { Haba, Kipina }
    [Tooltip("Player that can interact with this item")] [SerializeField] private Player player;
    [Tooltip("Hint for player that can interact")] [SerializeField] private string interactHint = "example hint";
    [Tooltip("Hint for player that can't interact")] [SerializeField] private string cantInteractHint = "You can't interact with this one";
    [Tooltip("Thickness of the outline")] [SerializeField] private float outlineThickness = 2f;
    private Outline highlight;

    // Start is called before the first frame update
    void Start()
    {
        highlight = GetComponent<Outline>();
        if (player == Player.Haba) highlight.OutlineColor = Color.red;
        if (player == Player.Kipina) highlight.OutlineColor = Color.blue;
        highlight.OutlineWidth = outlineThickness;
    }
    /// <summary>
    /// Gets the hint for the object depending on player given as parameter
    /// </summary>
    /// <param name="interactingPlayer">player asking for hint</param>
    /// <returns></returns>
    public string GetHint(GameObject interactingPlayer)
    {
        if (interactingPlayer.name == player + "(Clone)") return interactHint;
        else return cantInteractHint;
    }
}
