#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEditor.Animations;

public class ExampleAssetsImporter
{
    [MenuItem("Tools/Import Example Assets")]
    public static void ImportAssets()
    {
        string prefabDir = "Assets/Prefabs/ExampleAssets";
        if (!AssetDatabase.IsValidFolder("Assets/Prefabs")) AssetDatabase.CreateFolder("Assets", "Prefabs");
        if (!AssetDatabase.IsValidFolder(prefabDir)) AssetDatabase.CreateFolder("Assets/Prefabs", "ExampleAssets");

        // Zombie prefab
        GameObject zombie = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        zombie.name = "ZombieModel";
        zombie.transform.position = Vector3.zero;
        var agent = zombie.AddComponent<UnityEngine.AI.NavMeshAgent>();
        var ai = zombie.AddComponent<Assets.Scripts.ZombieAI>();

        // Animator and clips
        var animator = zombie.AddComponent<Animator>();
        var controller = AnimatorController.CreateAnimatorControllerAtPath(prefabDir + "/Zombie.controller");
        controller.AddParameter("Speed", AnimatorControllerParameterType.Float);
        var idleClip = new AnimationClip();
        idleClip.name = "zombie_idle";
        // tiny bob animation
        AnimationUtility.SetAnimationType(idleClip, ModelImporterAnimationType.Generic);
        var curve = AnimationCurve.EaseInOut(0, 0, 1, 0);
        idleClip.SetCurve("", typeof(Transform), "localPosition.y", curve);
        AssetDatabase.CreateAsset(idleClip, prefabDir + "/zombie_idle.anim");
        var walkClip = new AnimationClip();
        walkClip.name = "zombie_walk";
        AssetDatabase.CreateAsset(walkClip, prefabDir + "/zombie_walk.anim");
        var idleState = controller.layers[0].stateMachine.AddState("Idle");
        idleState.motion = idleClip;
        var walkState = controller.layers[0].stateMachine.AddState("Walk");
        walkState.motion = walkClip;
        var trans = idleState.AddTransition(walkState);
        trans.AddCondition(AnimatorConditionMode.Greater, 0.1f, "Speed");
        var trans2 = walkState.AddTransition(idleState);
        trans2.AddCondition(AnimatorConditionMode.Less, 0.1f, "Speed");
        animator.runtimeAnimatorController = controller;

        // Save prefab
        string path = prefabDir + "/Zombie.prefab";
        PrefabUtility.SaveAsPrefabAsset(zombie, path);
        GameObject.DestroyImmediate(zombie);

        // Player prefab
        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "PlayerModel";
        player.AddComponent<Assets.Scripts.PlayerController>();
        player.AddComponent<Assets.Scripts.PlayerStats>();
        player.AddComponent<Assets.Scripts.Inventory>();
        Animator playerAnimator = player.AddComponent<Animator>();
        var pCtrl = AnimatorController.CreateAnimatorControllerAtPath(prefabDir + "/Player.controller");
        AssetDatabase.CreateAsset(pCtrl, prefabDir + "/Player.controller");
        playerAnimator.runtimeAnimatorController = pCtrl;
        PrefabUtility.SaveAsPrefabAsset(player, prefabDir + "/Player.prefab");
        GameObject.DestroyImmediate(player);

        // Gun prefab
        GameObject gun = GameObject.CreatePrimitive(PrimitiveType.Cube);
        gun.name = "GunModel";
        gun.transform.localScale = new Vector3(0.3f, 0.15f, 0.9f);
        var gunComp = gun.AddComponent<Assets.Scripts.Gun>();
        gunComp.damagePercent = 20f;
        gunComp.fireRate = 4f;
        gunComp.magazineSize = 12;
        PrefabUtility.SaveAsPrefabAsset(gun, prefabDir + "/Gun.prefab");
        GameObject.DestroyImmediate(gun);

        // Vehicle prefab (simple cube)
        GameObject car = GameObject.CreatePrimitive(PrimitiveType.Cube);
        car.name = "CarModel";
        car.transform.localScale = new Vector3(2f, 1f, 4f);
        car.AddComponent<Rigidbody>();
        car.AddComponent<Assets.Scripts.VehicleController>();
        PrefabUtility.SaveAsPrefabAsset(car, prefabDir + "/Car.prefab");
        GameObject.DestroyImmediate(car);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Example assets imported to " + prefabDir);
    }
}
#endif
