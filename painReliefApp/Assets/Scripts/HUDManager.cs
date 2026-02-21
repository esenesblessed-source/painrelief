using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public PlayerStats playerStats;
    public Slider healthSlider;
    public Slider staminaSlider;
    public GameObject inventoryPanel; // simple panel to list items
    public RectTransform inventoryContent;
    public GameObject inventoryItemPrefab; // UI prefab with Text for item name

    private Inventory inventory;

    void Start()
    {
        if (playerStats == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) playerStats = p.GetComponent<PlayerStats>();
        }

        if (playerStats != null && healthSlider != null)
        {
            healthSlider.maxValue = playerStats.maxHealth;
            healthSlider.value = playerStats.health;
        }

        if (playerStats != null && staminaSlider != null)
        {
            staminaSlider.maxValue = playerStats.maxStamina;
            staminaSlider.value = playerStats.stamina;
        }

        if (inventoryPanel != null) inventoryPanel.SetActive(false);
        if (playerStats != null) inventory = playerStats.GetComponent<Inventory>();
    }

    void Update()
    {
        if (playerStats != null && healthSlider != null)
            healthSlider.value = playerStats.health;

        if (playerStats != null && staminaSlider != null)
            staminaSlider.value = playerStats.stamina;

        // toggle inventory with I (or mobile via MobileInput)
        if (Input.GetKeyDown(KeyCode.I)) ToggleInventory();
        if (MobileInput.Instance != null && MobileInput.Instance.enterExit)
        {
            ToggleInventory();
            MobileInput.Instance.enterExit = false;
        }
    }

    public void ToggleInventory()
    {
        if (inventoryPanel == null) return;
        bool active = !inventoryPanel.activeSelf;
        inventoryPanel.SetActive(active);
        if (active) RefreshInventory();
    }

    void RefreshInventory()
    {
        if (inventory == null || inventoryContent == null || inventoryItemPrefab == null) return;
        foreach (Transform t in inventoryContent) Destroy(t.gameObject);
        for (int i = 0; i < inventory.Count; i++)
        {
            var itemGo = inventory.GetItemAt(i);
            if (itemGo == null) continue;
            var ui = GameObject.Instantiate(inventoryItemPrefab, inventoryContent);
            var txt = ui.GetComponentInChildren<Text>();
            if (txt != null) txt.text = itemGo.name;
        }
    }
}
