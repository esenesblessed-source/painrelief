#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class AndroidConfigurator
{
    [MenuItem("Tools/Configure Android Build")]
    public static void ConfigureAndroid()
    {
        // Switch build target to Android
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);

        // Basic PlayerSettings
        PlayerSettings.applicationIdentifier = "com.yourcompany.painrelief";
        PlayerSettings.companyName = "YourCompany";
        PlayerSettings.productName = "PainReliefApp";

        // Set default minimum API level to Android 7.0 (API 24)
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel24;

        // Use Gradle build system
        EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;

        // Default scripting backend: IL2CPP for release performance (can be switched to Mono for faster iteration)
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);

        // Enable ARM64 target (modern requirement for Google Play)
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7 | AndroidArchitecture.ARM64;

        // Configure default graphics API to Vulkan + OpenGLES3 fallback
        GraphicsDeviceType[] apis = { GraphicsDeviceType.Vulkan, GraphicsDeviceType.OpenGLES3 };
        PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, apis);

        AssetDatabase.SaveAssets();
        Debug.Log("Android build configured: package/com set, target switched. Review Player Settings for keystore and signing before building.");
    }
}
#endif
