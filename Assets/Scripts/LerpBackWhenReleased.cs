using UnityEngine;

public class LerpBackWhenReleased : MonoBehaviour
{
    private Vector3 InitialPosition;
    private Quaternion InitialRotation;

    private Vector3 LetGoStartPosition;
    private Quaternion LetGoEndRotation;

    [SerializeField] private float LerpTime;
    private float Timer = 0;

    private bool isGrabbed;
    public bool IsGrabbed
    {
        get
        {
            return isGrabbed;
        }
        set
        {
            isGrabbed = value;

            if (!isGrabbed)
            {
                Timer = 0;

                LetGoStartPosition = transform.position;
                LetGoEndRotation = transform.rotation;
            }
        }
    }

    private void Start()
    {
        InitialPosition = transform.position;
        InitialRotation = transform.rotation;

        LetGoStartPosition = transform.position;
        LetGoEndRotation = transform.rotation;
    }

    // Update is called once per frame
    private void Update()
    {
        if (IsGrabbed)
            return;

        Timer = Mathf.Clamp01(Timer + Time.deltaTime / LerpTime);

        transform.position = Vector3.Lerp(LetGoStartPosition, InitialPosition, Timer);
        transform.rotation = Quaternion.Lerp(LetGoEndRotation, InitialRotation, Timer);
    }
}