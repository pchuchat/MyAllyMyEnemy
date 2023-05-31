using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Creator: PC, Phatchanon Chuchat
// TODO:
//  - MainMenu
//
// 

public class MainMenu : MonoBehaviour
{
    public Animator animator;
    public PlayerInput playerInput;
    public AudioSource source;

    private int action;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    // Start the game
    public void StartGame()
    {
        source.Play();
        playerInput.DeactivateInput();
        action = 1;
        animator.SetTrigger("FadeOut");
    }

    // Quit the game
    public void QuitGame()
    {
        source.Play();
        playerInput.DeactivateInput();
        action = 2;
        animator.SetTrigger("FadeOut");
    }

    // OnFadeComplete method is called when the fade animation is complete
    // It checks the value of the 'action' variable and performs the corresponding action
    public void OnFadeComplete()
    {
        if (action == 1)
        {
            // Load the game scene
            SceneManager.LoadScene(1);
        }
        if (action == 2)
        {
            // Quit the application
            Application.Quit();
        }
    }
}