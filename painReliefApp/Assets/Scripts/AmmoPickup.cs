using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 30;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var wm = other.GetComponent<WeaponManager>();
        if (wm == null) wm = other.GetComponentInChildren<WeaponManager>();
        if (wm != null)
        {
            wm.AddAmmoToAll(ammoAmount);
            Destroy(gameObject);
        }
    }
}
