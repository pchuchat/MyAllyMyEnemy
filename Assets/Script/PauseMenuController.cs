using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool isPaused = false;
    private PlayerControl playerControls;

    private void Awake()
    {
        playerControls = new PlayerControl();

        playerControls.Player.Pause.performed += _ => TogglePause();
   
    }

    private void OnEnable()
    {
        playerControls.Player.Enable();
    }

    private void OnDisable()
    {
        playerControls.Player.Disable();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenuUI.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;

        

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

    public void RestartGame()
    {
        // Resume time before reloading the scene
        Time.timeScale = 1;

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }

}