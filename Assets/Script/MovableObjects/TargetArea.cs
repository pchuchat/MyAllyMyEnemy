using UnityEngine;

public class TargetArea : MonoBehaviour
{
    [Header("Targetsnap attributes")]
    [Tooltip("The amount of added snapping area around the taget, 0 for the size of target")] [SerializeField] private float snapDistance = 2.0f;

    private BoxCollider targetCol;
    void Start()
    {
        targetCol = GetComponent<BoxCollider>();
        targetCol.size = new Vector3(targetCol.size.x + snapDistance, targetCol.size.y + 1f, targetCol.size.z + snapDistance);
    }
}
