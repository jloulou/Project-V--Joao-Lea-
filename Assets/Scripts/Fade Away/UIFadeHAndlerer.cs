using UnityEngine;
using UnityEngine.UI;

public class FixedUIFadeHandler : MonoBehaviour
{
    // Reference the objects you want to fade
    public GameObject[] objectsToFade;

    public void StartFadeOnObjects()
    {
        foreach (GameObject obj in objectsToFade)
        {
            var fadeScript = obj.GetComponent<FixedFadeAndDestroy>();
            if (fadeScript != null)
            {
                fadeScript.TriggerFade();
            }
        }
    }
}