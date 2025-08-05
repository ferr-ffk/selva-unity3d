using UnityEngine;

public class UIController : MonoBehaviour
{
    [Header("References")]
    [SerializeField, Tooltip("Reference for launch button")]
    private GameObject _launchButtonReference;

    [SerializeField, Tooltip("Reference for attack button")]
    private GameObject _attackButtonReference;

    public void ShowButtons()
    {
        ShowAttackButton();
        ShowLaunchButton();
    }

    public void HideButtons()
    {
        HideAttackButton();
        HideLaunchButton();
    }

    public void ShowLaunchButton()
    {
        _launchButtonReference.SetActive(true);
    }

    public void HideLaunchButton()
    {
        _launchButtonReference.SetActive(false);
    }

    public void ShowAttackButton()
    {
        _attackButtonReference.SetActive(true);
    }

    public void HideAttackButton()
    {
        _attackButtonReference.SetActive(false);
    }
}
