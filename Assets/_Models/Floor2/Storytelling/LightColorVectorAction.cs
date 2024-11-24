using UnityEngine;

namespace Storytelling
{
    /// <summary>
    /// Action that converts a position vector to light color
    /// </summary>
    public class LightColorVectorAction : VectorActionBehaviour
    {
        [SerializeField] private Light targetLight;
        
        [Header("Color Scaling Factors")]
        [SerializeField] private float redScale = 1f;
        [SerializeField] private float greenScale = 1f;
        [SerializeField] private float blueScale = 1f;

        private void Start()
        {
            if (targetLight == null)
            {
                targetLight = GetComponent<Light>();
                
                if (targetLight == null)
                {
                    Debug.LogError($"{name}: No Light component assigned or found on this GameObject");
                    enabled = false;
                }
            }
        }

        public override void Act(Vector3 vector)
        {
            if (targetLight != null)
            {
                // Convert position to color using scaling factors
                Color newColor = new Color(
                    Mathf.Clamp01(vector.x * redScale),   // X position affects Red
                    Mathf.Clamp01(vector.y * greenScale), // Y position affects Green
                    Mathf.Clamp01(vector.z * blueScale)   // Z position affects Blue
                );
                
                targetLight.color = newColor;
                
                if (logWhenPerformed)
                {
                    Debug.Log($"{name} changed light color to {newColor} based on position {vector}");
                }
            }
        }
    }
}
