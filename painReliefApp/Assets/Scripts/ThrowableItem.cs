using UnityEngine;

public class ThrowableItem : ItemBase
{
    public GameObject thrownPrefab;
    public float throwForce = 8f;
    public float upwardsForce = 1f;

    public override bool Use(GameObject user)
    {
        if (thrownPrefab == null) return false;
        Transform t = user.transform;
        GameObject proj = GameObject.Instantiate(thrownPrefab, t.position + t.forward * 1.2f + Vector3.up * 1.2f, Quaternion.identity);
        var rb = proj.GetComponent<Rigidbody>();
        if (rb == null) rb = proj.AddComponent<Rigidbody>();
        Vector3 force = (t.forward * throwForce) + (Vector3.up * upwardsForce);
        rb.AddForce(force, ForceMode.VelocityChange);
        return true; // consumed
    }
}
