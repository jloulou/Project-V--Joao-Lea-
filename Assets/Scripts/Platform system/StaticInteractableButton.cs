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

        // Store initial transform values
        localPosition = transform.localPosition;
        localRotation = transform.localRotation;

        // Configure the rigidbody if it exists
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    private void Update()
    {
        // Keep position and rotation fixed
        transform.localPosition = localPosition;
        transform.localRotation = localRotation;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        onButtonSelected?.Invoke();
    }
}