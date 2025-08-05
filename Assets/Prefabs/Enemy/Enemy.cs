using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private TextMesh _healthPorcentage;

    [SerializeField]
    private HealthComponent _healthComponent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _healthPorcentage.text = GetFormattedHealthPercentage();

        _healthComponent.Died.AddListener(OnDied);
        _healthComponent.HealthChanged.AddListener(OnHealthChanged);
    }

    /// <summary>
    /// Called when the enemy dies.
    /// </summary>
    private void OnDied()
    {
        _healthPorcentage.text = "0%";
        
        Destroy(gameObject); // Destroy the enemy immediatly
        Destroy(this, 3f); // Destroy the script after 3 seconds
    }

    /// <summary>
    /// Called when the health of the enemy changes.
    /// </summary>
    /// <param name="health"></param>
    public void OnHealthChanged(float health)
    {
        _healthPorcentage.text = GetFormattedHealthPercentage();
    }

    /// <summary>
    /// Returns the health percentage of the enemy's health as a string.
    /// </summary>
    /// <returns></returns>
    private string GetFormattedHealthPercentage()
    {
        return (_healthComponent.GetHealthPorcentage() * 100).ToString() + "%";
    }
}
