using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LaunchComponent : MonoBehaviour
{
    [Header("Launch Component")]
    [SerializeField, Tooltip("Height of the jump, defaults to 5")]
    private float _jumpHeight = defaultJumpHeight;

    private const float defaultJumpHeight = 5f;

    [SerializeField, Tooltip("Gravity value used for jump, defaults to -9.81")]
    private float _gravity = -Physics.gravity.magnitude;

    [Header("Debug Options")]
    [SerializeField, Tooltip("Debug path for the jump trajectory.")]
    private bool _debugPath;

    [SerializeField, Tooltip("Color of the debug path, defaults to green.")]
    private Color _debugPathColor = Color.green;

    private Rigidbody _rigidBody;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Launches the object in a parabolic trajectory towards the specified target position.
    /// </summary>
    /// <param name="target">The target position to launch the object towards.</param>
    /// <remarks>
    /// This method calculates the necessary initial velocity to reach the target position
    /// considering the specified jump height and gravity. It also ensures the object has a Rigidbody
    /// component and enables gravity for the object during the launch.
    /// </remarks>
    public void LaunchTo(Vector3 target)
    {
        Physics.gravity = Vector3.up * _gravity;

        _rigidBody.useGravity = true;

        _rigidBody.linearVelocity = CalculateLaunchData(target, _jumpHeight).initialVelocity;
    }

    public void LaunchTo(Vector3 target, float jumpHeight)
    {
        _jumpHeight = jumpHeight;

        LaunchTo(target);

        _jumpHeight = defaultJumpHeight;
    }

    /// <summary>
    /// Draws the parabolic path of the object towards the specified target position.
    /// </summary>
    /// <param name="target">The target position to visualize the trajectory towards.</param>
    /// <remarks>
    /// This method calculates the trajectory of the object using the initial velocity and gravity,
    /// then draws a series of lines in the Unity editor to represent the path. The resolution of the
    /// path determines the number of segments used to draw the trajectory.
    /// </remarks>
    public void DrawPath(Vector3 target)
    {
        LaunchData launchData = CalculateLaunchData(target, _jumpHeight);
        Vector3 previousDrawPoint = _rigidBody.position;

        int resolution = 30;
        for (int i = 1; i <= resolution; i++)
        {
            float simulationTime = i / (float) resolution * launchData.timeToTarget;

            Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * _gravity * simulationTime * simulationTime / 2f;
            Vector3 drawPoint = _rigidBody.position + displacement;

            Debug.DrawLine(previousDrawPoint, drawPoint, _debugPathColor);

            previousDrawPoint = drawPoint;
        }
    }

    /// <summary>
    /// Draws the parabolic path of the object towards the specified target position with a custom jump height. See <see cref="DrawPath(Vector3)"/>
    /// </summary>
    /// <param name="target"></param>
    /// <param name="jumpHeight"></param>
    public void DrawPath(Vector3 target, float jumpHeight)
    {
        _jumpHeight = jumpHeight;

        DrawPath(target);

        _jumpHeight = defaultJumpHeight;
    }

    /// <summary>
    /// Calculates the launch data required to reach a target position.
    /// </summary>
    /// <param name="target">The target position to reach.</param>
    /// <returns>A LaunchData object containing the initial velocity and time to target.</returns>
    /// <remarks>
    /// This method computes the initial velocity needed to launch an object from its current position
    /// to the specified target position, considering the jump height and gravity. It also calculates
    /// the total time required to reach the target.
    /// </remarks>
    private LaunchData CalculateLaunchData(Vector3 target, float height)
    {
        float displacementY = target.y - _rigidBody.position.y;

        Vector3 displacementXZ = new Vector3(target.x - _rigidBody.position.x, 0, target.z - _rigidBody.position.z);

        float time = Mathf.Sqrt(-2 * height / _gravity) + Mathf.Sqrt(2 * (displacementY - height) / _gravity);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * _gravity * height);
        Vector3 velocityXZ = displacementXZ / time;

        return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(_gravity), time);
    }

    struct LaunchData
    {
        public readonly Vector3 initialVelocity;
        public readonly float timeToTarget;

        public LaunchData(Vector3 initialVelocity, float timeToTarget)
        {
            this.initialVelocity = initialVelocity;
            this.timeToTarget = timeToTarget;
        }

    }
}
