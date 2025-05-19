using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
///     Component for handling quick time events (QTEs) in Unity.
///     <para>
///         This component enables the creation and management of time-based input challenges, such as requiring the player
///         to press a specific key within a time limit. It supports both single-action and continuous-action QTEs,
///         triggering UnityEvents for success or failure.
///     </para>
/// </summary>
/// <remarks>
/// <para>
///     Attach this component to a GameObject to define a quick time event for gameplay. Configure its parameters in the
///     Inspector, including the required input action, event type, duration, and response events.
/// </para>
/// <para>
///     The component supports two event types:
///     <list type="bullet">
///         <item>
///             <term>Single</term>
///             <description>The player must press the specified key once within the allotted time.</description>
///         </item>
///         <item>
///             <term>Continuous</term>
///             <description>The player must continuously or repeatedly press the specified key for the duration (not implemented).</description>
///         </item>
///     </list>
/// </para>
/// </remarks>
/// <example>
/// // Example usage in Unity:
/// // 1. Attach QuickTimeEventComponent to a GameObject.
/// // 2. Set the InputActionReference to the desired key or button.
/// // 3. Configure EventDuration, EventType, and description.
/// // 4. Assign UnityEvents for start, success, and failure responses.
/// // 5. Call StartEvent() to initiate the QTE.
/// </example>
/// <seealso cref="UnityEvent"/>
public class QuickTimeEventComponent : MonoBehaviour
{
    /// <summary>
    /// Enumeration for different types of quick time events.
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <item>
    /// <term>Single</term>
    /// <description>Represents a quick time event that requires a single key press.</description>
    /// </item>
    /// <item>
    /// <term>Continuous</term>
    /// <description>Represents a quick time event that requires continuous or repeated key presses over a duration.</description>
    /// </item>
    /// </list>
    /// </remarks>
    enum EventType
    {
        /// <summary>
        /// Represents a quick time event that requires a single key press.
        /// </summary>
        Single,
        /// <summary>
        /// Represents a quick time event that requires continuous or repeated key presses over a duration.
        /// </summary>
        Continuous,
    }

    [Header("Quick Time Event Settings")]
    [SerializeField, Tooltip("The duration of the quick time event in seconds.")]
    public float EventDuration = 0;

    [SerializeField, Tooltip("The key to be pressed during the quick time event.")]
    private InputActionReference _inputActionReference;

    [SerializeField, Tooltip("The type of quick time event.")]
    private EventType _eventType;

    [SerializeField, Tooltip("Event description to be displayed.")]
    private string _eventDescription = "";

    [Header("Event Responses")]
    [SerializeField, Tooltip("Action triggered on event start.")]
    private UnityEvent _eventStart = new();

    [SerializeField, Tooltip("Action triggered on event success.")]
    private UnityEvent _eventSuccess = new();

    [SerializeField, Tooltip("Action triggered on event failure.")]
    private UnityEvent _eventFailure = new();

    [Tooltip("Indicates whether the event is currently running.")]
    public bool Running { get; private set; }

    [SerializeField, Tooltip("Time remaining for the event in seconds.")]
    public float TimeRemaining { get; private set; } = 0;

    /// <summary>
    /// Coroutine instance for the countdown timer. Necessary for stopping it.
    /// </summary>
    private IEnumerator countDownInstance;

    private void Start()
    {
        // Sets the time remaining to the event duration
        TimeRemaining = EventDuration;
    }

    private void Update()
    {
        if (!Running)
            return;

        TimeRemaining = Mathf.Max(TimeRemaining - Time.deltaTime, 0);

        if (_eventType == EventType.Single)
        {
            // Handle Single Type Event
            // Check if the key is pressed during the time frame
            CheckSingleEvent();
        }
        else if (_eventType == EventType.Continuous)
        {
            // Handle Continuous Type Event

            CheckContinuousEvent();
        }
    }

    private void CheckContinuousEvent()
    {
        throw new NotImplementedException();
    }

    private void CheckSingleEvent()
    {
        // Check if the key is pressed during the time frame
        if (TimeRemaining > 0 && _inputActionReference.action.triggered)
        {
            // Invokes sucess event
            _eventSuccess?.Invoke();

            // Interrupts the timer
            StopCoroutine(countDownInstance);

            // Resets the time remaining
            Running = false;
        }
    }

    public void StartEvent()
    {
        if (Running)
        {
            Debug.LogWarning($"Event '{_eventDescription}' already running.");

            // Resets timer, if it was running
            StopCoroutine(countDownInstance);
        }

        // Invokes event start
        _eventStart?.Invoke();

        // Sets the time remaining
        TimeRemaining = EventDuration;

        // Starts timer with duration of EventDuration
        // Sets member instance value so that can be stopped later
        countDownInstance = StartCountDown();

        StartCoroutine(countDownInstance);

        // Sets the event as running
        Running = true;
    }

    private IEnumerator StartCountDown()
    {
        // Start timer
        // It freezes code running in this block, so that it can be interrupted
        yield return new WaitForSeconds(EventDuration);

        yield return null;

        // If it's not interrupted by the player, fails the event
        StopCoroutine(countDownInstance);
        _eventFailure.Invoke();
        Running = false;
    }

    public string GetEventDescription()
    {
        return _eventDescription;
    }
}
