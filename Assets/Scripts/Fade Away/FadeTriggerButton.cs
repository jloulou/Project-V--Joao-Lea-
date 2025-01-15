using UnityEngine;
using UnityEngine.UI;

public class FixedFadeTriggerButton : MonoBehaviour
{
    private void Start()
    {
        // Get the button component and add a listener
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(TriggerAllFades);
        }
    }

    private void TriggerAllFades()
    {
        // Find all objects with FixedFadeAndDestroy component
        FixedFadeAndDestroy[] fadeObjects = FindObjectsOfType<FixedFadeAndDestroy>();
        FixedUIFadeAndDestroy[] uiFadeObjects = FindObjectsOfType<FixedUIFadeAndDestroy>();

        // Trigger immediate fade for each 3D object
        foreach (FixedFadeAndDestroy fade in fadeObjects)
        {
            fade.TriggerFadeImmediate();
        }

        // Trigger immediate fade for each UI object
        foreach (FixedUIFadeAndDestroy fade in uiFadeObjects)
        {
            fade.TriggerFadeImmediate();
        }
    }

    private void OnDestroy()
    {
        // Clean up the listener when the object is destroyed
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.RemoveListener(TriggerAllFades);
        }
    }
}