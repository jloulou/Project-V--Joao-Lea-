// VRNumberKeyboard.cs
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

    [Tooltip("Reference to the PlatformTrigger component")]
    [SerializeField]
    private PlatformTrigger platformTrigger;

    [Header("Events")]
    [Tooltip("Event triggered when correct password is entered")]
    public UnityEvent onCorrectPassword;

    [Tooltip("Event triggered when incorrect password is cleared")]
    public UnityEvent onIncorrectPassword;

    // Settings
    private const int MAX_INPUT_LENGTH = 3;
    private const string CORRECT_PASSWORD = "123";

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

        if (platformTrigger == null)
        {
            Debug.LogError("PlatformTrigger reference is missing! Please assign it in the inspector.");
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
    }

    private void OnNumberButtonPressed(string number)
    {
        if (inputField == null) return;

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
        // Small delay to ensure the last number is visible before clearing
        yield return new WaitForSeconds(0.5f);

        if (inputField.text != CORRECT_PASSWORD)
        {
            ClearInput();
            onIncorrectPassword.Invoke();
            Debug.Log("Incorrect password entered!");
        }
        else
        {
            Debug.Log("Correct password entered!");
            onCorrectPassword.Invoke();

            // Trigger the platform to move to floor 4
            if (platformTrigger != null)
            {
                platformTrigger.targetFloor = 4;  // Set target floor to 4
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