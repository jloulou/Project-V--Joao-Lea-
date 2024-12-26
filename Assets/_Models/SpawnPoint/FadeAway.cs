using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class FadeAndDestroy : MonoBehaviour
{
    [Tooltip("Time in seconds before the object starts fading")]
    public float delayBeforeFade = 2f;

    [Tooltip("Duration of the fade effect in seconds")]
    public float fadeDuration = 3f;

    [Tooltip("Should the script start automatically on enable?")]
    public bool autoStart = true;

    // Events
    [Header("Events")]
    public UnityEvent onFadeStart;
    public UnityEvent onFadeComplete;
    public UnityEvent<float> onFadeProgress;

    private Material[] materials;
    private float fadeTimer;
    private bool isFading;
    private Color[] originalColors;
    private bool hasStartedFading = false;

    private void Start()
    {
        // Get all materials from the renderer
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            materials = renderer.materials;
            // Store original colors
            originalColors = new Color[materials.Length];
            for (int i = 0; i < materials.Length; i++)
            {
                originalColors[i] = materials[i].color;
            }
        }
        else
        {
            Debug.LogWarning("No Renderer component found on object: " + gameObject.name);
        }

        if (autoStart)
        {
            StartFade();
        }
    }

    public void StartFade()
    {
        // Start the delay coroutine
        Invoke("BeginFading", delayBeforeFade);
    }

    private void BeginFading()
    {
        isFading = true;
        fadeTimer = 0f;
        hasStartedFading = false;
    }

    private void Update()
    {
        if (!isFading || materials == null) return;

        // Trigger fade start event only once when fading begins
        if (!hasStartedFading)
        {
            onFadeStart?.Invoke();
            hasStartedFading = true;
        }

        fadeTimer += Time.deltaTime;
        float normalizedTime = fadeTimer / fadeDuration;

        // Calculate alpha based on time
        float alpha = 1f - Mathf.Clamp01(normalizedTime);

        // Invoke progress event
        onFadeProgress?.Invoke(1f - alpha);

        // Apply fade to all materials
        for (int i = 0; i < materials.Length; i++)
        {
            Color newColor = originalColors[i];
            newColor.a = alpha;
            materials[i].color = newColor;
        }

        // Check if fade is complete
        if (normalizedTime >= 1f)
        {
            onFadeComplete?.Invoke();
            Destroy(gameObject);
        }
    }
}