using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Controls the player character, handling movement, launching, and interactions with enemies.
/// </summary>
[RequireComponent(typeof(MovementComponent))]
[RequireComponent(typeof(LaunchComponent))]
[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    // Movement and Jump Components

    /// <summary>
    /// Component responsible for player movement.
    /// </summary>
    private MovementComponent _movementComponent;

    /// <summary>
    /// Component responsible for launching mechanics
    /// </summary>
    private LaunchComponent _launchComponent;

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

    [Tooltip("Input action for attacking.")]
    [SerializeField]
    private InputActionReference _attackReference;

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

    [SerializeField, Tooltip("Reference to the target for attacking.")]
    private GameObject _attackTarget;

    [Tooltip("ID of the jump target, used for debugging.")]
    private int _jumpTargetId = 0;

    [Tooltip("ID of the attack target, used for debugging.")]
    private int _attackTargetId = 0;

    [Header("Player Settings")]
    [SerializeField, Tooltip("Factor for slowing down time. Defaults to 0.25f.")]
    private float _slowDownFactor = 0.25f;

    [Header("Quick Time Event Settings")]
    private float adjustedFixedDeltaTime;

    [SerializeField, Tooltip("Reference to the UI handler for the launch button in this component.")]
    private UIController _uiButtons;

    [SerializeField, Tooltip("The QTE event component.")]
    private QuickTimeEventComponent _qteComponent;

    [SerializeField, Tooltip("TextMeshPro component for displaying QTE instructions.")]
    private Text _qteText;

    private bool _lockMovement = false;

    /// <summary>
    /// Called once before the first execution of Update after the MonoBehaviour is created.
    /// </summary>
    void Start()
    {
        _movementComponent = GetComponent<MovementComponent>();
        _launchComponent = GetComponent<LaunchComponent>();

        adjustedFixedDeltaTime = Time.fixedDeltaTime * _slowDownFactor;

        // Disable UI buttons, so that they are enabled on attack/launch range enter
        _uiButtons.HideAttackButton();
        _uiButtons.HideLaunchButton();

        _attackReference.action.performed += ctx => _qteComponent.Trigger();
        _jumpReference.action.performed += ctx => LaunchToTarget();

        if (_qteComponent == null)
        {
            Debug.LogWarning("QuickTimeEventComponent is not assigned. QTE functionality will not work.");
        }
    }

    /// <summary>
    /// Called once per frame. Handles player movement, rotation, and launching logic.
    /// </summary>
    void Update()
    {
        // Handle player movement and rotation
        if (!_lockMovement)
        {
            HandleMovement();
        }
    }

    /// <summary>
    /// Triggered when the attack button is pressed in the UI. Debugs the action and prepares for the quick time event (QTE).
    /// </summary>
    public void OnAttackEventTrigger()
    {
        if (_attackTarget == null)
        {
            Debug.LogWarning("No attack target assigned. Cannot trigger attack event.");
            return;
        }

        // Gets the exact damage needed to deal to the target based on the QTE threshold
        // example: if the threshold is 5, and the target has 100 HP, then each attack will deal 20 damage
        float damage = _attackTarget.GetComponent<HealthComponent>()._maxHealth / _qteComponent.GetThreshold();

        _attackTarget.GetComponent<HealthComponent>().Damage(damage);

        Debug.LogFormat("Attacked enemy! Current count: {0}, Threshold: {1}", _qteComponent.GetCurrentCount(), _qteComponent.GetThreshold());
    }

    /// <summary>
    /// Kills the target specified, runs on quick time event success.
    /// </summary>
    public void OnAttackEventSuccess()
    {
        // Insta kill
        _attackTarget.GetComponent<HealthComponent>().Damage(100000f);

        Debug.Log("Target enemy has died. Resetting UI and tracking variables.");

        _lockMovement = false;

        ResetTarget();
        RevertSlowDownTime();
        _uiButtons.HideAttackButton();
        _uiButtons.HideLaunchButton();
    }

    public void OnAttackEventFailure()
    {
        // Damages the player
        Debug.Log("Damaging the player: -10 HP");

        Debug.Log("Attack event failed. Resetting UI and tracking variables.");

        _lockMovement = false;

        ResetTarget();
        RevertSlowDownTime();

        _uiButtons.HideAttackButton();
        _uiButtons.HideLaunchButton();
    }

    private void ResetTarget()
    {
        _jumpTarget = Vector3.zero;
        _jumpTargetId = 0;

        _attackTarget = null;
        _attackTargetId = 0;
    }

    /// <summary>
    /// Handles player movement based on input from the joystick.
    /// </summary>
    private void HandleMovement()
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
    }

    /// <summary>
    /// Handles the jump logic.
    /// </summary>
    private void LaunchToTarget()
    {
        // If the jump target is null, skip the jumping logic
        if (_jumpTarget != Vector3.zero)
        {
            // Automatically draw the debug path
            _launchComponent.DrawPath(_jumpTarget);

            // Launch player to target once triggered and check if it's grounded
            if (_jumpReference.action.triggered && _movementComponent.Grounded())
            {
                // Makes the player look at the jump target before launching
                _armature.transform.LookAt(_jumpTarget);

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
        // And if doesn't already have a target
        if (other.TryGetComponent(out Enemy enemy) && _jumpTargetId != other.GetInstanceID() && _jumpTargetId == 0)
        {
            Debug.Log("Enemy detected: " + enemy.name);

            _jumpTarget = other.transform.position;
            _jumpTargetId = other.GetInstanceID();

            _uiButtons.ShowLaunchButton();
        }
    }

    /// <summary>
    /// Called when a collider enters the player's attack range.
    /// Sets the jump target to the enemy's position if an enemy is detected.
    /// </summary>
    /// <param name="other">The collider that was entered.</param>
    public void OnAttackRangeColliderEnter(Collider other)
    {
        int objId = other.GetInstanceID();

        // Check if the collided object is an enemy 
        // If doesn't already have an attack target
        // And if it's the launch target too
        if (other.TryGetComponent(out Enemy enemy) && _attackTargetId == 0 && _jumpTargetId == objId)
        {
            Debug.Log("Enemy in attack range: " + enemy.name);

            // Lock movement to prevent player from moving while in QTE
            _lockMovement = true;

            // Config tracking target
            _jumpTarget = other.transform.position;
            _attackTargetId = objId;
            _attackTarget = enemy.gameObject;

            // Enable attack button in UI
            _uiButtons.ShowAttackButton();

            // Slow down time for the QTE
            SlowDownTime();

            _qteComponent.StartEvent();
            _qteText.text = "Press 'Attack' to defeat the enemy!";
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

            _uiButtons.HideLaunchButton();
            _uiButtons.HideAttackButton();

            RevertSlowDownTime();
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

            _attackTargetId = 0;

            _attackTarget = null;

            _uiButtons.HideAttackButton();

            RevertSlowDownTime();
        }
    }
        
    private void SlowDownTime()
    {
        Time.timeScale = _slowDownFactor;
        Time.fixedDeltaTime = adjustedFixedDeltaTime;
    }

    private void RevertSlowDownTime()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;
    }

    private void OnDestroy()
    {
        // Unsubscribe from input actions
        _attackReference.action.performed -= ctx => _qteComponent.Trigger();
        _jumpReference.action.performed -= ctx => LaunchToTarget();
    }
}
