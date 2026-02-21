using UnityEngine;

public class AutoQualityManager : MonoBehaviour
{
    public int targetFPS = 30;
    public float sampleInterval = 5f;
    private float timer = 0f;

    void Update()
    {
        timer += Time.unscaledDeltaTime;
        if (timer >= sampleInterval)
        {
            timer = 0f;
            float fps = 1f / Time.unscaledDeltaTime;
            // very simple: if fps below target decrease quality, if above increase
            if (fps < targetFPS && QualitySettings.GetQualityLevel() > 0)
            {
                QualitySettings.DecreaseLevel(true);
                Debug.Log("AutoQuality: Decreased level to " + QualitySettings.GetQualityLevel());
            }
            else if (fps > targetFPS + 10 && QualitySettings.GetQualityLevel() < QualitySettings.names.Length - 1)
            {
                QualitySettings.IncreaseLevel(true);
                Debug.Log("AutoQuality: Increased level to " + QualitySettings.GetQualityLevel());
            }
        }
    }
}
