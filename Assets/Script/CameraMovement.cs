using System;
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
    [Tooltip("Follow speed of the camera")] [SerializeField] private float followSpeed = 2f;

    private GameObject[] players;
    private Vector3[] playerPositions = new Vector3[2];
    private Vector3 midpoint;
    private float rotateSpeed;
    private float amountRotated = 0;
    private float rotateDegrees;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        mainCamera.transform.eulerAngles = new Vector3(90f - (Mathf.Asin(zoomFactor/Mathf.Sqrt(cameraHeight * cameraHeight + zoomFactor * zoomFactor)) * Mathf.Rad2Deg), 0, 0);
    }
    /// <summary>
    /// Moves the camera according to the center point of the two players
    /// </summary>
    public void MoveCamera()
    {
        if (players.Length == 2)
        {
            // Calculating the midpoint of the players
            midpoint = (players[0].transform.position + players[1].transform.position) / 2f;
            midpoint.y = playerPositions[0].y + playerPositions[1].y / 2;

            // Moving the camera
            Vector3 cameraDestination = midpoint - transform.forward * zoomFactor;
            cameraDestination.y += cameraHeight;
            transform.position = Vector3.Slerp(transform.position, cameraDestination, followSpeed * Time.deltaTime);
        }
        if (players.Length == 1) //Added for testing with one player
        {
            // Getting the position of the player
            Vector3 cameraTarget = players[0].transform.position;

            // Moving the camera
            Vector3 cameraDestination = cameraTarget - transform.forward * zoomFactor;
            cameraDestination.y = cameraHeight;
            transform.position = Vector3.Slerp(transform.position, cameraDestination, followSpeed * Time.deltaTime);
        }
    }

    public void RotateCamera(float rotDeg, float rotateSpeed)
    {
        this.rotateSpeed = rotateSpeed;
        rotateDegrees = rotDeg;
    }
    void Update()
    {
        float rotate = 0;
        if (amountRotated < rotateDegrees)
        {
            rotate = rotateSpeed * Time.deltaTime;
            amountRotated += rotate;
        }
        if (rotate != 0)
        {
            transform.RotateAround(midpoint, Vector3.up, rotate);
        }

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
