using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableEndCollider : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (transform.parent)
        {
            if (transform.parent.CompareTag("player"))
            {
                transform.parent.GetComponent<PlayerCarryCable>().DropCable();
            }
        }
       
    }

    void OnCollisionStay(Collision collision)
    {
        if (transform.parent == null && !collision.collider.CompareTag("player") && !collision.collider.CompareTag("Untagged"))
        {
            transform.position = Vector3.MoveTowards(transform.position, collision.collider.transform.position, -1 * Time.deltaTime);
        }
    }
}
