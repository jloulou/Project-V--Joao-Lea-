using UnityEngine;
using UnityEngine.Events;

public class FixedFadeAndDestroy : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent onFadeStart;
    public UnityEvent onFadeComplete;
    public UnityEvent<float> onFadeProgress;

    private Material[] materials;
    private float fadeTimer;
    private Color[] originalColors;
    private bool isFading;
    private bool fadeTriggered;

    private const float FADE_DURATION = 25f;
    private const float FADE_DELAY = 150f;

    private void Start()
    {
        InitializeMaterials();
        StartFade();
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
        float normalizedTime = fadeTimer / FADE_DURATION;
        float alpha = 1f - ApplyEaseInOutCurve(Mathf.Clamp01(normalizedTime));

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