using UnityEngine;
using UnityEngine.SceneManagement;

public class Titlepage : MonoBehaviour
{
    // This method will be called when the start button is clicked.
    public static void StartGame()
    {
        // Load the scene called "Level1"
        SceneManager.LoadScene("Level 1");
    }
    public void QuitGame()
    {
     
        Application.Quit();
    }
    public static void MainMenu()
    {
        // Load the scene called "Level1"
        SceneManager.LoadScene("Title");
    }
    public static void RestartLevel()
    {
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        StartGame();
    }

    public static void Lost()
    {
        Debug.Log("Lost");
        SceneManager.LoadScene("Lost");
    }
}
