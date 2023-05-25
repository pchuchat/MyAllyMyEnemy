using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform player;
    private float dist;
    public float moveSpeed;
    public float howClose;

    void Start()
    {

    }

    public float damageAmount = 10.0f;  // Vihollisen aiheuttama vahinko

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Törmäys tapahtui.");

        Health playerHealth = collision.gameObject.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);
            Debug.Log("Pelaaja otti vahinkoa.");
        }
        else
        {
            Debug.Log("Health-komponenttia ei löytynyt pelaajalta.");
        }
    }

    void Update()
    {
        // Yritä hakea pelaaja-olio, jos sitä ei ole vielä löydetty
        if (player == null)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("player");
            float closestDistance = Mathf.Infinity; // Alustetaan lähimmän pelaajan etäisyys äärettömän suureksi

            foreach (GameObject potentialPlayer in players) // Käydään läpi kaikki "player"-tagilliset objektit
            {
                float distance = Vector3.Distance(potentialPlayer.transform.position, transform.position); // Lasketaan etäisyys tästä Enemy-oliosta pelaajaan
                if (distance < closestDistance) // Jos tämä pelaaja on lähempänä kuin aiemmat pelaajat
                {
                    closestDistance = distance; // Päivitetään lähimmän pelaajan etäisyys
                    player = potentialPlayer.transform; // Asetetaan tämä pelaaja seurattavaksi
                }
            }
        }

        if (player != null)
        {
            dist = Vector3.Distance(player.position, transform.position);
            /*  // Tulostaa etäisyyden ja howClose-arvon joka kerta kun Update()-metodia kutsutaan
            Debug.Log("Distance to player: " + dist + ", How close: " + howClose);*/

            if (dist <= howClose)
            {
                transform.LookAt(player);
                GetComponent<Rigidbody>().AddForce(transform.forward * moveSpeed);
            }
        }
    }
}