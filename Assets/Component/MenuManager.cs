using System;
using System.Linq;
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
            canvas.gameObject.SetActive(false);
        }

        // Sets the starting canvas as the current canvas
        _currentCanvas = _startingCanvas;
        _currentCanvas.gameObject.SetActive(true);
    }

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

        _currentCanvas?.gameObject.SetActive(false);

        canvas.gameObject.SetActive(true);
    }
}
