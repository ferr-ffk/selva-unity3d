using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SingleQuickTimeEventHandler : MonoBehaviour
{
    public QuickTimeEventComponent quickTimeEventComponent;

    public TextMeshProUGUI textMesh;

    public TextMeshProUGUI description;

    public InputActionReference eventStart;

    private void Start()
    {
        textMesh.text = quickTimeEventComponent.TimeRemaining.ToString();

        description.text = "aperta 'c' p começar";
    }

    private void Update()
    {
        if (eventStart.action.triggered)
        {
            quickTimeEventComponent.StartEvent();
        }

        textMesh.text = quickTimeEventComponent.TimeRemaining.ToString();
    }

    public void OnEventStart()
    {
        Debug.Log("Event started");

        textMesh.text = quickTimeEventComponent.TimeRemaining.ToString();

        description.text = quickTimeEventComponent.GetEventDescription();
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
