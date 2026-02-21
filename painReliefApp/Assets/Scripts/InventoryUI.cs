using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public RectTransform content;
    public GameObject itemPrefab; // simple UI element with Text

    void Start()
    {
        if (inventory == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) inventory = p.GetComponent<Inventory>();
        }
        Refresh();
    }

    public void Refresh()
    {
        if (content == null || itemPrefab == null || inventory == null) return;
        foreach (Transform t in content) Destroy(t.gameObject);
        for (int i = 0; i < inventory.Count; i++)
        {
            var item = inventory.GetItemAt(i);
            if (item == null) continue;
            var ui = Instantiate(itemPrefab, content);
            var txt = ui.GetComponentInChildren<Text>();
            if (txt != null) txt.text = item.name;
            int idx = i;
            var btn = ui.GetComponentInChildren<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(() => { inventory.UseItem(idx, GameObject.FindGameObjectWithTag("Player")); Refresh(); });
            }
        }
    }
}
