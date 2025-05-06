using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Movement and Jump Components
    [Header("Movement and Jump Components")]
    [Tooltip("Component responsible for player movement.")]
    [SerializeField]
    private MovementComponent _movementComponent;

    [SerializeField, Tooltip("Component responsible for launching mechanics")]
    private LaunchComponent _launchComponent;

    [Tooltip("Rigidbody of the player, used for physics-based interactions.")]
    [SerializeField]
    private Rigidbody _rigidBody;

    // Input References
    [Header("Input References")]
    [Tooltip("Input action for player movement.")]
    [SerializeField]
    private InputActionReference _movementReference;

    [Tooltip("Input action for camera look.")]
    [SerializeField]
    private InputActionReference _lookReference;

    [Tooltip("Input action for jumping.")]
    [SerializeField]
    private InputActionReference _jumpReference;

    // Scene Object References
    [Header("Scene Object References")]
    [Tooltip("Reference to the player's camera.")]
    [SerializeField]
    private GameObject _camera;

    [Tooltip("Reference to the player's armature for rotation.")]
    [SerializeField]
    private GameObject _armature;

    [SerializeField, Tooltip("Reference to the target for jumping.")]
    private Vector3 _jumpTarget;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Obtém o vetor 2D de direção a partir do input
        Vector2 joystickDirection = _movementReference.action.ReadValue<Vector2>();

        // Cria um vetor 3D para movimentar apenas no x e z (sendo Y a vertical)
        Vector3 direction = new(joystickDirection.x, 0, joystickDirection.y);

        // Utiliza a rotação que a câmera está apontando, e usa para que o jogador se movimente de acordo com ela
        Quaternion cameraRotation = Quaternion.Euler(0, _camera.transform.eulerAngles.y, 0);

        bool movement = direction != Vector3.zero;

        // If there is movement, rotate the player based on the camera rotation value
        if (movement)
        {
            Quaternion desiredRotation = Quaternion.LookRotation(direction) * cameraRotation;

            _armature.transform.rotation = Quaternion.Slerp(_armature.transform.rotation, desiredRotation, Time.deltaTime * 8);
        }

        // Move o Jogador
        _movementComponent.Move(gameObject, direction, cameraRotation);

        // Launch player to target once triggered and check if it's grounded
        if (_jumpReference.action.triggered && _movementComponent.Grounded())
        {
            _launchComponent.LaunchTo(_jumpTarget);
        }

        _launchComponent.DrawPath(_jumpTarget);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other);
    }
}
