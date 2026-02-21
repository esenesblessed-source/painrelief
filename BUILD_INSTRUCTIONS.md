# Build & Setup Instructions

1) Install Unity Hub: https://unity.com/download
2) Install Unity Editor 2022.3 LTS via Unity Hub.
3) Open Unity Hub -> "Add" -> select this project folder (`painReliefApp`).
4) Open the project in the Editor, create a new Scene (or open `SampleScene`).
5) Create an empty GameObject named `GameManager` and attach `GameManager` script.
6) Create a Capsule, add `CharacterController`, attach `PlayerController` to it.
7) Create a Camera as child of the Capsule, attach `SimpleCameraController` and set `player` reference.

8) To quickly prototype a level in-game, create an empty GameObject named `LevelGenerator` and attach the `LevelGenerator` script.
	- Optionally create a Capsule GameObject, attach `EnemyPatrol` and drag that Capsule to the `enemyPrefab` field on `LevelGenerator`.
	- When you press Play, `LevelGenerator` will spawn a grid of floor tiles and the configured enemies.

Camera modes:
- Attach `SimpleCameraController` to your main Camera and set the `player` reference to the player Capsule.
- Attach `CameraModeController` to any GameObject (for example `GameManager`) to enable in-game toggling.
- Press `V` in play mode to toggle between first- and third-person views.

Weapons and shooting:
- Add `WeaponManager` to the player object and assign a `Gun` prefab (or `Gun` component) to the `slotSmall` and `slotBig` fields.
- Default input keys in the prototype: Left Mouse = Fire (`Fire1`), Right Mouse = Aim (`Mouse1`), `R` = Reload, `Q` = Switch weapon.
- Guns use raycast-based hits and apply `IDamageable.ApplyDamage(percent)` to targets.
- Create `AmmoPickup` prefabs to add reserve ammo when picked up by the player.



To build for Windows or WebGL, use Unity's Build Settings (File -> Build Settings).

NavMesh & Zombies:
- To enable zombie pathfinding, open `Window -> AI -> Navigation` and bake the NavMesh for your level geometry.
- Create a `ZombieSpawner` GameObject and assign a `Zombie` prefab (with `NavMeshAgent` and `ZombieAI`) to `zombiePrefab`.
- Make sure the player GameObject has the tag `Player` so zombies can find it.
- Add a `SoundManager` GameObject (attach `SoundManager` script) so zombies can hear gunshots and running.

Time & Weather:
- Add a `TimeOfDay` GameObject and attach `TimeOfDay` script. Assign the scene sun (Directional Light) to `sunLight`.
- Add a `WeatherSystem` GameObject and attach `WeatherSystem` script to control fog and cloud coverage.
- Zombies will automatically adjust sight/hearing based on time of day and weather: night reduces sight but increases hearing reliance; fog reduces visibility.

Vehicles:
- Create a vehicle GameObject with a `Rigidbody` and add the `VehicleController` script. Set `maxTorque`, `maxSteerAngle`, and `maxSpeed` to tune handling.
- Add a trigger collider near the driver's door and attach `VehicleEnterExit` to it. When the player is inside the trigger press `E` to enter the vehicle; press `E` again to exit.
- The prototype uses a simple camera parent/child swap while driving—adjust camera positions on the vehicle for a better feel.
- For realistic driving, replace this simple controller with a wheel-collider-based vehicle package or Unity's vehicle assets.

Mobile Controls (Android):
- Create a `Canvas` in Screen Space - Overlay and design your mobile HUD: place an `OnScreenJoystick` (use an Image for `background` and a child Image for `handle`) in the lower-left for movement.
- Add `MobileInput` to a persistent GameObject (for example `GameManager`) and reference the `OnScreenJoystick` instance.
- Add UI buttons for `Fire`, `Aim`, `Jump`, `Run`, `Switch`, and `EnterExit`. Attach `MobileUIButton` to each and set `buttonName` accordingly. For toggle/click buttons (like `Switch`), enable `useClick`.
- In your player control/weapon code, read `MobileInput.Instance.move`, `fire`, `aim`, `jump`, `run`, and `switchWeapon` to control movement and actions when running on mobile.
- The provided mobile input scripts are simple building blocks; you should refine deadzones, visual feedback, and layout for different screen sizes.

