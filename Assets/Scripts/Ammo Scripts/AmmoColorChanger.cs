using UnityEngine;

public class AmmoColorChanger : MonoBehaviour
{
    public Renderer ammoRenderer;
    public float colorChangeSpeed = 1.0f; // Adjusts the speed of color cycling
    public float glowIntensity = 2.0f;    // Intensity of the neon glow

    private Material ammoMaterial;
    private float hue;

    void Start()
    {
        if (ammoRenderer == null)
        {
            ammoRenderer = GetComponent<Renderer>();
        }
        
        // Get a reference to the material
        ammoMaterial = ammoRenderer.material;

        // Enable emission on the material
        ammoMaterial.EnableKeyword("_EMISSION");
    }

    void Update()
    {
        // Increment the hue value over time
        hue += Time.deltaTime * colorChangeSpeed;
        if (hue > 1.0f) hue -= 1.0f; // Keep hue within 0-1 range

        // Convert hue to an RGB color and set it
        Color color = Color.HSVToRGB(hue, 1.0f, 1.0f);
        ammoMaterial.color = color;

        // Apply the same color to the emission channel for a neon glow
        ammoMaterial.SetColor("_EmissionColor", color * glowIntensity);
    }
}
