using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pelaajaelama : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("vihollineruumi"))
        {
            Die();
        }
    }

    void Die()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Liikkuminen>().enabled = false;
        Invoke(nameof(Reloadlevel), 1.3f);
    }

    void Reloadlevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
