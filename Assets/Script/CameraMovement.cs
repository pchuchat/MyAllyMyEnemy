using UnityEngine;
// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Creator: Kettunen. J
// TODO:
//  -preventing players from moving off-camera
//  -limiting camera movement to level boundaries
//
// Finds the objects tagged as player in the scene and follows their midpoint with the camera(rig)
public class CameraMovement : MonoBehaviour
{
    public float zoomFactor = 15f;
    public float cameraHeight = 10f;

    private GameObject[] players;
    void Start()
    {

    }

    /// <summary>
    /// Moves the camera according to the center point of the two players
    /// </summary>
    public void MoveCamera()
    {
        float followTimeDelta = 0.8f;
        if (players.Length == 2)
        {
            // Calculating the midpoint of the players
            Vector3 midpoint = (players[0].transform.position + players[1].transform.position) / 2f;

            // Moving the camera
            Vector3 cameraDestination = midpoint - transform.forward * zoomFactor;
            cameraDestination.y = cameraHeight;
            transform.position = Vector3.Slerp(transform.position, cameraDestination, followTimeDelta);
        }
        if (players.Length == 1) //Added for testing with one player
        {
            // Getting the position of the player
            Vector3 cameraTarget = players[0].transform.position;

            // Moving the camera
            Vector3 cameraDestination = cameraTarget - transform.forward * zoomFactor;
            cameraDestination.y = cameraHeight;
            transform.position = Vector3.Slerp(transform.position, cameraDestination, followTimeDelta);
        }
    }

    void Update()
    {
        //Finding the players from the scene
        players = GameObject.FindGameObjectsWithTag("player");

        MoveCamera();
    }
    
}
