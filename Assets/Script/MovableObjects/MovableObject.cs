using UnityEngine;
// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Creator: Kettunen. J
//
// Deals with the collision of the movable object after it's thrown
public class MovableObject : MonoBehaviour
{
    private Rigidbody rb;
    private bool keepThisOne;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("target_area"))
        {
            keepThisOne = true;
            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezePosition;
            transform.position = collider.gameObject.transform.position;
            collider.gameObject.tag = "Untagged";
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!keepThisOne)
        {
            Destroy(gameObject);
        }
    }
}
