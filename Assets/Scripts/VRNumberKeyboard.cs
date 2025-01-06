using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class VRNumberKeyboard : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public TMP_InputField inputField = null;
    [SerializeField] public Button[] numberButtons = new Button[9];
    [SerializeField] private PlatformTrigger platformTrigger;

    [Header("Settings")]
    [SerializeField] private float inactivityTimeout = 8f; // Time in seconds before keyboard deactivates

    [Header("Events")]
    public UnityEvent onCorrectPassword;
    public UnityEvent onIncorrectPassword;

    private const int MAX_INPUT_LENGTH = 3;
    private const string CORRECT_PASSWORD = "123";
    private float lastInteractionTime;
    private Coroutine inactivityCheckCoroutine;

    private void Awake()
    {
        if (onCorrectPassword == null) onCorrectPassword = new UnityEvent();
        if (onIncorrectPassword == null) onIncorrectPassword = new UnityEvent();

        ValidateReferences();
    }

    private void OnEnable()
    {
        ResetInactivityTimer();
        StartInactivityCheck();
    }

    private void OnDisable()
    {
        if (inactivityCheckCoroutine != null)
        {
            StopCoroutine(inactivityCheckCoroutine);
        }
    }

    private void ValidateReferences()
    {
        if (inputField == null)
            Debug.LogError("Input Field reference is missing! Please assign it in the inspector.");

        if (numberButtons == null || numberButtons.Length != 9)
            Debug.LogError("Please assign all 9 number buttons in the inspector!");

        if (platformTrigger == null)
            Debug.LogError("PlatformTrigger reference is missing! Please assign it in the inspector.");
    }

    private void Start()
    {
        SetupInputField();
        SetupButtons();
        ResetInactivityTimer();
        StartInactivityCheck();
    }

    private void SetupInputField()
    {
        if (inputField != null)
        {
            inputField.text = "";
            inputField.characterLimit = MAX_INPUT_LENGTH;
            inputField.contentType = TMP_InputField.ContentType.IntegerNumber;
            inputField.keyboardType = TouchScreenKeyboardType.NumberPad;
        }
    }

    private void SetupButtons()
    {
        for (int i = 0; i < numberButtons.Length; i++)
        {
            if (numberButtons[i] != null)
            {
                int buttonNumber = i + 1;
                numberButtons[i].onClick.AddListener(() => OnNumberButtonPressed(buttonNumber.ToString()));
            }
        }
    }

    private void ResetInactivityTimer()
    {
        lastInteractionTime = Time.time;
    }

    private void StartInactivityCheck()
    {
        if (inactivityCheckCoroutine != null)
        {
            StopCoroutine(inactivityCheckCoroutine);
        }
        inactivityCheckCoroutine = StartCoroutine(CheckInactivity());
    }

    private IEnumerator CheckInactivity()
    {
        while (true)
        {
            if (Time.time - lastInteractionTime >= inactivityTimeout)
            {
                Debug.Log("Keyboard deactivated due to inactivity");
                gameObject.SetActive(false);
                yield break;
            }
            yield return new WaitForSeconds(0.5f); // Check every half second
        }
    }

    private void OnNumberButtonPressed(string number)
    {
        if (inputField == null) return;

        ResetInactivityTimer();

        if (inputField.text.Length < MAX_INPUT_LENGTH)
        {
            inputField.text += number;

            if (inputField.text.Length == MAX_INPUT_LENGTH)
            {
                ValidatePassword();
            }
        }
    }

    private void ValidatePassword()
    {
        StartCoroutine(HandlePasswordValidation());
    }

    private IEnumerator HandlePasswordValidation()
    {
        yield return new WaitForSeconds(0.5f);

        if (inputField.text != CORRECT_PASSWORD)
        {
            ClearInput();
            onIncorrectPassword.Invoke();
            Debug.Log("Incorrect password entered!");

            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Correct password entered!");
            onCorrectPassword.Invoke();

            if (platformTrigger != null)
            {
                platformTrigger.targetFloor = 4;
                platformTrigger.TriggerPlatform();
            }
        }
    }

    private void ClearInput()
    {
        if (inputField != null)
        {
            inputField.text = "";
        }
    }
}