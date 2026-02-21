using UnityEngine;

public class PlayerStats : UnityEngine.MonoBehaviour, IDamageable
{
    [Header("Health")]
    public float maxHealth = 100f;
    public float health = 100f;

    [Header("Stamina")]
    public float maxStamina = 100f;
    public float stamina = 100f;
    public float staminaDrainPerSecond = 15f;
    public float staminaRegenPerSecond = 10f;

    [Header("Settings")]
    public float runSpeedThreshold = 0.1f;

    void Start()
    {
        health = Mathf.Clamp(health, 0f, maxHealth);
        stamina = Mathf.Clamp(stamina, 0f, maxStamina);
    }

    void Update()
    {
        HandleStamina();
    }

    void HandleStamina()
    {
        bool isRunning = Input.GetKey(UnityEngine.KeyCode.LeftShift) && (Mathf.Abs(Input.GetAxis("Horizontal")) > runSpeedThreshold || Mathf.Abs(Input.GetAxis("Vertical")) > runSpeedThreshold);
        if (isRunning)
        {
            stamina -= staminaDrainPerSecond * Time.deltaTime;
            if (stamina < 0f) stamina = 0f;
        }
        else
        {
            stamina += staminaRegenPerSecond * Time.deltaTime;
            if (stamina > maxStamina) stamina = maxStamina;
        }
    }

    public void TakeDamagePercent(float percent)
    {
        float dmg = percent;
        health -= dmg;
        health = Mathf.Clamp(health, 0f, maxHealth);
        if (health <= 0f) Die();
    }

    // IDamageable implementation
    public void ApplyDamage(float amount)
    {
        TakeDamagePercent(amount);
    }

    public void HealPercent(float percent)
    {
        health += percent;
        health = Mathf.Clamp(health, 0f, maxHealth);
    }

    void Die()
    {
        // Placeholder: disable input / show death state. Implement when integrating input system.
        Debug.Log("Player died");
    }
}
