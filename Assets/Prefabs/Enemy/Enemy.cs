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
        _healthPorcentage.text = (_healthComponent.GetHealthPorcentage() * 100).ToString() + "%";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnHealthChanged(float health)
    {
        _healthPorcentage.text = (_healthComponent.GetHealthPorcentage() * 100).ToString() + "%";
    }
}
