using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [SerializeField, Tooltip("The maximum health of the object.")]
    private float _maxHealth;

    [SerializeField, Tooltip("Emitted once at death.")]
    public UnityEvent Died = new UnityEvent();

    [SerializeField, Tooltip("Emitted once every damage/heal dealt.")]
    public UnityEvent<float> HealthChanged = new UnityEvent<float>();

    /// <summary>
    /// The current health of the object.
    /// </summary>
    public float CurrentHealth { get; private set; }

    private void Start()
    {
        if (_maxHealth <= 0)
        {
            Debug.LogError("Max health needs to be greater than zero.");
            return;
        }

        CurrentHealth = _maxHealth;
    }

    /// <summary>
    /// Deals damage to the component, then checks if it's dead.
    /// </summary>
    /// <param name="damage">Damage value</param>
    public void Damage(float damage)
    {
        CurrentHealth -= damage;

        HealthChanged.Invoke(CurrentHealth);

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Died.Invoke();
        }
    }

    /// <summary>
    /// Heals the component.
    /// </summary>
    /// <param name="heal">Heal value.</param>
    public void Heal(float heal)
    {
        CurrentHealth = Mathf.Max(CurrentHealth + heal, _maxHealth);

        HealthChanged.Invoke(CurrentHealth);
    }

    /// <summary>
    /// Returns the current health as a percentage of the max health. Useful for progress bar UI.
    /// </summary>
    /// <returns>Health value out of 100%</returns>
    public float GetHealthPorcentage()
    {
        if (_maxHealth <= 0)
        {
            return 0;
        }

        return Mathf.Min(CurrentHealth / _maxHealth, 1);
    }
}
