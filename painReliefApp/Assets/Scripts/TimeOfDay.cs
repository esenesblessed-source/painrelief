using UnityEngine;

public class TimeOfDay : MonoBehaviour
{
    public static TimeOfDay Instance { get; private set; }

    [Tooltip("Current time in hours (0-24)")]
    [Range(0f, 24f)]
    public float currentTime = 8f;
    [Tooltip("How many real seconds correspond to one in-game hour")]
    public float secondsPerGameHour = 60f;
    public bool advance = true;

    public float sunrise = 6f;
    public float sunset = 18f;

    [Header("Lighting")]
    public Light sunLight;
    public Color dayColor = Color.white;
    public Color nightColor = new Color(0.4f, 0.45f, 0.6f);
    public float dayIntensity = 1f;
    public float nightIntensity = 0.15f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (sunLight == null && RenderSettings.sun != null) sunLight = RenderSettings.sun;
    }

    void Update()
    {
        if (advance)
        {
            float hoursPerSecond = 1f / secondsPerGameHour;
            currentTime += hoursPerSecond * Time.deltaTime;
            if (currentTime >= 24f) currentTime -= 24f;
        }

        UpdateLighting();
    }

    void UpdateLighting()
    {
        if (sunLight == null) return;
        // simple day/night blend based on time
        float t = Mathf.InverseLerp(sunrise, sunset, currentTime);
        // when t=0 at sunrise, t=1 at sunset
        Color col = Color.Lerp(nightColor, dayColor, t);
        float intensity = Mathf.Lerp(nightIntensity, dayIntensity, t);
        sunLight.color = col;
        sunLight.intensity = intensity;
        // rotate sun across sky
        float angle = Mathf.Lerp(-30f, 210f, t);
        sunLight.transform.rotation = Quaternion.Euler(new Vector3(angle, 170f, 0f));
    }

    public bool IsNight()
    {
        return (currentTime < sunrise || currentTime >= sunset);
    }
}
