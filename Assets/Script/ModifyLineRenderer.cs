using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyLineRenderer : MonoBehaviour
{
    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        AddNewPoint(new Vector3(1, 30, 0));
    }

    private void AddNewPoint(Vector3 newPoint)
    {
        lineRenderer.positionCount += 1;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newPoint);
    }
}
