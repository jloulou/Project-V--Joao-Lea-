using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.Events;

public class StaticInteractableButton : XRBaseInteractable
{
    [SerializeField]
    private UnityEvent onButtonSelected;

    private Vector3 localPosition;
    private Quaternion localRotation;
    private Rigidbody rb;

    protected override void Awake()
    {
        base.Awake();

        // Store local transform values
        localPosition = transform.localPosition;
        localRotation = transform.localRotation;

        // Configure the rigidbody
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    private void Update()
    {
        // Maintain local position and rotation relative to parent
        transform.localPosition = localPosition;
        transform.localRotation = localRotation;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        onButtonSelected?.Invoke();
        ShowUIKeyboard();
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
    }

    private void ShowUIKeyboard()
    {
        // Add your UI keyboard activation logic here
    }
}