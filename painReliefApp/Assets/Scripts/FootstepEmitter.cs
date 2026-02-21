using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FootstepEmitter : MonoBehaviour
{
    public float stepInterval = 0.5f;
    private float timer = 0f;

    void Update()
    {
        var cc = GetComponent<CharacterController>();
        if (cc == null) return;
        if (cc.velocity.magnitude > 0.1f && cc.isGrounded)
        {
            timer += Time.deltaTime;
            if (timer >= stepInterval)
            {
                timer = 0f;
                if (AudioManager.Instance != null) AudioManager.Instance.PlayFootstep(transform.position);
            }
        }
        else timer = 0f;
    }
}
