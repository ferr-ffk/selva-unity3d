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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDied()
    {
        _healthPorcentage.text = "0%";
        
        Destroy(gameObject); // Destroy the enemy immediatly
        Destroy(this, 3f); // Destroy the script after 3 seconds
    }

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
