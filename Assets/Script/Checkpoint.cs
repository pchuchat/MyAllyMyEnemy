using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Creator: Joonas Hopeasaari
// Data structure for a checkpoint


[Serializable]
public class Checkpoint : MonoBehaviour
{
    public Vector3 p1Position;

    public Vector3 p2Position;

    public Vector3 cameraPosition;

    public Vector3 forward;

    public int[] scenesToReload;
}
