using UnityEngine;

[ExecuteAlways]
public class DayCycleManager : MonoBehaviour {
    [Range(0f, 1f)]
    public float dayNightValue = 0.5f;

    [Header("Skybox")]
    public Material skyboxMaterial;

    [Header("Lights")]
    public Light mainLight;
    public Light rimLight;

    [Header("Light Settings")]
    public Gradient directionalColor;     // Color over day
    public AnimationCurve directionalIntensity; // Intensity over day
    public Gradient rimLightColor;
    public AnimationCurve rimLightIntensity;

    [Header("Sun Direction")]
    [SerializeField] private float sunElevationAngle = 180f; // total degrees the sun travels (sunrise to sunset)

    // Optional azimuth rotation (e.g., rotate around Y to make it rise from east)
    [SerializeField] private float azimuthAngle = 30f;

    private void OnValidate()
    {
        UpdateCycle();
    }

    private void Update()
    {
        UpdateCycle();
    }

    private void UpdateCycle()
    {
        UpdateSkybox();
        UpdateLights();
    }

    private void UpdateSkybox()
    {
        if (skyboxMaterial != null)
        {
            skyboxMaterial.SetFloat("_TimeValue", dayNightValue);
            DynamicGI.UpdateEnvironment();
        }
    }

    private void UpdateLights()
    {
        float xRotation = Mathf.Lerp(0f, 180f, dayNightValue);
        Quaternion rotation = Quaternion.Euler(xRotation, azimuthAngle, 0f);

        if (mainLight != null)
        {
            mainLight.color = directionalColor.Evaluate(dayNightValue);
            mainLight.intensity = directionalIntensity.Evaluate(dayNightValue);
            mainLight.transform.rotation = rotation;
        }

        if (rimLight != null)
        {
            rimLight.color = rimLightColor.Evaluate(dayNightValue);
            rimLight.intensity = rimLightIntensity.Evaluate(dayNightValue);
            rimLight.transform.rotation = rotation;
        }
    }
}
