using UnityEngine;

public class LerpBackWhenReleased : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private Vector3 letGoStartPosition;
    private Quaternion letGoEndRotation;

    [SerializeField] private float lerpTime = 1f; // Make sure this is editable in the Inspector
    private float timer = 0;

    private bool isGrabbed;
    public bool IsGrabbed
    {
        get { return isGrabbed; }
        set
        {
            if (isGrabbed == value) return;

            isGrabbed = value;

            if (!isGrabbed)
            {
                timer = 0;
                letGoStartPosition = transform.position;
                letGoEndRotation = transform.rotation;
            }
        }
    }

    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        letGoStartPosition = transform.position;
        letGoEndRotation = transform.rotation;
    }

    private void Update()
    {
        if (IsGrabbed) return;

        // Debugging LerpTime
        Debug.Log($"LerpTime: {lerpTime}");

        timer = Mathf.Clamp01(timer + Time.deltaTime / lerpTime);

        transform.position = Vector3.Lerp(letGoStartPosition, initialPosition, timer);
        transform.rotation = Quaternion.Lerp(letGoEndRotation, initialRotation, timer);
    }
}
