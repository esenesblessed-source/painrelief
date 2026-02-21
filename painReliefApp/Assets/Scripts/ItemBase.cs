using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    public string itemName = "Item";

    // Called when the item is used by the player. Return true if the item should be consumed.
    public abstract bool Use(GameObject user);
}
