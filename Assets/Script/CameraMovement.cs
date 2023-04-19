using UnityEngine;
// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Creator: Kettunen. J
// TODO:
//  -preventing players from moving off-camera
//  -limiting camera movement to level boundaries
// Finds the objects tagged as player in the scene and follows their midpoint with the camera(rig)
public class CameraMovement : MonoBehaviour
{
    [Tooltip("How far the camera is away from players")] [SerializeField] private float zoomFactor = 15f;
    [Tooltip("The height of the camera from groundlevel")] [SerializeField] private float cameraHeight = 13f;
    [Tooltip("Follow speed of the camera")] [SerializeField] private float follofSpeed = 2f;

    private GameObject[] players;
    private Vector3[] playerPositions = new Vector3[2];

    /// <summary>
    /// Moves the camera according to the center point of the two players
    /// </summary>
    public void MoveCamera()
    {
        if (players.Length == 2)
        {
            // Calculating the midpoint of the players
            Vector3 midpoint = (players[0].transform.position + players[1].transform.position) / 2f;
            midpoint.y = playerPositions[0].y + playerPositions[1].y / 2;

            // Moving the camera
            Vector3 cameraDestination = midpoint - transform.forward * zoomFactor;
            cameraDestination.y = cameraHeight + midpoint.y;
            transform.position = Vector3.Slerp(transform.position, cameraDestination, follofSpeed * Time.deltaTime);
        }
        if (players.Length == 1) //Added for testing with one player
        {
            // Getting the position of the player
            Vector3 cameraTarget = players[0].transform.position;

            // Moving the camera
            Vector3 cameraDestination = cameraTarget - transform.forward * zoomFactor;
            cameraDestination.y = cameraHeight;
            transform.position = Vector3.Slerp(transform.position, cameraDestination, follofSpeed * Time.deltaTime);
        }
    }

    void Update()
    {
        //Finding the players from the scene
        players = GameObject.FindGameObjectsWithTag("player");
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<CharacterController>().isGrounded)
            {
                playerPositions[i] = players[i].transform.position;
            }
        }
        MoveCamera();
    }
}
