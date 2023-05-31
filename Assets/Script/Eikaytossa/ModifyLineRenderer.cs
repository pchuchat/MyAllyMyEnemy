using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Creator: PC, Phatchanon Chuchat
// TODO:
// 
//
// 

public class ModifyLineRenderer : MonoBehaviour
{
    private LineRenderer lineRenderer;

    private void Start()
    {
        // Get the LineRenderer component attached to the same GameObject
        lineRenderer = GetComponent<LineRenderer>();

        // Add a new point to the LineRenderer at the specified position
        AddNewPoint(new Vector3(1, 30, 0));
    }

    private void AddNewPoint(Vector3 newPoint)
    {
        // Increase the position count of the LineRenderer by 1
        lineRenderer.positionCount += 1;

        // Set the position of the newly added point in the LineRenderer
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newPoint);
    }
}