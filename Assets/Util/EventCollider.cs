using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Emits events when the collider enters, stays, or exits another collider.
/// This is a workaround for the fact that a rigid body's on-collision/trigger events are not publicly accessible by code.
/// </summary>
public class EventCollider : MonoBehaviour
{
    /// <summary>
    /// Emitted once when the collider enters another collider.
    /// </summary>
    [SerializeField, Tooltip("Emitted once when the collider enters another collider.")]
    private UnityEvent<Collider> _onCollisionEnter;

    /// <summary>
    /// Emitted every frame while the collider stays in contact with another collider.
    /// </summary>
    [SerializeField, Tooltip("Emitted every frame while the collider stays in contact with another collider.")]
    private UnityEvent<Collider> _onCollisionStay;

    /// <summary>
    /// Emitted once when the collider exits another collider.
    /// </summary>
    [SerializeField, Tooltip("Emitted once when the collider exits another collider.")]
    private UnityEvent<Collider> _onCollisionExit;

    /// <summary>
    /// Called by Unity when another collider enters the trigger.
    /// Invokes the <see cref="_onCollisionEnter"/> event.
    /// </summary>
    /// <param name="collision">The other collider involved in this collision.</param>
    private void OnTriggerEnter(Collider collision)
    {
        _onCollisionEnter.Invoke(collision);
    }

    /// <summary>
    /// Called by Unity every frame another collider stays within the trigger.
    /// Invokes the <see cref="_onCollisionStay"/> event.
    /// </summary>
    /// <param name="collision">The other collider involved in this collision.</param>
    private void OnTriggerStay(Collider collision)
    {
        _onCollisionStay.Invoke(collision);
    }

    /// <summary>
    /// Called by Unity when another collider exits the trigger.
    /// Invokes the <see cref="_onCollisionExit"/> event.
    /// </summary>
    /// <param name="collision">The other collider involved in this collision.</param>
    private void OnTriggerExit(Collider collision)
    {
        _onCollisionExit.Invoke(collision);
    }
}
