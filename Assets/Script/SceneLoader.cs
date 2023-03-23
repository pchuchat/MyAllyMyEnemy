using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Creator: Hopeasaari J.
// TODO:
//  - Implement actual logic for loading, now simply loads specified scenes on game startup
//
// Additively loads and unloads other scenes from the main scene

public class SceneLoader : MonoBehaviour
{

    void Start()
    {
        // Additively loads the scene with index 1 in the project's build settings
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }

}
