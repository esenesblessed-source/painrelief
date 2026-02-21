using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Tooltip("Prefab reference for the item to add to inventory")]
    public GameObject itemPrefab;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var inv = other.GetComponent<Inventory>();
        if (inv == null)
        {
            inv = other.GetComponentInChildren<Inventory>();
        }

        if (inv != null)
        {
            bool ok = inv.AddItem(itemPrefab != null ? itemPrefab : gameObject);
            if (ok)
            {
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Inventory full");
            }
        }
    }
}
