using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class VRNumberKeyboard : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Assign your TMP_InputField here")]
    [SerializeField]
    public TMP_InputField inputField = null;

    [Tooltip("Assign your 9 number buttons here in order (1-9)")]
    [SerializeField]
    public Button[] numberButtons = new Button[9];

    [Header("Timer Settings")]
    [Tooltip("Time in seconds before keyboard deactivates due to inactivity")]
    public float inactivityTimeout = 8f;

    [Header("Events")]
    [Tooltip("Event triggered when correct password is entered")]
    public UnityEvent onCorrectPassword;

    [Tooltip("Event triggered when incorrect password is cleared")]
    public UnityEvent onIncorrectPassword;

    // Settings
    private const int MAX_INPUT_LENGTH = 3;
    private const string CORRECT_PASSWORD = "123";

    // Timer variable
    private float lastInteractionTime;
    private bool isTimerActive = true;

    private void Awake()
    {
        // Initialize events if null
        if (onCorrectPassword == null)
            onCorrectPassword = new UnityEvent();
        if (onIncorrectPassword == null)
            onIncorrectPassword = new UnityEvent();

        // Validate references
        if (inputField == null)
        {
            Debug.LogError("Input Field reference is missing! Please assign it in the inspector.");
        }

        if (numberButtons == null || numberButtons.Length != 9)
        {
            Debug.LogError("Please assign all 9 number buttons in the inspector!");
        }
    }

    private void Start()
    {
        if (inputField != null)
        {
            // Setup input field
            inputField.text = "";
            inputField.characterLimit = MAX_INPUT_LENGTH;
            inputField.contentType = TMP_InputField.ContentType.IntegerNumber;
            inputField.keyboardType = TouchScreenKeyboardType.NumberPad;
        }

        // Setup buttons
        for (int i = 0; i < numberButtons.Length; i++)
        {
            if (numberButtons[i] != null)
            {
                int buttonNumber = i + 1; // Numbers 1-9
                numberButtons[i].onClick.AddListener(() => OnNumberButtonPressed(buttonNumber.ToString()));
            }
        }

        // Initialize timer
        ResetInactivityTimer();
    }

    private void Update()
    {
        // Check for inactivity timeout
        if (isTimerActive && Time.time - lastInteractionTime >= inactivityTimeout)
        {
            Debug.Log("Keyboard deactivated due to inactivity");
            DeactivateKeyboard();
        }
    }

    private void OnNumberButtonPressed(string number)
    {
        if (inputField == null) return;

        // Reset timer on interaction
        ResetInactivityTimer();

        // Only add number if we haven't reached the limit
        if (inputField.text.Length < MAX_INPUT_LENGTH)
        {
            inputField.text += number;

            // Check if we've reached max length
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
        // Stop the inactivity timer during validation
        isTimerActive = false;

        // Small delay to ensure the last number is visible before clearing
        yield return new WaitForSeconds(0.5f);

        if (inputField.text != CORRECT_PASSWORD)
        {
            ClearInput();
            onIncorrectPassword.Invoke();
            Debug.Log("Incorrect password entered!");

            // Add small delay before deactivating
            yield return new WaitForSeconds(0.5f);
            DeactivateKeyboard();
        }
        else
        {
            Debug.Log("Correct password entered!");
            onCorrectPassword.Invoke();
        }
    }

    private void ClearInput()
    {
        if (inputField != null)
        {
            inputField.text = "";
        }
    }

    private void ResetInactivityTimer()
    {
        lastInteractionTime = Time.time;
        isTimerActive = true;
    }

    private void DeactivateKeyboard()
    {
        isTimerActive = false;
        gameObject.SetActive(false);
    }

    // Optional: Public method to reactivate the keyboard
    public void ActivateKeyboard()
    {
        gameObject.SetActive(true);
        ClearInput();
        ResetInactivityTimer();
    }
}