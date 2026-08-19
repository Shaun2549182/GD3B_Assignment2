using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadLinear()
    {
        SceneManager.LoadScene("Linear");
    }

    public void LoadExponential()
    {
        SceneManager.LoadScene("Exponential");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}