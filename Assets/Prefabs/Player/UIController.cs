using UnityEngine;

/// <summary>
/// UIController is responsible for managing the visibility of UI buttons in the game.
/// </summary>
public class UIController : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField, Tooltip("Reference to the canvas object containing the UI buttons.")]
    private Canvas _canvasReference;

    public void ShowUI()
    {
        _canvasReference.enabled = true;
    }

    public void HideUI()
    {
        _canvasReference.enabled = false;
    }
}
