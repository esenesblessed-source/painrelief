using UnityEngine;

public static class AudioPlaceholderGenerator
{
    // sample rate
    private const int SR = 44100;

    public static AudioClip CreateGunshotClip()
    {
        int length = SR / 4; // 0.25s
        float[] data = new float[length];
        // white noise burst with exponential decay
        for (int i = 0; i < length; i++)
        {
            float t = (float)i / length;
            float env = Mathf.Exp(-6f * t);
            data[i] = (Random.value * 2f - 1f) * env * 0.9f;
        }
        var clip = AudioClip.Create("gunshot_placeholder", length, 1, SR, false);
        clip.SetData(data, 0);
        return clip;
    }

    public static AudioClip CreateFootstepClip()
    {
        int length = SR / 10; // 0.1s
        float[] data = new float[length];
        for (int i = 0; i < length; i++)
        {
            float t = (float)i / length;
            float env = Mathf.Sin(Mathf.PI * t) * Mathf.Exp(-4f * t);
            data[i] = (Random.value * 2f - 1f) * env * 0.6f;
        }
        var clip = AudioClip.Create("footstep_placeholder", length, 1, SR, false);
        clip.SetData(data, 0);
        return clip;
    }

    public static AudioClip CreateZombieGrowlClip()
    {
        int length = SR * 2; // 2s
        float[] data = new float[length];
        float baseFreq = 80f;
        for (int i = 0; i < length; i++)
        {
            float t = (float)i / SR;
            // low rumble with low-frequency oscillator
            float lfo = Mathf.Sin(2f * Mathf.PI * 0.6f * t) * 0.5f + 0.5f;
            float noise = (Random.value * 2f - 1f) * 0.5f;
            float tone = Mathf.Sin(2f * Mathf.PI * baseFreq * (1f + 0.02f * Mathf.Sin(2f * Mathf.PI * 0.2f * t)) * t);
            data[i] = (tone * 0.6f + noise * 0.4f) * lfo * 0.8f;
        }
        var clip = AudioClip.Create("zombie_growl_placeholder", length, 1, SR, false);
        clip.SetData(data, 0);
        return clip;
    }

    public static AudioClip CreateCarEngineClip()
    {
        int length = SR * 2; // 2s loop
        float[] data = new float[length];
        float baseFreq = 70f;
        for (int i = 0; i < length; i++)
        {
            float t = (float)i / SR;
            float tone = Mathf.Sin(2f * Mathf.PI * baseFreq * t) * 0.6f;
            float overtone = Mathf.Sin(2f * Mathf.PI * baseFreq * 2.0f * t) * 0.2f;
            float noise = (Random.value * 2f - 1f) * 0.05f;
            data[i] = (tone + overtone + noise) * 0.8f;
        }
        var clip = AudioClip.Create("car_engine_placeholder", length, 1, SR, false);
        clip.SetData(data, 0);
        return clip;
    }
}
