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
    private GameObject reticle;
    private readonly Collider[] lineHits = new Collider[1];

    public bool Enabled
    {
        get
        {
            return lineRenderer.enabled;
        }
        set
        {
            lineRenderer.enabled = value;
        }
    }

    public void Start()
    {
        Enabled = true;
        //Temporary reticle
        reticle = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        reticle.GetComponent<SphereCollider>().enabled = false;
        reticle.SetActive(false);
        reticle.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    /// <summary>
    /// Destroys the aiming components of the corshair for when it is no longer needed.
    /// </summary>
    public void Terminate()
    {
        Destroy(reticle);
        lineRenderer.enabled = false;

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
    /// Simulates a path for the given object at given force 
    /// and draws a line with a croshair for the simulated path
    /// </summary>
    /// <param name="gameObject">Object which path is calculated</param>
    /// <param name="forceDirection">force that is used for calculation</param>
    public void SimulatePath(GameObject gameObject, Vector3 forceDirection)
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

        for (int i = 0; i < maxIterations && numSegments < maxSegmentCount && position.y > -5f; i++ )
        {
            if (Physics.OverlapSphereNonAlloc(position, 0.01f, lineHits) > 0 && !lineHits[0].gameObject.CompareTag("hintTrigger"))
            {
                reticle.transform.position = position;
                reticle.SetActive(true);
                break;
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
        lineRenderer.transform.position = segments[0];

        lineRenderer.positionCount = numSegments;
        for (int i = 0; i < numSegments; i++)
        {
            lineRenderer.SetPosition(i, segments[i]);
        }
    }
}
