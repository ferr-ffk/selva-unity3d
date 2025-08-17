using System;
using System.Linq;
using UnityEditor.SearchService;
using UnityEngine;

/// <summary>
/// Uses canvas to manage the UI elements in the game.
/// </summary>
public class MenuManager : MonoBehaviour
{
    [Header("Canvas References")]
    [SerializeField, Tooltip("The canvas that will be used to start the game. If not set, the first element in the canvasList will be used.")]
    private Canvas _startingCanvas;

    [SerializeField, Tooltip("List of all canvases in this context. The first element will be used for the starting element if it's not set.")]
    private Canvas[] _canvasList;

    private Canvas _currentCanvas;

    private float _previousTimeScale = 1f;

    private void Start()
    {
        if (_canvasList == null || _canvasList.Length == 0 && _startingCanvas == null)
        {
            Debug.LogError("Canvas list is empty or not set. Please assign canvases in the inspector or a starting canvas.");
            return;
        }

        if (_startingCanvas == null && _canvasList.Length > 0)
        {
            _startingCanvas = _canvasList[0];

            return;
        }

        // Clears all existing canvases
        foreach (var canvas in _canvasList)
        {
            canvas.enabled = false;
        }

        // Sets the starting canvas as the current canvas
        _currentCanvas = _startingCanvas;
        _currentCanvas.enabled = true;
    }

    /// <summary>
    /// Switches to a specified canvas.
    /// </summary>
    /// <param name="canvas"></param>
    public void SwitchTo(Canvas canvas)
    {
        if (canvas == null)
        {
            Debug.LogError("Cannot switch to a null canvas.");
            return;
        }

        if (!_canvasList.Contains(canvas))
        {
            Debug.LogError($"Canvas {canvas.name} is not part of the canvasList.");
            return;
        }

        _currentCanvas.enabled = false;

        _currentCanvas = canvas;

        _currentCanvas.enabled = true;

    }

    /// <summary>
    /// Pauses or resumes the game by setting the time scale.
    /// </summary>
    /// <param name="pause">The value for the current paused game state</param>
    public void PauseGame(bool pause)
    {
        if (pause)
        {
            _previousTimeScale = Time.timeScale;
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = _previousTimeScale;
        }
    }

    /// <summary>
    /// Loads a scene by its name.
    /// </summary>
    /// <param name="scene">The name of the scene</param>
    public void LoadScene(string scene)
    {
        if (scene == null)
        {
            Debug.LogError("Cannot load a null scene.");
            return;
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }

    /// <summary>
    /// Quits the game and stops playing in the editor if applicable.
    /// </summary>
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
