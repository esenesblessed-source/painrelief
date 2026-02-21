using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int capacity = 10;
    private List<GameObject> items = new List<GameObject>();

    public bool AddItem(GameObject itemPrefab)
    {
        if (items.Count >= capacity) return false;
        items.Add(itemPrefab);
        Debug.Log($"Picked up: {itemPrefab.name}");
        return true;
    }

    public bool RemoveItemAt(int index)
    {
        if (index < 0 || index >= items.Count) return false;
        items.RemoveAt(index);
        return true;
    }

    public GameObject GetItemAt(int index)
    {
        if (index < 0 || index >= items.Count) return null;
        return items[index];
    }

    public int Count => items.Count;

    // Use an item from inventory by index
    public void UseItem(int index, GameObject user)
    {
        var go = GetItemAt(index);
        if (go == null) return;
        var item = go.GetComponent<ItemBase>();
        if (item != null)
        {
            bool consumed = item.Use(user);
            if (consumed)
                RemoveItemAt(index);
        }
    }
}
