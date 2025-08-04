using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ContinuousQuickTimeEventHandler : MonoBehaviour
{
    public QuickTimeEventComponent quickTimeEventComponent;

    public TextMeshProUGUI textMesh;

    public TextMeshProUGUI description;

    public InputActionReference eventStart;

    public InputActionReference eventTrigger;

    private void Start()
    {
        textMesh.text = quickTimeEventComponent.TimeRemaining.ToString();

        description.text = "aperta aqui p começar";
    }

    private void Update()
    {
        if (eventStart.action.triggered)
        {
            quickTimeEventComponent.StartEvent();
        }

        if (eventTrigger.action.triggered)
        {
            quickTimeEventComponent.Trigger();
        }

        textMesh.text = quickTimeEventComponent.TimeRemaining.ToString();
    }

    public void OnEventStart()
    {
        Debug.Log("Event started");

        textMesh.text = quickTimeEventComponent.TimeRemaining.ToString();

        description.text = quickTimeEventComponent.GetEventDescription();
    }

    public void OnEventTriggered()
    {
        Debug.Log("Event triggered. Current Count: " + quickTimeEventComponent.GetCurrentCount());
        description.text = "evento em andamento...";
    }

    public void OnEventSuccess()
    {
        description.text = "suceso :)";

        Debug.Log("Event succeeded");
    }

    public void OnEventFailure()
    {
        description.text = "faliou :(";

        Debug.Log("Event failed");
    }
}
