using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [SerializeField, Tooltip("The maximum health of the object.")]
    private float _maxHealth;

    [SerializeField]
    public UnityEvent Died = new UnityEvent();

    public float CurrentHealth { get; private set; }

    private void Start()
    {
        if (_maxHealth <= 0)
        {
            Debug.LogError("Max health needs to be set.");
            return;
        }

        CurrentHealth = _maxHealth;
    }

    public void Damage(float damage)
    {
        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Died.Invoke();
        }
    }
}
