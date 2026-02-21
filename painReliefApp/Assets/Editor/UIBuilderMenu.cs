#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIBuilderMenu
{
    [MenuItem("Tools/Build Example UI")]
    public static void BuildExampleUI()
    {
        // Create Canvas
        GameObject canvasGO = new GameObject("HUD_Canvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        // Create Health Slider
        GameObject healthGO = new GameObject("HealthSlider");
        healthGO.transform.SetParent(canvasGO.transform, false);
        Slider healthSlider = healthGO.AddComponent<Slider>();
        RectTransform hsRT = healthGO.GetComponent<RectTransform>();
        hsRT.anchorMin = new Vector2(0.02f, 0.95f);
        hsRT.anchorMax = new Vector2(0.3f, 0.99f);
        hsRT.offsetMin = Vector2.zero; hsRT.offsetMax = Vector2.zero;

        // Create Stamina Slider
        GameObject stamGO = new GameObject("StaminaSlider");
        stamGO.transform.SetParent(canvasGO.transform, false);
        Slider stamSlider = stamGO.AddComponent<Slider>();
        RectTransform ssRT = stamGO.GetComponent<RectTransform>();
        ssRT.anchorMin = new Vector2(0.02f, 0.90f);
        ssRT.anchorMax = new Vector2(0.3f, 0.94f);
        ssRT.offsetMin = Vector2.zero; ssRT.offsetMax = Vector2.zero;

        // Inventory Panel (hidden by default)
        GameObject invPanel = new GameObject("InventoryPanel");
        invPanel.transform.SetParent(canvasGO.transform, false);
        RectTransform ipRT = invPanel.AddComponent<RectTransform>();
        ipRT.anchorMin = new Vector2(0.55f, 0.1f);
        ipRT.anchorMax = new Vector2(0.95f, 0.6f);
        ipRT.offsetMin = Vector2.zero; ipRT.offsetMax = Vector2.zero;
        var img = invPanel.AddComponent<Image>();
        img.color = new Color(0f, 0f, 0f, 0.5f);
        invPanel.SetActive(false);

        // Inventory item prefab (Button + Text)
        GameObject itemPrefab = new GameObject("InventoryItemPrefab");
        GameObject btnGO = new GameObject("Button");
        btnGO.transform.SetParent(itemPrefab.transform, false);
        var button = btnGO.AddComponent<Button>();
        var imgB = btnGO.AddComponent<Image>();
        imgB.color = Color.white;
        GameObject txtGO = new GameObject("Text");
        txtGO.transform.SetParent(btnGO.transform, false);
        var txt = txtGO.AddComponent<Text>();
        txt.text = "Item";
        txt.alignment = TextAnchor.MiddleCenter;
        txt.color = Color.black;

        // Create Minimap RawImage (user should attach a RenderTexture)
        GameObject miniGO = new GameObject("Minimap");
        miniGO.transform.SetParent(canvasGO.transform, false);
        RectTransform mmRT = miniGO.AddComponent<RectTransform>();
        mmRT.anchorMin = new Vector2(0.78f, 0.78f);
        mmRT.anchorMax = new Vector2(0.98f, 0.98f);
        mmRT.offsetMin = Vector2.zero; mmRT.offsetMax = Vector2.zero;
        var raw = miniGO.AddComponent<RawImage>();

        // Save prefabs
        string prefabDir = "Assets/Prefabs/ExampleUI";
        if (!AssetDatabase.IsValidFolder("Assets/Prefabs")) AssetDatabase.CreateFolder("Assets", "Prefabs");
        if (!AssetDatabase.IsValidFolder(prefabDir)) AssetDatabase.CreateFolder("Assets/Prefabs", "ExampleUI");

        string itemPath = prefabDir + "/InventoryItemPrefab.prefab";
        UnityEditor.PrefabUtility.SaveAsPrefabAsset(itemPrefab, itemPath);
        GameObject.DestroyImmediate(itemPrefab);

        // Attach HUDManager to a new GameObject
        GameObject hudManagerGO = new GameObject("HUDManager");
        hudManagerGO.transform.SetParent(canvasGO.transform, false);
        var hud = hudManagerGO.AddComponent<Assets.Scripts.HUDManager>();
        hud.healthSlider = healthSlider;
        hud.staminaSlider = stamSlider;
        hud.inventoryPanel = invPanel;
        // load the inventory item prefab
        hud.inventoryItemPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(itemPath);

        // Mark scene dirty
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());

        Debug.Log("Example UI built. Check the HUD_Canvas in the Hierarchy.");
    }
}
#endif
