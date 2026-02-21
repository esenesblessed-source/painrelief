using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("SFX Clips (assign in editor)")]
    public AudioClip gunshotClip;
    public AudioClip footstepClip;
    public AudioClip zombieGrowlClip;
    public AudioClip carEngineClip;

    private List<AudioSource> pool = new List<AudioSource>();

    void Awake()
    {
        if (Instance == null) Instance = this; else Destroy(gameObject);
    }

    void Start()
    {
        // create placeholder clips if none assigned
        if (gunshotClip == null) gunshotClip = AudioPlaceholderGenerator.CreateGunshotClip();
        if (footstepClip == null) footstepClip = AudioPlaceholderGenerator.CreateFootstepClip();
        if (zombieGrowlClip == null) zombieGrowlClip = AudioPlaceholderGenerator.CreateZombieGrowlClip();
        if (carEngineClip == null) carEngineClip = AudioPlaceholderGenerator.CreateCarEngineClip();
    }

    AudioSource GetSource()
    {
        foreach (var s in pool) if (!s.isPlaying) return s;
        var go = new GameObject("SFX_Source");
        go.transform.SetParent(transform);
        var src = go.AddComponent<AudioSource>();
        src.spatialBlend = 1f; // 3D
        pool.Add(src);
        return src;
    }

    public void PlaySFXAt(AudioClip clip, Vector3 pos, float volume = 1f)
    {
        if (clip == null) return;
        AudioSource.PlayClipAtPoint(clip, pos, volume);
    }

    public void PlaySFX(AudioClip clip, Vector3 pos, float volume = 1f, float spatialBlend = 1f)
    {
        if (clip == null) return;
        var src = GetSource();
        src.clip = clip;
        src.transform.position = pos;
        src.volume = volume;
        src.spatialBlend = spatialBlend;
        src.Play();
    }

    // helper calls
    public void PlayGunshot(Vector3 pos)
    {
        PlaySFX(gunshotClip, pos, 1f);
    }

    public void PlayFootstep(Vector3 pos)
    {
        PlaySFX(footstepClip, pos, 0.7f);
    }

    public void PlayZombieGrowl(Vector3 pos)
    {
        PlaySFX(zombieGrowlClip, pos, 0.9f);
    }

    public void PlayEngineSound(AudioSource engineSource)
    {
        if (engineSource == null || carEngineClip == null) return;
        engineSource.clip = carEngineClip;
        engineSource.loop = true;
        engineSource.spatialBlend = 1f;
        engineSource.Play();
    }
}
