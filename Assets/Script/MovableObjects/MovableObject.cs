using System.Collections.Generic;
using UnityEngine;
// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Creator: Kettunen. J
//
// Deals with the collision of the movable object after it's thrown
[RequireComponent(typeof(PathSimulation))]
public class MovableObject : MonoBehaviour
{
    [Tooltip("Whether the object is heavy or not")] [SerializeField] private bool heavyLifting = false;
    [Header("Sounds")]
    [Tooltip("Sound for when the object hits target zone")] [SerializeField] private AudioClip targetHitSound;
    [Tooltip("Sound for when the object is destroyed")] [SerializeField] private AudioClip destructionSound;

    private Rigidbody rb;
    private bool keepThisOne;
    private AudioSource audioSource;
    [SerializeField]private List<GameObject> targets = new();
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public bool IsHeavy()
    {
        return heavyLifting;
    }

    /// <summary>
    /// Performed when the object hits a trigger.
    /// If the trigger is a target area snaps the object to the target and plays target hit sound
    /// </summary>
    /// <param name="collider">The trigger that was hit</param>
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("target_area") && targets.Contains(collider.gameObject) && gameObject.CompareTag("movable_object"))
        {
            audioSource.clip = targetHitSound;
            audioSource.Play();
            keepThisOne = true;
            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezePosition;
            rb.freezeRotation = true;
            transform.position = collider.gameObject.transform.position;
            transform.forward = collider.gameObject.transform.forward;
            collider.gameObject.tag = "Untagged";
            collider.gameObject.layer = 0;
            tag = "Untagged";
        }
    }

    /// <summary>
    /// Handles collisions for the movable object after it is thrown
    /// and destroys itself on collision with anything else than target area.
    /// Also plays the destrouction sound of the object if one is availble
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (!keepThisOne && gameObject.CompareTag("movable_object"))
        {
            tag = "Untagged";
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
            if (destructionSound != null)
            {
                audioSource.clip = destructionSound;
                audioSource.Play();
                Destroy(gameObject, audioSource.clip.length);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
    public void SetTargets(List<GameObject> targetAreas)
    {
        targets = targetAreas;
    }
}
