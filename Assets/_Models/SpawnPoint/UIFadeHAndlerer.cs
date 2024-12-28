using UnityEngine;
using UnityEngine.UI;

public class UIFadeHandler : MonoBehaviour
{
    // Reference the objects you want to fade
    public GameObject[] objectsToFade;

    public void StartFadeOnObjects()
    {
        foreach (GameObject obj in objectsToFade)
        {
            var fadeScript = obj.GetComponent<FadeAndDestroy>();
            if (fadeScript != null)
            {
                fadeScript.delayBeforeFade = 0f; // No delay
                fadeScript.TriggerFade();
            }
        }
    }
}