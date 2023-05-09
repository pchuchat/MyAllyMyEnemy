using System.Collections.Generic;
using UnityEngine;
// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Creator: Kettunen. 
//
// Gives the object simulations needed for throwing and drawing aiminglines
[RequireComponent(typeof(LineRenderer))]
public class PathSimulation : MonoBehaviour
{
    //AimingLine
    public LineRenderer lineRenderer;
    public int maxIterations = 10000;
    public int maxSegmentCount = 300;
    public float segmentStepModulo = 10f;
    private Vector3[] segments;
    private int numSegments = 0;
    private readonly Collider[] lineHits = new Collider[3];
    public bool targetLocked = false;
    private Transform lockedTarget;
    private GameObject crosshair;

    public bool Enabled
    {
        get
        {
            return lineRenderer.enabled;
        }
        set
        {
            lineRenderer.enabled = value;
            crosshair.GetComponentInChildren<Canvas>().enabled = value;
        }
    } 

    public void Start()
    {
        Enabled = true;
        targetLocked = false;
    }

    public void SetCrosshair(GameObject crosshair)
    {
        this.crosshair = Instantiate(crosshair);
    }

    /// <summary>
    /// Calculates the required throwing force for the object to reach given target position
    /// </summary>
    /// <param name="target">which needs to be reached</param>
    /// <param name="initialAngle">throwing angle</param>
    /// <returns>the force required to reach target with given angle of throw</returns>
    public Vector3 CalculateThrowingForce(Vector3 target, float initialAngle)
    {
        // getting gravity value
        float gravity = Physics.gravity.magnitude;
        // Selected angle in radians
        float angle = initialAngle * Mathf.Deg2Rad;

        // Positions of this object and the target on the same plane
        Vector3 planarTarget = new(target.x, 0, target.z);
        Vector3 planarPosition = new(transform.position.x, 0, transform.position.z);

        // Distance between target and object on the same plane
        float distance = Vector3.Distance(planarTarget, planarPosition);
        // Difference in y direction between the target and object
        float yOffset = transform.position.y - target.y;

        // alculating the initialvelocity
        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity = new(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        // Calculating and rotating the velocity to reach target
        float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPosition) * (target.x > transform.position.x ? 1 : -1);
        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

        return finalVelocity;
    }
    /// <summary>
    /// Gets the locked target from simulation
    /// </summary>
    /// <returns>locked target</returns>
    public  Transform GetLockedTarget()
    {
        return lockedTarget;
    }
    /// <summary>
    /// Simulates a path for the given object at given force 
    /// and draws a line with a croshair for the simulated path
    /// implements also targetsnap to given targets with a given snap radius
    /// </summary>
    /// <param name="gameObject">Object which path is calculated</param>
    /// <param name="forceDirection">force that is used for calculation</param>
    /// <param name="detectRadius">Radius for detecting collisions and ending simulation</param>
    /// <param name="targets">possible targets for snapping</param>
    /// <returns>The position of the line end, or where the first hit was</returns>
    public Vector3 SimulatePath(GameObject gameObject, Vector3 forceDirection, float detectRadius = 0.1f, List<GameObject> targets = null)
    {
        Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
        float mass = rigidbody.mass;
        float drag = rigidbody.drag;

        float timestep = Time.fixedDeltaTime;

        float stepDrag = 1 - drag * timestep;
        Vector3 velocity = forceDirection / mass * timestep;
        Vector3 gravity = timestep * timestep * Physics.gravity;
        Vector3 position = gameObject.transform.position + rigidbody.centerOfMass;

        if (segments == null || segments.Length != maxSegmentCount)
        {
            segments = new Vector3[maxSegmentCount];
        }

        segments[0] = position;
        numSegments = 1;

        for (int i = 0; i < maxIterations && numSegments < maxSegmentCount && position.y > -5f; i++)
        {
            if (targets == null && Physics.OverlapSphereNonAlloc(position, detectRadius, lineHits) > 0 && (lineHits[0].gameObject.CompareTag("target_area") || !lineHits[0].isTrigger))
            {
                if (!targetLocked)
                {
                    crosshair.transform.position = position;
                    return position;
                }
                if (targetLocked && Vector3.Distance(position, lineHits[0].transform.position) <= 0.2f)
                {
                    lockedTarget = lineHits[0].transform;
                    return position;
                }
            }
            else if (Physics.OverlapSphereNonAlloc(position, detectRadius, lineHits) > 0 && lineHits[0].gameObject.CompareTag("target_area") && targets.Contains(lineHits[0].gameObject))
            {
                crosshair.transform.position = lineHits[0].gameObject.transform.position;
                targetLocked = true; 
                lockedTarget = lineHits[0].transform;
                return lineHits[0].gameObject.transform.position;
            }
            else if (targets != null && Physics.Raycast(position, transform.TransformDirection(Vector3.down), out RaycastHit hit, 5f) && hit.collider.gameObject.CompareTag("target_area") && targets.Contains(hit.collider.gameObject))
            {
                crosshair.transform.position = hit.collider.gameObject.transform.position;
                targetLocked = true;
                return hit.collider.gameObject.transform.position;
            }
            velocity += gravity;
            velocity *= stepDrag;

            position += velocity;

            if (i % segmentStepModulo == 0)
            {
                segments[numSegments] = position;
                numSegments++;
            }
        }
        targetLocked = false;
        crosshair.transform.position = position;
        return position;
    }
    /// <summary>
    /// Draws the simulated path and sets the reticle active
    /// </summary>
    public void Draw()
    {
        lineRenderer.transform.position = segments[0];

        lineRenderer.positionCount = numSegments;
        for (int i = 0; i < numSegments; i++)
        {
            lineRenderer.SetPosition(i, segments[i]);
        }
    }
}
