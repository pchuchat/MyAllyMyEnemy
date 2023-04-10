using TMPro;
using UnityEngine;
using UnityEngine.UI;
// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Creator: Kettunen. J
//
// Gives a trigger the ability to show hints when player is inside

public class HintTrigger : MonoBehaviour
{
    [Tooltip("The content of the textfield")] [SerializeField] private string message;
    [Tooltip("Fontsize")] [SerializeField] private float fontSize = 5f;
    [Tooltip("Distance above target")] [SerializeField] private float offsetY = 1f;


    //Variables for canvas element
    private GameObject canvasObject;
    private Canvas hintCanvas;
    private RectTransform canvasrectTransform;

    //Variables for textfield
    private GameObject textObject;
    private TextMeshPro textField;
    private RectTransform textrectTransform;

    //Target for position
    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        target = transform.gameObject;

        // Setting up the canvas
        canvasObject = new GameObject();
        canvasObject.transform.parent = target.transform;
        canvasObject.name = "HintCanvas";
        canvasObject.AddComponent<Canvas>();

        hintCanvas = canvasObject.GetComponent<Canvas>();
        hintCanvas.renderMode = RenderMode.WorldSpace;
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();

        canvasrectTransform = canvasObject.GetComponent<RectTransform>();
        canvasrectTransform.localPosition = new Vector3(0, 0, 0);

        // Setting up the text field
        textObject = new GameObject();
        textObject.transform.parent = canvasObject.transform;
        textObject.name = "text";

        textField = textObject.AddComponent<TextMeshPro>();
        textField.text = message;
        textField.fontSize = fontSize;
        textField.color = new Color(0, 0, 0);
        textField.alignment = TextAlignmentOptions.Center;

        // Text position
        textrectTransform = textObject.GetComponent<RectTransform>();
        textrectTransform.localPosition = new Vector3(0, (target.transform.localScale.y / 2 + offsetY), 0);
        textrectTransform.sizeDelta = new Vector2(3, 3);
        textField.enabled = false;
    }

    /// <summary>
    /// Shows the hint while player is inside the trigger
    /// </summary>
    /// <param name="other">the player that hit trigger</param>
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("player"))
        {
            textField.enabled = true;
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
            textField.enabled = false;
        }
    }
}
