using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MovementComponent : MonoBehaviour
{

    [SerializeField, Tooltip("The maximum velocity the object can achieve.")]
    private float _targetVelocity;

    [SerializeField, Tooltip("Acceleration for the given object. Defaults to 1")]
    private float _acceleration = 1.0f;

    [SerializeField, Tooltip("The layer mask belonging to the ground.")]
    private LayerMask _groundLayer;

    [SerializeField, Tooltip("The character's height.")]
    private float _height = 1f;

    [NonSerialized]
    public float timeWalking;

    private Rigidbody _rigidbody;
    private CharacterController _characterController;

    [NonSerialized]
    public Vector3 currentVelocity = Vector3.zero;

    private bool isPlayer;

    private void Start()
    {
        // Freezes the rotation of the object so it doesn't fall
        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody == null)
        {
            Debug.LogError("Rigidbody not found!");
            return;
        } 
        else
        {
            _rigidbody.freezeRotation = true;
        }

        // Gets the CharacterController component from the object, if it exists
        isPlayer = TryGetComponent<CharacterController>(out _characterController);
    }

    /// <summary>
    /// Moves the given object in a specified direction.
    /// </summary>
    /// <param name="objectToMove">The GameObject to be moved</param>
    /// <param name="direction">The Vector3 representing the direction</param>
    /// <param name="forward">The vector representing the forward direction of movement, useful if the component belongs to the player and its rotation changes constantly due to the camera</param>
    public void Move(GameObject objectToMove, Vector3 direction, Quaternion forward = default)
    {
        if (objectToMove == null)
        {
            Debug.LogError("Object cannot be null!");
            return;
        }

        if (direction == Vector3.zero)
        {
            // Resets the walking time
            timeWalking = 0;

            currentVelocity = Vector3.zero;

            return;
        }

        // Updates the time the player has been walking
        timeWalking += Time.deltaTime;

        // Transforms the movement vector based on its direction (if it exists) to ensure the movement is always aligned with the camera
        Vector3 movementDirection = forward * direction;

        // Calculates the desired movement velocity
        Vector3 targetVelocity = movementDirection * _targetVelocity;

        // Calculates the acceleration value to be used in the Lerp
        float accelerationValue = 1 - Mathf.Exp(-_acceleration * Time.deltaTime);

        // Calculates the Lerp, which is essentially a way to smooth movement with acceleration
        Vector3 lerpedDirection = Vector3.Lerp(currentVelocity, targetVelocity, accelerationValue);

        // Moves the object
        // If it has the CharacterController component, use it for movement; otherwise, use the rigidbody
        if (isPlayer)
        {
            _characterController.Move(lerpedDirection * Time.deltaTime);
        } 
        else
        {
            // Adds force to the object based on the player's acceleration; the airMultiplier makes the player move slower in the air and sets ForceMode.Force for consistent force application
            _rigidbody.AddForce(lerpedDirection.normalized * _targetVelocity * 10f, ForceMode.Force);
        }

        // Updates the local variable with the most recent velocity of the object
        currentVelocity = targetVelocity;
    }

    /// <summary>
    /// Checks if the player is grounded. Requires player height and layer mask.
    /// </summary>
    /// <returns>True if grounded.</returns>
    public bool Grounded()
    {
        return Physics.Raycast(gameObject.transform.position, Vector3.down, _height * 0.5f + 0.3f, _groundLayer);
    }
}
