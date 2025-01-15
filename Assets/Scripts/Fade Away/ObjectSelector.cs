using UnityEngine;

public class FixedObjectSelector : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                var fadeScript = hit.collider.GetComponent<FixedFadeAndDestroy>();
                if (fadeScript != null)
                {
                    fadeScript.OnObjectSelected();
                }
            }
        }
    }
}