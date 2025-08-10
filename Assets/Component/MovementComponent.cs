using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovementComponent : MonoBehaviour
{

    [SerializeField, Tooltip("The maximum velocity the object can achieve.")]
    private float _targetVelocity;

    [SerializeField, Tooltip("The layer mask belonging to the ground.")]
    private LayerMask _groundLayer;

    [SerializeField, Tooltip("The character's height.")]
    private float _height = 1f;

    [SerializeField, Tooltip("Linear damping when component is not grounded")]
    private float _airLinearDamping = 0.1f;

    [NonSerialized]
    public float timeWalking;

    private Rigidbody _rigidbody;

    [NonSerialized]
    public Vector3 currentVelocity = Vector3.zero;

    private float defaultLinearDamping = 0.25f;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        // Freezes the rotation of the object so it doesn't fall
        _rigidbody.freezeRotation = true;

        defaultLinearDamping = _rigidbody.linearDamping;
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

        // Moves the object
        // Adds force to the object based on the player's acceleration; the airMultiplier makes the player move slower in the air and sets ForceMode.Force for consistent force application
        _rigidbody.AddForce(targetVelocity.normalized * _targetVelocity * 10f, ForceMode.Force);

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

    private void Update()
    {
        if (Grounded())
        {
            _rigidbody.linearDamping = defaultLinearDamping; // Applies default linear damping when grounded
        }
        else
        {
            _rigidbody.linearDamping = _airLinearDamping; // Applies low linear damping when not grounded    
        }
    }

    public void SetTargetVelocity(float newTargetVelocity)
    {
        _targetVelocity = newTargetVelocity;
    }

    public float GetTargetVelocity()
    {
        return _targetVelocity;
    }
}
