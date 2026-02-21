using UnityEngine;

public class HealthInjection : ItemBase
{
    public float healPercent = 50f;

    public override bool Use(GameObject user)
    {
        var stats = user.GetComponent<PlayerStats>();
        if (stats == null) return false;
        stats.HealPercent(healPercent);
        Debug.Log($"{itemName} used: healed {healPercent}%");
        return true; // consumed
    }
}
