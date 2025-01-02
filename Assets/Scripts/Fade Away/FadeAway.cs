using UnityEngine;
using UnityEngine.Events;

public class FadeAndDestroy : MonoBehaviour
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

    private Material[] materials;
    private float fadeTimer;
    private Color[] originalColors;
    private bool isFading;
    private bool fadeTriggered;

    private void Start()
    {
        InitializeMaterials();
        if (autoStart) StartFade();
    }

    private void InitializeMaterials()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            materials = renderer.materials;
            originalColors = new Color[materials.Length];

            for (int i = 0; i < materials.Length; i++)
            {
                originalColors[i] = materials[i].color;
                SetupMaterial(materials[i]);
            }
        }
    }

    private void SetupMaterial(Material material)
    {
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 1);
        material.EnableKeyword("_ALPHABLEND_ON");
        material.renderQueue = 3000;
    }

    private void Update()
    {
        if (!isFading || materials == null) return;

        fadeTimer += Time.deltaTime;
        float normalizedTime = fadeTimer / fadeDuration;
        float alpha = 1f - ApplyFadeCurve(Mathf.Clamp01(normalizedTime));

        for (int i = 0; i < materials.Length; i++)
        {
            Color newColor = originalColors[i];
            newColor.a = alpha;
            materials[i].color = newColor;
        }

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