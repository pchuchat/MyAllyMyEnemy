using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionHint : MonoBehaviour
{
    [Tooltip("The content of the textfield")] [SerializeField] private string message = "example";
    [Tooltip("Fontsize")] [SerializeField] private float fontSize = 3f;
    [Tooltip("Distance above target")] [SerializeField] private float offsetY = 1f;

    private GameObject canvasObject;
    private Canvas hintCanvas;
    private RectTransform canvasrectTransform;

    private GameObject textObject;
    private TextMeshPro textField;
    private RectTransform textrectTransform;

    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        target = transform.gameObject;
        Debug.Log(target.name);

        // Canvas
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

        // Text
        textObject = new GameObject();
        textObject.transform.parent = canvasObject.transform;
        textObject.name = "text";

        textField = textObject.AddComponent<TextMeshPro>();
        //textField.font = (Font)Resources.Load("MyFont");
        textField.text = message;
        textField.fontSize = fontSize;
        textField.alignment = TextAlignmentOptions.Center;

        // Text position
        textrectTransform = textObject.GetComponent<RectTransform>();
        textrectTransform.localPosition = new Vector3(0, (target.transform.localScale.y/2 + offsetY), 0);
        textrectTransform.sizeDelta = new Vector2(2, 2);
        textField.enabled = false;
    }

    public void Activate()
    {
        textField.enabled = true;
    }
    public void DeActivate()
    {
        textField.enabled = false;
    }
}
