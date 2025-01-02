using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIFadeAndDestroy : MonoBehaviour
{
    public enum FadeCurve { Linear, EaseIn, EaseOut, EaseInOut, Exponential }

    [Header("Fade Settings")]
    public float delayBeforeFade = 2f;
    public float fadeDuration = 3f;
    public FadeCurve fadeCurveType = FadeCurve.Linear;
    public bool autoStart = true;

    [Header("Events")]
    public UnityEvent onFadeStart;
    public UnityEvent onFadeComplete;
    public UnityEvent<float> onFadeProgress;

    private CanvasGroup canvasGroup;
    private float fadeTimer;
    private bool isFading;
    private bool fadeTriggered;

    private void Start()
    {
        InitializeCanvasGroup();
        if (autoStart) StartFade();
    }

    private void InitializeCanvasGroup()
    {
        // Get or add CanvasGroup component
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 1f;
    }

    private void Update()
    {
        if (!isFading || canvasGroup == null) return;

        fadeTimer += Time.deltaTime;
        float normalizedTime = fadeTimer / fadeDuration;
        float alpha = 1f - ApplyFadeCurve(Mathf.Clamp01(normalizedTime));

        canvasGroup.alpha = alpha;
        onFadeProgress?.Invoke(1f - alpha);

        if (normalizedTime >= 1f)
        {
            onFadeComplete?.Invoke();
            Destroy(gameObject);
        }
    }

    public void OnObjectSelected()
    {
        if (!fadeTriggered)
        {
            TriggerFade();
        }
    }

    public void TriggerFade()
    {
        fadeTriggered = true;
        StartFade();
    }

    public void StartFade()
    {
        Invoke("BeginFading", delayBeforeFade);
    }

    private void BeginFading()
    {
        isFading = true;
        fadeTimer = 0f;
        onFadeStart?.Invoke();
    }

    private float ApplyFadeCurve(float t)
    {
        switch (fadeCurveType)
        {
            case FadeCurve.Linear:
                return t;
            case FadeCurve.EaseIn:
                return t * t;
            case FadeCurve.EaseOut:
                return 1 - (1 - t) * (1 - t);
            case FadeCurve.EaseInOut:
                return t < 0.5f ? 2 * t * t : 1 - Mathf.Pow(-2 * t + 2, 2) / 2;
            case FadeCurve.Exponential:
                return 1 - Mathf.Pow(2, -10 * t);
            default:
                return t;
        }
    }
}