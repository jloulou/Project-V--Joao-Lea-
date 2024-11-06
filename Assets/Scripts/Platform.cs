using UnityEngine;
using UnityEngine.UI;

public class MoveObjectOnYAxis : MonoBehaviour
{
    public GameObject targetObject; // The object to move
    public Dropdown positionDropdown; // Dropdown UI element

    private void Start()
    {
        // Initialize the dropdown options
        positionDropdown.ClearOptions();
        positionDropdown.AddOptions(new System.Collections.Generic.List<string> { "1", "2", "3" });

        // Add listener to the dropdown to detect selection changes
        positionDropdown.onValueChanged.AddListener(delegate { OnDropdownChanged(); });
    }

    private void OnDropdownChanged()
    {
        // Check the selected option and set the Y position accordingly
        float newYPosition = 0f;

        switch (positionDropdown.value)
        {
            case 0: // Option "1"
                newYPosition = -5f;
                break;
            case 1: // Option "2"
                newYPosition = -10f;
                break;
            case 2: // Option "3"
                newYPosition = -15f;
                break;
        }

        // Move the target object to the new Y position
        if (targetObject != null)
        {
            Vector3 newPosition = targetObject.transform.position;
            newPosition.y = newYPosition;
            targetObject.transform.position = newPosition;
        }
    }
}

