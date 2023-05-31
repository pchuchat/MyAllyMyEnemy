using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Creator: PC, Phatchanon Chuchat
// TODO:
//  - 
//
// 

public class GamesManager : MonoBehaviour
{
    public GameObject player1Prefab;
    public GameObject player2Prefab;

    private void Start()
    {
        player1Prefab = Resources.Load<GameObject>("Haba3D");
        player2Prefab = Resources.Load<GameObject>("Kipina3D");

        Debug.Log("player1Prefab: " + player1Prefab);
        Debug.Log("player2Prefab: " + player2Prefab);
    }

    // Tämä metodi kutsutaan, kun pelaaja kuolee.
    public void RespawnPlayer(GameObject playerPrefab, Vector3 spawnLocation)
    {
        Debug.Log("RespawnPlayer method called. Attempting to create a new player after 10 seconds...");
        StartCoroutine(RespawnAfterDelay(10f, playerPrefab, spawnLocation));
    }

    IEnumerator RespawnAfterDelay(float delay, GameObject playerPrefab, Vector3 spawnLocation)
    {
        yield return new WaitForSeconds(delay);

        if (playerPrefab != null)
        {
            GameObject newPlayer = Instantiate(playerPrefab, spawnLocation, Quaternion.identity);

            if (newPlayer != null)
            {
                Debug.Log("Pelaajan luominen onnistui. Pelaaja luotu sijaintiin: " + spawnLocation);
            }
            else
            {
                Debug.Log("Pelaajan luominen epäonnistui. Tarkista, että playerPrefab on asetettu oikein.");
            }
        }
        else
        {
            Debug.LogError("playerPrefab is null. Ensure it is properly assigned.");
        }
    }
}