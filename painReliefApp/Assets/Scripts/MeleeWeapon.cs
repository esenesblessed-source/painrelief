using UnityEngine;

public class MeleeWeapon : ItemBase
{
    [Tooltip("Damage in percent points applied to IDamageable (0-100)")]
    public float damagePercent = 25f;
    public float range = 1.5f;
    public float radius = 0.6f;

    public override bool Use(GameObject user)
    {
        Transform t = user.transform;
        RaycastHit[] hits = Physics.SphereCastAll(t.position + Vector3.up * 1f, radius, t.forward, range);
        bool hitAnything = false;
        foreach (var h in hits)
        {
            var dmg = h.collider.GetComponent<IDamageable>();
            if (dmg != null)
            {
                dmg.ApplyDamage(damagePercent);
                hitAnything = true;
            }
        }
        return false; // melee weapon not consumed
    }
}
