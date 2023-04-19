using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    [Tooltip("How many degrees the camera should be rotated")] [SerializeField] private float rotateDeg = 180f;
    [Tooltip("Speed of the rotation in degrees/second")] [SerializeField] private float rotateSpeed = 20f;
    [Tooltip("Tag for parent to activate rotating")] [SerializeField] private string activationTag = "charged";

    private CameraMovement cameraMovement;
    private bool rotated = false;
    // Start is called before the first frame update
    void Start()
    {
        cameraMovement = Camera.main.GetComponentInParent<CameraMovement>();
    }
    // Update is called once per frame
    void Update()
    {
        if (CompareTag(activationTag) && !rotated)
        {
            rotated = true;
            cameraMovement.RotateCamera(rotateDeg, rotateSpeed);
        }
    }
}
