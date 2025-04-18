using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private MovementComponent _movementComponent;

    [SerializeField]
    private InputActionReference _movementReference;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movementInput = _movementReference.action.ReadValue<Vector2>();

        Vector3 movementDirection = new(movementInput.x * Time.deltaTime, 0, movementInput.y * Time.deltaTime);

        _movementComponent.Move(gameObject, movementDirection);
    }
}
