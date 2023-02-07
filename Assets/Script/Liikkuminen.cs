using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Tekijä: Kettunen. J
// TODO: 
// -
// Luo hahmolle Character controllerin (ohjaimen) ja huolehtii hahmon liikkeestä pelialueella.
public class Liikkuminen : MonoBehaviour
{
    // Julkiset muuttujat joita voi muokata Unityn puolella
    public float pelaajanNopeus = 3.0f;
    public float hypynKorkeus = 1.0f;
    public float painovoima = 9.81f;

    // Privaatit muuttujat jotka eivät näy Unityssä
    private CharacterController ohjain;
    private float hyppyNopeus;
    private float maassaAjastin;
    private Vector3 liike;

    bool pelaajaMaassa;
    bool tuplahyppy;

    /// <summary>
    /// Ajetaan ensimmäisenä, luo ohjaimen.
    /// </summary>
    private void Awake()
    {
        ohjain = gameObject.AddComponent<CharacterController>();
    }

    /// <summary>
    /// Ajaa päivitykset joka framella
    /// </summary>
    private void Update()
    {
        pelaajaMaassa = ohjain.isGrounded;
        if (pelaajaMaassa)
        {
            // Annetaan pieni viive maassa olemisen tarkistukselle
            maassaAjastin = 0.2f;
        }
        if (maassaAjastin > 0)
        {
            maassaAjastin -= Time.deltaTime;
        }

        // Nollataan pelaajan pystysuuntainen nopeus maahan osuttaessa
        if (pelaajaMaassa && hyppyNopeus < 0)
        {
            hyppyNopeus = 0f;
        }

        // painovoima pelaajalle
        hyppyNopeus -= painovoima * Time.deltaTime;

        // Lasketaan liikkeen suunta
        liike = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // skaalataan liike annetun nopeuden mukaiseksi
        liike *= pelaajanNopeus;


        // Annetaan liikkeelle hystereesiä
        if (liike.magnitude > 0.05f)
        {
            gameObject.transform.forward = liike;
        }

        // Hyppääminen
        if (Input.GetButtonDown("Hyppy"))
        {
            // Sallitaan vain jos pelaaja on ollut maassa tarkistusviiveen sisällä
            if (maassaAjastin > 0)
            {
                // Nollataan ajastin varmuuden vuoksi
                maassaAjastin = 0;

                // Hypyn nopeus laskettuna hyppykorkeuden ja painovoiman avulla.
                hyppyNopeus += Mathf.Sqrt(hypynKorkeus * 2 * painovoima);
                tuplahyppy = true;
            }
            else if (tuplahyppy)
            {
                // Hypyn nopeus laskettuna hyppykorkeuden ja painovoiman avulla.
                hyppyNopeus += Mathf.Sqrt(hypynKorkeus * 2 * painovoima);
                tuplahyppy = false;
            }
        }
        liike.y = hyppyNopeus;
        ohjain.Move(liike * Time.deltaTime);
    }
}