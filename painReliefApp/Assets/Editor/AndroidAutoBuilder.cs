#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

public static class AndroidAutoBuilder
{
    [MenuItem("Tools/Build Android APK")]
    public static void BuildAndroidAPK()
    {
        // Ensure Android is selected
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);

        // collect enabled scenes in Build Settings
        var scenesList = new System.Collections.Generic.List<string>();
        for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            if (EditorBuildSettings.scenes[i].enabled) scenesList.Add(EditorBuildSettings.scenes[i].path);
        }
        if (scenesList.Count == 0)
        {
            // fallback to sample scene if present
            string sample = "Assets/Scenes/SampleScene.unity";
            if (File.Exists(sample)) scenesList.Add(sample);
        }

        if (scenesList.Count == 0)
        {
            EditorUtility.DisplayDialog("No Scenes", "No scenes found in Build Settings and SampleScene.unity not found. Add at least one scene to build.", "OK");
            return;
        }

        string[] scenes = scenesList.ToArray();
        string outputDir = "Builds/Android";
        if (!Directory.Exists(outputDir)) Directory.CreateDirectory(outputDir);
        string apkPath = Path.Combine(outputDir, "PainReliefApp-debug.apk");
        string aabPath = Path.Combine(outputDir, "PainReliefApp.aab");

        // Build quick debug APK (signed with debug keystore) for immediate install
        try
        {
            EditorUserBuildSettings.buildAppBundle = false;
            BuildPlayerOptions optApk = new BuildPlayerOptions();
            optApk.scenes = scenes;
            optApk.locationPathName = apkPath;
            optApk.target = BuildTarget.Android;
            optApk.options = BuildOptions.Development; // debug-signed for easy install

            Debug.Log("Starting Android debug APK build to " + apkPath);
            var reportApk = BuildPipeline.BuildPlayer(optApk);
            if (reportApk.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
                Debug.Log("APK build succeeded: " + apkPath);
            else
                Debug.LogError("APK build failed: " + reportApk.summary.result.ToString());
        }
        catch (System.Exception ex)
        {
            Debug.LogError("APK build exception: " + ex.Message);
        }

        // Build Android App Bundle (AAB) suitable for Play Store (requires proper signing)
        try
        {
            EditorUserBuildSettings.buildAppBundle = true;
            BuildPlayerOptions optAab = new BuildPlayerOptions();
            optAab.scenes = scenes;
            optAab.locationPathName = aabPath;
            optAab.target = BuildTarget.Android;
            optAab.options = BuildOptions.None;

            Debug.Log("Starting Android App Bundle build to " + aabPath);
            var reportAab = BuildPipeline.BuildPlayer(optAab);
            if (reportAab.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
                Debug.Log("AAB build succeeded: " + aabPath);
            else
                Debug.LogError("AAB build failed: " + reportAab.summary.result.ToString());
        }
        catch (System.Exception ex)
        {
            Debug.LogError("AAB build exception: " + ex.Message);
        }

        // reset preference
        EditorUserBuildSettings.buildAppBundle = false;
    }
}
#endif
