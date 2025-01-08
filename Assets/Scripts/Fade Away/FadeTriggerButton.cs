using UnityEngine;
using UnityEngine.UI;

public class FadeTriggerButton : MonoBehaviour
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
        // Find all objects with FadeAndDestroy component
        FadeAndDestroy[] fadeObjects = FindObjectsOfType<FadeAndDestroy>();
        UIFadeAndDestroy[] uiFadeObjects = FindObjectsOfType<UIFadeAndDestroy>();

        // Trigger fade for each 3D object
        foreach (FadeAndDestroy fade in fadeObjects)
        {
            fade.delayBeforeFade = 0f;  // Skip the delay timer
            fade.TriggerFade();
        }

        // Trigger fade for each UI object
        foreach (UIFadeAndDestroy fade in uiFadeObjects)
        {
            fade.delayBeforeFade = 0f;  // Skip the delay timer
            fade.TriggerFade();
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