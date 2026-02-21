using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance { get; private set; }

    public ParticleSystem muzzleFlashPrefab;
    public ParticleSystem bloodPrefab;
    public ParticleSystem impactPrefab;

    void Awake()
    {
        if (Instance == null) Instance = this; else Destroy(gameObject);
    }

    void Start()
    {
        // create simple particle prefabs if none assigned (placeholders)
        if (muzzleFlashPrefab == null)
        {
            var go = new GameObject("muzzleFlash_placeholder");
            var ps = go.AddComponent<ParticleSystem>();
            var main = ps.main;
            main.startColor = new ParticleSystem.MinMaxGradient(Color.yellow);
            main.startLifetime = 0.12f;
            main.startSize = 0.3f;
            main.loop = false;
            muzzleFlashPrefab = ps;
        }
        if (bloodPrefab == null)
        {
            var go = new GameObject("blood_placeholder");
            var ps = go.AddComponent<ParticleSystem>();
            var main = ps.main;
            main.startColor = new ParticleSystem.MinMaxGradient(Color.red);
            main.startLifetime = 0.6f;
            main.startSize = 0.25f;
            main.loop = false;
            bloodPrefab = ps;
        }
        if (impactPrefab == null)
        {
            var go = new GameObject("impact_placeholder");
            var ps = go.AddComponent<ParticleSystem>();
            var main = ps.main;
            main.startColor = new ParticleSystem.MinMaxGradient(Color.gray);
            main.startLifetime = 0.25f;
            main.startSize = 0.2f;
            main.loop = false;
            impactPrefab = ps;
        }
    }

    public void SpawnMuzzleFlash(Vector3 pos, Quaternion rot)
    {
        if (muzzleFlashPrefab == null) return;
        var ps = Instantiate(muzzleFlashPrefab, pos, rot);
        ps.Play();
        Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
    }

    public void SpawnBlood(Vector3 pos)
    {
        if (bloodPrefab == null) return;
        var ps = Instantiate(bloodPrefab, pos, Quaternion.identity);
        ps.Play();
        Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
    }

    public void SpawnImpact(Vector3 pos)
    {
        if (impactPrefab == null) return;
        var ps = Instantiate(impactPrefab, pos, Quaternion.identity);
        ps.Play();
        Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
    }
}
