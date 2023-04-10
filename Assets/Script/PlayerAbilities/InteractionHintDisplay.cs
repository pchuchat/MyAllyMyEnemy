using TMPro;
using UnityEngine;
// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Creator: Kettunen. J
//
// Displays hints for player above the player
public class InteractionHintDisplay : MonoBehaviour
{
    private Camera mainCamera;
    private TextMeshProUGUI interactionHint;

    private void Start()
    {
        mainCamera = Camera.main;
        interactionHint = GetComponentInChildren<TextMeshProUGUI>();
        interactionHint.enabled = false;
    }

    private void LateUpdate()
    {
        var rotation = mainCamera.transform.rotation;
        transform.LookAt(transform.position + rotation * Vector3.forward, rotation * Vector3.up);
    }

    public bool isActive = false;
    /// <summary>
    /// Sets the hint active with given text
    /// </summary>
    /// <param name="text">shown in hint</param>
    public void SetHint(string text)
    {
        interactionHint.text = text;
        interactionHint.enabled = true;
        isActive = true;
    }
    /// <summary>
    /// Deactivates the hint and resets values
    /// </summary>
    public void Deactivate()
    {
        interactionHint.enabled = false;
        isActive = false;
        interactionHint.text = null;
    }
}
