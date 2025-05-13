using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class emits events when the collider enters, stays, or exits another collider. It's done as a workaround for the fact that a rigid body's 
/// on-collision/trigger events are not publicly accessible by code.
/// </summary>
public class EventCollider : MonoBehaviour
{
    [SerializeField, Tooltip("Emitted once when the collider enters another collider.")]
    private UnityEvent<Collider> _onCollisionEnter;

    [SerializeField, Tooltip("Emitted every frame while the collider stays in contact with another collider.")]
    private UnityEvent<Collider> _onCollisionStay;

    [SerializeField, Tooltip("Emitted once when the collider exits another collider.")]
    private UnityEvent<Collider> _onCollisionExit;

    private void OnTriggerEnter(Collider collision)
    {
        _onCollisionEnter.Invoke(collision);
    }
        
    private void OnTriggerStay(Collider collision)
    {
        _onCollisionStay.Invoke(collision);
    }

    private void OnTriggerExit(Collider collision)
    {
        _onCollisionExit.Invoke(collision);
    }
}
