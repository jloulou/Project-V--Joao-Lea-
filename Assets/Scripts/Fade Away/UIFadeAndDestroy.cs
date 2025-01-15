using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FixedUIFadeAndDestroy : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent onFadeStart;
    public UnityEvent onFadeComplete;
    public UnityEvent<float> onFadeProgress;

    private CanvasGroup canvasGroup;
    private float fadeTimer;
    private bool isFading;
    private bool fadeTriggered;

    private const float FADE_DURATION = 25f;
    private const float FADE_DELAY = 150f;

    private void Start()
    {
        InitializeCanvasGroup();
        StartFade();
    }

    private void InitializeCanvasGroup()
    {
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
        float normalizedTime = fadeTimer / FADE_DURATION;
        float alpha = 1f - ApplyEaseInOutCurve(Mathf.Clamp01(normalizedTime));

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
        Invoke("BeginFading", FADE_DELAY);
    }

    private void BeginFading()
    {
        isFading = true;
        fadeTimer = 0f;
        onFadeStart?.Invoke();
    }

    private float ApplyEaseInOutCurve(float t)
    {
        return t < 0.5f ? 2 * t * t : 1 - Mathf.Pow(-2 * t + 2, 2) / 2;
    }
}