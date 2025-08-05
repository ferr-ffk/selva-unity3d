using UnityEngine;

/// <summary>
/// UIController is responsible for managing the visibility of UI buttons in the game.
/// </summary>
public class UIController : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField, Tooltip("Reference to the button used for attacking.")]
    private GameObject _attackButton;

    [SerializeField, Tooltip("Reference to the button used for jumping.")]
    private GameObject _launchButton;

    public void ShowAttackButton()
    {
        if (_attackButton != null)
        {
            _attackButton.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Attack button reference is not set.");
        }
    }

    public void HideAttackButton()
    {
        if (_attackButton != null)
        {
            _attackButton.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Attack button reference is not set.");
        }
    }

    public void ShowLaunchButton()
    {
        if (_launchButton != null)
        {
            _launchButton.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Launch button reference is not set.");
        }
    }

    public void HideLaunchButton()
    {
        if (_launchButton != null)
        {
            _launchButton.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Launch button reference is not set.");
        }
    }
}
