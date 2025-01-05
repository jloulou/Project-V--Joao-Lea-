using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.Events;

public class StaticInteractableButton : XRBaseInteractable
{
    [SerializeField]
    private UnityEvent onButtonSelected;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Rigidbody rb;

    protected override void Awake()
    {
        base.Awake();

        // Store original transform values
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        // Get and configure components
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    private void Update()
    {
        // Ensure the button stays in its original position
        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        // Invoke the Unity Event
        onButtonSelected?.Invoke();
        // Trigger your UI keyboard here
        ShowUIKeyboard();
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        // Handle selection exit if needed
    }

    private void ShowUIKeyboard()
    {
        // Add your UI keyboard activation logic here
        // Example:
        // UIKeyboard.Instance.Show();
    }
}