UI / HUD:
- Create a `Canvas` and add a `HUDManager` component on a persistent GameObject (for example `GameManager`). Assign `healthSlider` (UI Slider) and `staminaSlider` to it.
- Create a simple `HealthBar`/`StaminaBar` using `Slider` UI elements. Link them to the `HUDManager`.
- For minimap: add a secondary Camera set to `Depth` above the scene, set its `Clear Flags` to `Solid Color` or `Depth only`, render to a small `RenderTexture`, and display that texture in a RawImage on the HUD. Attach `Minimap` script to the minimap Camera and set the `player`.
- For inventory: create a panel with a ScrollView, set the `content` RectTransform, and use a small UI prefab (Text + Button) as `inventoryItemPrefab`. Attach `InventoryUI` and point it at your `Inventory` component.
- `HUDManager` toggles the inventory panel with `I` (or mobile `enterExit` input) and updates health/stamina each frame.

Audio & VFX:
- Add an `AudioManager` GameObject and assign audio clips for `gunshotClip`, `footstepClip`, `zombieGrowlClip`, and `carEngineClip`.
- Add a `VFXManager` GameObject and assign `muzzleFlashPrefab`, `bloodPrefab`, and `impactPrefab` (ParticleSystem prefabs).
- `Gun` now calls `AudioManager.PlayGunshot` and `VFXManager.SpawnMuzzleFlash` can be invoked from gun muzzle positions to show flashes.
- Add `FootstepEmitter` to the player to automatically play footstep SFX while moving.
- Audio and VFX assets should be placed under `Assets/Audio` and `Assets/VFX`. Use small, loopable engine audio for vehicles and one-shot clips for effects.

Placeholder assets:
- This repo doesn't include commercial audio or art. Use licensed assets from the Unity Asset Store or your own files. Place them under `Assets/Audio` and `Assets/Models`.

Placeholder generation:
- The project includes procedural placeholder audio and particle fallbacks so the game can run without external assets. `AudioManager` will auto-generate simple clips at runtime if no clips are assigned. `VFXManager` creates simple particle systems if no prefabs are provided.
- You can replace generated assets by assigning your own clips and particle prefabs to `AudioManager` and `VFXManager` in the scene.

Importing example assets & Android configuration:
- In Unity Editor, use `Tools -> Import Example Assets` to create simple placeholder prefabs (Player, Zombie, Gun, Car) under `Assets/Prefabs/ExampleAssets`.
- After importing, drag the prefabs into your scene or assign to spawners/managers as needed.
- Use `Tools -> Configure Android Build` to switch the project build target to Android and apply a set of recommended PlayerSettings (package name, build system = Gradle, scripting backend = IL2CPP, ARM64 enabled). Review and set keystore/signing before building a release APK.

Automated Android build:
- Use `Tools -> Build Android APK` to run an automated build that packages `Assets/Scenes/SampleScene.unity` into `Builds/Android/PainReliefApp.apk`.
- Review and configure `Project Settings -> Player -> Publishing Settings` for keystore and signing before performing release builds. The automated build will warn if no keystore is configured.

Command-line quick build (Windows):
- You can run the build from PowerShell using `build_android.ps1`. Example (replace UNITY_PATH):

```powershell
.
\build_android.ps1 -unityPath "C:\Program Files\Unity\Hub\Editor\2022.3.x\Editor\Unity.exe" -projectPath "C:\Path\To\painReliefApp"
```

- The script runs Unity in batch mode and calls `AndroidAutoBuilder.BuildAndroidAPK`. Output APK/AAB are placed in `Builds/Android/`.

Optimization tips:
- Use Unity's Profiler (Window -> Analysis -> Profiler) to find CPU/GPU hotspots.
- Mark static geometry as `Static` for static batching and bake lightmaps where appropriate.
- Use `MeshCombiner` (component) to combine static props into fewer meshes to reduce draw calls.
- Use LODGroups on distant objects to lower polygon counts at range.
- Enable GPU instancing on materials that support it.
- Use occlusion culling for dense urban scenes (Window -> Rendering -> Occlusion Culling) and bake occlusion data.
- For Android, prefer compressed textures (ETC2) and reduce shadow resolution and distance for mobile targets.
- Use `FPSDisplay` and `AutoQualityManager` scripts (in `Assets/Scripts/Optimization`) to observe performance and let the game adapt quality at runtime.



Integration notes:
- `PlayerController` now reads `MobileInput.Instance.move` and `MobileInput.Instance.jump` (jump is consumed on use).
- `WeaponManager` reads `MobileInput.Instance.fire`, `aim`, and consumes `switchWeapon` when switching weapons.
- Add UI and wire buttons using `MobileUIButton` to test mobile interactions.




