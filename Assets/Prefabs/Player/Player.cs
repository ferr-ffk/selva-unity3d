using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls the player character, handling movement, launching, and interactions with enemies.
/// </summary>
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
    private Vector3 _jumpTarget = Vector3.zero;

    [Tooltip("ID of the jump target, used for debugging.")]
    private int _jumpTargetId = 0;

    /// <summary>
    /// Called once before the first execution of Update after the MonoBehaviour is created.
    /// </summary>
    void Start()
    {
    }

    /// <summary>
    /// Called once per frame. Handles player movement, rotation, and launching logic.
    /// </summary>
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

        // If the jump target is null, skip the jumping logic
        if (_jumpTarget != Vector3.zero)
        {
            // Automatically draw the debug path
            _launchComponent.DrawPath(_jumpTarget);

            // Launch player to target once triggered and check if it's grounded
            if (_jumpReference.action.triggered && _movementComponent.Grounded())
            {
                _launchComponent.LaunchTo(_jumpTarget);
            }
        }
    }

    /// <summary>
    /// Called when a collider enters the player's launch range.
    /// Sets the jump target to the enemy's position if an enemy is detected.
    /// </summary>
    /// <param name="other">The collider that was entered.</param>
    public void OnLaunchRangeColliderEnter(Collider other)
    {
        // Check if the collided object is an enemy
        if (other.TryGetComponent(out Enemy enemy))
        {
            Debug.Log("Enemy detected: " + enemy.name);

            _jumpTarget = other.transform.position;
            _jumpTargetId = other.GetInstanceID();
        }
    }

    /// <summary>
    /// Called when a collider enters the player's attack range.
    /// Sets the jump target to the enemy's position if an enemy is detected.
    /// </summary>
    /// <param name="other">The collider that was entered.</param>
    public void OnAttackRangeColliderEnter(Collider other)
    {
        // Check if the collided object is an enemy
        if (other.TryGetComponent(out Enemy enemy))
        {
            Debug.Log("Enemy in attack range: " + enemy.name);

            _jumpTarget = other.transform.position;
            _jumpTargetId = other.GetInstanceID();
        }
    }

    /// <summary>
    /// Called when a collider exits the player's launch range.
    /// Resets the jump target if the exited collider matches the current target.
    /// </summary>
    /// <param name="other">The collider that was exited.</param>
    public void OnLauncheRangeColliderExit(Collider other)
    {
        // Check if the exited object is the same as the current target
        if (_jumpTargetId == other.GetInstanceID())
        {
            Debug.Log("Enemy that entered has now exited launch range");

            _jumpTarget = Vector3.zero;
            _jumpTargetId = 0;
        }
    }

    /// <summary>
    /// Called when a collider exits the player's attack range.
    /// Logs the exit if the exited collider matches the current target.
    /// </summary>
    /// <param name="other">The collider that was exited.</param>
    public void OnAttackRangeColliderExit(Collider other)
    {
        // Check if the exited object is the same as the current target
        if (_jumpTargetId == other.GetInstanceID())
        {
            Debug.Log("Enemy that entered has now exited attack range");
        }
    }
}
