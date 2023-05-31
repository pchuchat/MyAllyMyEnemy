using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Creator: PC, Phatchanon Chuchat
// TODO:
//  - PauseMenu
//
// 

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool isPaused = false;
    private PlayerControl playerControls;

    // Awake method is called as soon as the component is created
    private void Awake()
    {
        playerControls = new PlayerControl();

        // Add a listener for the Pause input to the TogglePause method
        playerControls.Player.Pause.performed += _ => TogglePause();
    }

    // OnEnable method is called when the component is enabled
    private void OnEnable()
    {
        playerControls.Player.Enable();
    }

    // OnDisable method is called when the component is disabled
    private void OnDisable()
    {
        playerControls.Player.Disable();
    }

    // TogglePause method toggles the pause state of the game and controls the display of the pause menu
    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenuUI.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;

        // Disable player controls and enable UI controls when the game is paused
        if (isPaused)
        {
            playerControls.Player.Disable();
            playerControls.UI.Enable();
        }
        else
        {
            playerControls.UI.Disable();
            playerControls.Player.Enable();
        }
    }

    // RestartGame method restarts the game by reloading the current scene
    public void RestartGame()
    {
        // Restore the time scale to normal before reloading the scene
        Time.timeScale = 1;

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Quit method quits the application
    public void Quit()
    {
        Application.Quit();
    }
}