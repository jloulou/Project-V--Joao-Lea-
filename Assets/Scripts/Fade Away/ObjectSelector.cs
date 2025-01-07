using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                var fadeScript = hit.collider.GetComponent<FadeAndDestroy>();
                if (fadeScript != null)
                {
                    // Skip timer, start fade immediately
                    fadeScript.delayBeforeFade = 0f;
                    fadeScript.OnObjectSelected();
                }
            }
        }
    }
}