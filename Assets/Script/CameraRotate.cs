using System.Reflection.Emit;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{

    private enum RotateDirection { Shortest, Clockwise, Counterclockwise }
    [Tooltip("Direction of rotation")][SerializeField] private RotateDirection rotateDirection;
    [Tooltip("New (y) rotation spesified in world space as degrees")][SerializeField] private float newRotation = 180f;
    [Tooltip("Time for rotation in seconds")][SerializeField] private float rotationTime = 2f;
    [Tooltip("Tag for parent to activate rotating, not needed when using triggers")][SerializeField] private string activationTag = "charged";
    [Tooltip("Whether multiple activations are allowed, leave off for devices")][SerializeField] private bool allowMultipleRotations = false;
    private CameraMovement cameraMovement;    
    private bool rotated = false;
    private Transform cameraRig;
    private float rotateSpeed;
    // Start is called before the first frame update
    void Start()
    {
        cameraMovement = Camera.main.GetComponentInParent<CameraMovement>();
        cameraRig = cameraMovement.GetComponent<Transform>();
    }

    public void Rotate()
    {
        float difference = Mathf.DeltaAngle(cameraRig.rotation.eulerAngles.y, newRotation);
        if (Mathf.Abs(difference) < 10f) return;
        switch (rotateDirection)
        {
            case RotateDirection.Shortest:
                if (difference > 0) goto Clockwise;
                else goto counterClockWise;
            case RotateDirection.Clockwise:
            Clockwise:
                if (difference < 0) difference += 360;
                break;
            case RotateDirection.Counterclockwise:
            counterClockWise:
                if (difference > 0) difference -= 360;
                break;
        }

        //Debug.Log(difference);
        rotateSpeed = difference / rotationTime;
        if (!allowMultipleRotations) rotated = true;
        cameraMovement.RotateCamera(difference, rotateSpeed);
    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(cameraRig.rotation.eulerAngles.y);
        if (CompareTag(activationTag) && !rotated && !cameraMovement.rotating)
        {
            Rotate();
        }
    }
}
