using UnityEngine;
// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Creator: Kettunen. J
//
// Deals with the collision of the movable object after it's thrown
public class MovableObject : MonoBehaviour
{
    [Tooltip("Sound for picking up object from spawner")] [SerializeField] private AudioClip pickUpSound;
    [Tooltip("Sound for when the object is thrown")] [SerializeField] private AudioClip throwSound;
    [Tooltip("Sound for when the object hits target zone")] [SerializeField] private AudioClip targetSound;
    [Tooltip("Sound for when the object is destroyed")] [SerializeField] private AudioClip destructionSound;

    private Rigidbody rb;
    private bool keepThisOne;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("target_area") && gameObject.CompareTag("movable_object"))
        {
            audioSource.clip = targetSound;
            audioSource.Play();
            keepThisOne = true;
            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezePosition;
            rb.freezeRotation = true;
            transform.position = collider.gameObject.transform.position;
            transform.forward = collider.gameObject.transform.forward;
            collider.gameObject.tag = "Untagged";
            gameObject.tag = "Untagged";
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!keepThisOne && gameObject.CompareTag("movable_object"))
        {
            gameObject.tag = "Untagged";
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            audioSource.clip = destructionSound;
            audioSource.Play();
            Destroy(gameObject, audioSource.clip.length);
        }
    }
    public void PlayPickUpSound()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = pickUpSound;
        audioSource.Play();
    }
    public void PlayThrowSound()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = throwSound;
        audioSource.Play();
    }
}
