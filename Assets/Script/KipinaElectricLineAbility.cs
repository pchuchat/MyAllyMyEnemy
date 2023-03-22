using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KipinaElectricLineAbility : MonoBehaviour
{
    [SerializeField] private float electricLineSpeed = 5.0f;
    [SerializeField] private LayerMask electricLineLayer;
    [SerializeField] private GameObject electricBall;
    [SerializeField] private GameObject electricBallHolder;

    private CharacterController characterController;
    private PlayerMovement playerMovement;
    private bool isOnElectricLine = false;
    private LineRenderer currentElectricLine;
    private float currentLineProgress = 0.0f;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float electricLineDistance;
    private float horizontalInputValue;
   


    private void Start()
    {

        electricBall = electricBallHolder.transform.GetChild(0).gameObject;
        SetElectricBallActive(false);
        characterController = GetComponent<CharacterController>();
        playerMovement = GetComponent<PlayerMovement>();
        Time.fixedDeltaTime = 0.02f;
    }

    private void SetElectricBallActive(bool active)
    {
        electricBall.GetComponent<MeshRenderer>().enabled = active;
        electricBallHolder.transform.position = transform.position;
        Debug.Log("ElectricBall asetettu " + (active ? "aktiiviseksi" : "passiiviseksi"));
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && !isOnElectricLine)
        {
            Debug.Log("Interact-painiketta painettu ja hahmo ei ole sähkölinjalla");
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.forward, out hit, 3.0f, electricLineLayer))
            {
                currentElectricLine = hit.collider.gameObject.GetComponent<LineRenderer>() ?? hit.collider.gameObject.transform.parent.GetComponent<LineRenderer>();
                startPosition = currentElectricLine.GetPosition(0);
                endPosition = currentElectricLine.GetPosition(1);
                electricLineDistance = Vector3.Distance(startPosition, endPosition);
                isOnElectricLine = true;
                playerMovement.enabled = false;
                characterController.enabled = false;

                // Aseta ElectricBall aktiiviseksi
                SetElectricBallActive(true);
                Debug.Log("Hahmo siirtyi sähkölinjalle");

                // Aseta hahmon sijainti raycastin osumapisteeksi
                transform.position = hit.point;

                // Aseta currentLineProgress oikeaan arvoon suhteessa osumapisteeseen
                float closestPointDistance = Mathf.Clamp(Vector3.Distance(hit.point, startPosition), 0, electricLineDistance);
                currentLineProgress = closestPointDistance;
            }
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (isOnElectricLine)
        {
            horizontalInputValue = context.ReadValue<Vector2>().x;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isOnElectricLine)
        {
            // Tarkistetaan, onko pelaaja sähkölinjan alkupisteessä tai loppupisteessä
            Vector3 playerPosition = transform.position;
            bool atStart = playerPosition == startPosition;
            bool atEnd = playerPosition == endPosition;

            // Nollaa sähkölinjan muuttujat
            currentElectricLine = null;
            startPosition = Vector3.zero;
            endPosition = Vector3.zero;
            electricLineDistance = 0.0f;
            isOnElectricLine = false;
            playerMovement.enabled = true;
            characterController.enabled = true;
            // Aseta ElectricBall passiiviseksi
            SetElectricBallActive(false);

            // Jos pelaaja ei ole sähkölinjan loppupisteessä, tarkistetaan, voiko hän palata sähkölinjalle
            if (!atEnd)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.forward, out hit, 3.0f, electricLineLayer))
                {
                    currentElectricLine = hit.collider.gameObject.GetComponent<LineRenderer>() ?? hit.collider.gameObject.transform.parent.GetComponent<LineRenderer>();
                    startPosition = currentElectricLine.GetPosition(0);
                    endPosition = currentElectricLine.GetPosition(1);
                    electricLineDistance = Vector3.Distance(startPosition, endPosition);
                    isOnElectricLine = true;
                    playerMovement.enabled = false;
                    characterController.enabled = false;

                    // Aseta pelaajan sijainti raycastin osumapisteeksi
                    transform.position = hit.point;
                }
            }

            // Palauta pelaaja sähkölinjalle
            if (isOnElectricLine)
            {
                if (atEnd)
                {
                    currentLineProgress = electricLineDistance;
                }
                else
                {
                    currentLineProgress = Vector3.Distance(transform.position, startPosition);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (isOnElectricLine)
        {
            // Liikuta hahmoa sähkölinjalla pelaajan ohjauksessa
            currentLineProgress += horizontalInputValue * electricLineSpeed * Time.fixedDeltaTime;
            currentLineProgress = Mathf.Clamp(currentLineProgress, 0, electricLineDistance);

            // Päivitä hahmon sijainti sähkölinjalla
            float lerpValue = currentLineProgress / electricLineDistance;
            Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, lerpValue);
            transform.position = newPosition;
        }
    }
}