using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void LoadMainScene()
    {
        // Load the main scene of the game
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
       // Quit the game application
        Application.Quit();

        // If running in the editor, stop playing
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
