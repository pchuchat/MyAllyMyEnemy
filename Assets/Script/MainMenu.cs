using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    public Animator animator;
    public PlayerInput playerInput;

    private int action;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    public void StartGame()
    {
        playerInput.DeactivateInput();
        action = 1;
        animator.SetTrigger("FadeOut");
    }

    public void QuitGame()
    {
        playerInput.DeactivateInput();
        action = 2;
        animator.SetTrigger("FadeOut");
    }

    // Start the game -> action = 1
    // Quit the game --> action = 2
    public void OnFadeComplete()
    {
        if (action == 1)
        {
            SceneManager.LoadScene(1);
        }
        if (action == 2)
        {
            Application.Quit();
        }
    }
}