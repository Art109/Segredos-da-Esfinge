using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public Animator animator;

    public void fadeToLevel ()
    {
        animator.SetTrigger("FadeOut");
    }

    // Inicia o jogo na cena 1
    public void StartGame()
    {
        SceneManager.LoadScene(0);
    }

    // Opções, se for ter
    public void OpenOptions()
    {
        Debug.Log("Options menu opened");
    }

    // Sai do jogo
    public void QuitGame()
    {
        Debug.Log("Game is exiting");
        Application.Quit();
    }
}
