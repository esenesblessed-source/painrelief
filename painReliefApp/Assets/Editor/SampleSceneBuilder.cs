#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class SampleSceneBuilder
{
    [MenuItem("Tools/Create Sample Scene")]
    public static void CreateSampleScene()
    {
        // Create a new empty scene
        var scene = UnityEditor.SceneManagement.EditorSceneManager.NewScene(UnityEditor.SceneManagement.NewSceneSetup.EmptyScene);

        // Create ground
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.localScale = Vector3.one * 50f;

        // Create player capsule
        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "Player";
        player.tag = "Player";
        player.transform.position = new Vector3(0f, 1f, 0f);
        var cc = player.AddComponent<CharacterController>();
        player.AddComponent<Assets.Scripts.PlayerController>();
        player.AddComponent<Assets.Scripts.PlayerStats>();
        player.AddComponent<Assets.Scripts.Inventory>();

        // Main Camera
        GameObject camGO = new GameObject("Main Camera");
        var cam = camGO.AddComponent<Camera>();
        camGO.tag = "MainCamera";
        camGO.transform.position = player.transform.position + new Vector3(0f, 1.6f, 0f);
        camGO.AddComponent<Assets.Scripts.SimpleCameraController>().player = player.transform;

        // GameManager
        GameObject gm = new GameObject("GameManager");
        gm.AddComponent<Assets.Scripts.GameManager>();

        // SoundManager
        GameObject sm = new GameObject("SoundManager");
        sm.AddComponent<Assets.Scripts.SoundManager>();

        // TimeOfDay and WeatherSystem
        GameObject tod = new GameObject("TimeOfDay");
        tod.AddComponent<Assets.Scripts.TimeOfDay>();
        GameObject ws = new GameObject("WeatherSystem");
        ws.AddComponent<Assets.Scripts.WeatherSystem>();

        // ZombieSpawner
        GameObject sp = new GameObject("ZombieSpawner");
        sp.AddComponent<Assets.Scripts.ZombieSpawner>();

        // Create folders for Scenes
        if (!AssetDatabase.IsValidFolder("Assets/Scenes")) AssetDatabase.CreateFolder("Assets", "Scenes");
        string path = "Assets/Scenes/SampleScene.unity";
        UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene, path);

        Debug.Log("Sample scene created at " + path + " — open it and use Tools/Build Example UI to create the HUD.");
    }
}
#endif
