public interface IDamageable
{
    // amount is treated as percentage points of health (0-100)
    void ApplyDamage(float amount);
}
