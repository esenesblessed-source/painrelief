using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public struct SoundEvent { public Vector3 pos; public float radius; public float time; }
    public SoundEvent lastSound;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void MakeSound(Vector3 pos, float radius)
    {
        lastSound.pos = pos;
        lastSound.radius = radius;
        lastSound.time = Time.time;
    }

    // returns true if the listener at 'listenerPos' with hearing range 'hearingRadius' can hear the last sound
    public bool HearLastSound(Vector3 listenerPos, float hearingRadius, out Vector3 soundPos)
    {
        soundPos = Vector3.zero;
        if (lastSound.time <= 0f) return false;
        if (Time.time - lastSound.time > 6f) return false; // sound expires
        float dist = Vector3.Distance(listenerPos, lastSound.pos);
        if (dist <= hearingRadius + lastSound.radius)
        {
            soundPos = lastSound.pos;
            return true;
        }
        return false;
    }
}
