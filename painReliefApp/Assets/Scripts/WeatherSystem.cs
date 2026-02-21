using UnityEngine;

public class WeatherSystem : MonoBehaviour
{
    public static WeatherSystem Instance { get; private set; }

    public enum WeatherType { Clear, Cloudy, Fog, Rain }
    public WeatherType currentWeather = WeatherType.Clear;

    [Header("Fog")]
    public float clearFogDensity = 0f;
    public float fogDensity = 0.02f;

    [Header("Clouds")]
    [Range(0f,1f)] public float cloudCoverage = 0f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        ApplyWeather();
    }

    void Update()
    {
        // placeholder: weather could change over time; not implemented yet
    }

    public void SetWeather(WeatherType w)
    {
        currentWeather = w;
        ApplyWeather();
    }

    void ApplyWeather()
    {
        if (currentWeather == WeatherType.Fog)
        {
            RenderSettings.fog = true;
            RenderSettings.fogDensity = fogDensity;
            cloudCoverage = 0.9f;
        }
        else
        {
            RenderSettings.fog = false;
            RenderSettings.fogDensity = clearFogDensity;
            cloudCoverage = (currentWeather == WeatherType.Cloudy) ? 0.6f : 0f;
        }
    }

    // returns a multiplier for visual detection (0-1, lower means harder to see)
    public float GetVisibilityMultiplier()
    {
        float m = 1f;
        if (currentWeather == WeatherType.Fog) m *= 0.5f;
        if (cloudCoverage > 0.7f) m *= 0.8f;
        return Mathf.Clamp01(m);
    }
}
