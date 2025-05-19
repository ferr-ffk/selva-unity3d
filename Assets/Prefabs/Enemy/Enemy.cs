using UnityEngine;

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
    }

    // Update is called once per frame
    void Update()
    {
        
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
