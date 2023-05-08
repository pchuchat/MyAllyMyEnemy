 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Creator: Hopeasaari J.
// TODO:
//  - Implement actual logic for loading, now simply loads specified scenes on game startup
//
// Additively loads and unloads level scenes from the main scene

public class SceneLoader : MonoBehaviour
{
    private bool loadScenes = true;

    void Start()
    {
        // Don't load scenes in the editor
        #if UNITY_EDITOR
            loadScenes = false;
        #endif

        // Additively loads all the scenes of Level 1, based on indexes in the project's build settings
        // NOTE: Logic to load and unload scenes appropriately on the fly should be implemented later
        if (loadScenes)
        {
            SceneManager.LoadScene(2, LoadSceneMode.Additive);
            SceneManager.LoadScene(3, LoadSceneMode.Additive);
            SceneManager.LoadScene(4, LoadSceneMode.Additive);
            SceneManager.LoadScene(5, LoadSceneMode.Additive);
            SceneManager.LoadScene(6, LoadSceneMode.Additive);
        }
    }

}
