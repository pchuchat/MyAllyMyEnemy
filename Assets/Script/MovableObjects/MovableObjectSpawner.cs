using UnityEngine;
// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Creator: Kettunen. J
//
// Makes the parent gameobject a movable object spawner, that can be used to give the player objects to carry and  throw.
public class MovableObjectSpawner : MonoBehaviour
{
    [Tooltip("The object that is spawned for the player")] [SerializeField] private GameObject movableObject;
    

    private void Start()
    {
        //makes sure the movable object that is spawned has no collider during spawning
        //or else it would collide with the spawner, collider should be (and is) added when the object is thrown
        movableObject.GetComponent<BoxCollider>().enabled = false;
    }

    /// <summary>
    /// Gives a copy of the movable object given
    /// </summary>
    /// <returns>movable object</returns>
    public GameObject GetMovableObject()
    {
        return Instantiate(movableObject);
    }
}
