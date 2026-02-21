# painReliefApp — Unity 3D Starter

This repository is a starter scaffold for a Unity 3D game project.

Recommended Unity version: 2022.3 LTS

Included:
- `Assets/Scripts/PlayerController.cs` — basic movement
- `Assets/Scripts/SimpleCameraController.cs` — mouse look
- `Assets/Scripts/GameManager.cs` — singleton manager
- `Packages/manifest.json` — minimal packages file
- `BUILD_INSTRUCTIONS.md` — how to install Unity and open the project

Editor tools:
- `Assets/Editor/UIBuilderMenu.cs` — adds "Tools/Build Example UI" to create example HUD prefabs and UI in the current scene.
- `Assets/Editor/SampleSceneBuilder.cs` — adds "Tools/Create Sample Scene" to scaffold a starter scene and save it to `Assets/Scenes/SampleScene.unity`.

Next steps:
1. Install Unity (see BUILD_INSTRUCTIONS.md).
2. Open this folder as a Unity project.
3. Create a scene, add a `CharacterController` and attach `PlayerController` and `SimpleCameraController`.
