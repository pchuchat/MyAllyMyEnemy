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
    private GameObject[] players;
    private Camera mainCamera;
    void Start()
    {
        mainCamera = Camera.main;
    }

    /// <summary>
    /// Moves the camera according to the center point of the two players
    /// </summary>
    public void MoveCamera()
    {
        float zoomFactor = 10f;
        float followTimeDelta = 0.8f;
        
            // Calculating the midpoint of the players
            Vector3 midpoint = (players[0].transform.position + players[1].transform.position) / 2f;

            // Moving the camera
            Vector3 cameraDestination = midpoint - mainCamera.transform.forward * zoomFactor;
            mainCamera.transform.position = Vector3.Slerp(mainCamera.transform.position, cameraDestination, followTimeDelta);

    }

    void Update()
    {
        players = GameObject.FindGameObjectsWithTag("player");
        MoveCamera();
    }
    
}
