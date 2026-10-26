#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Utilidad del Editor para crear y configurar las 2 escenas requeridas por TP05.2
/// y añadirlas a los Build Settings de Unity.
/// </summary>
public static class RunnerSceneBuilder
{
    private const string MainMenuScenePath = "Assets/Scenes/MainMenuScene.unity";
    private const string GameplayScenePath = "Assets/Scenes/GameplayScene.unity";

    [MenuItem("Runner2D/Setup Build Settings & Scenes")]
    public static void SetupScenes()
    {
        // 1. Crear MainMenuScene si no existe
        if (!System.IO.File.Exists(MainMenuScenePath))
        {
            Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            
            // Configurar AudioManager inicial
            GameObject audioMgr = new GameObject("AudioManager");
            audioMgr.AddComponent<AudioManager>();

            // Configurar MainMenu UI Root
            GameObject menuMgr = new GameObject("MainMenuManager");
            menuMgr.AddComponent<MainMenuManager>();

            EditorSceneManager.SaveScene(newScene, MainMenuScenePath);
            Debug.Log($"[Runner2D] Created scene: {MainMenuScenePath}");
        }

        // 2. Crear GameplayScene si no existe
        if (!System.IO.File.Exists(GameplayScenePath))
        {
            Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

            // World Speed Controller
            GameObject speedCtrl = new GameObject("WorldSpeedController");
            speedCtrl.AddComponent<WorldSpeedController>();

            // Parallax Controller
            GameObject parallax = new GameObject("ParallaxController");
            parallax.AddComponent<ParallaxController>();

            // Scrolling Ground
            GameObject ground = new GameObject("ScrollingGround");
            ground.AddComponent<ScrollingGround2D>();

            // Runner Spawner
            GameObject spawner = new GameObject("RunnerSpawner");
            spawner.AddComponent<RunnerSpawner>();

            // HUD
            GameObject hud = new GameObject("RunnerHUD");
            hud.AddComponent<RunnerHUD>();

            // Pause Menu
            GameObject pause = new GameObject("PauseMenuManager");
            pause.AddComponent<PauseMenuManager>();

            EditorSceneManager.SaveScene(newScene, GameplayScenePath);
            Debug.Log($"[Runner2D] Created scene: {GameplayScenePath}");
        }

        // 3. Registrar ambas escenas en EditorBuildSettings (TP05.2)
        EditorBuildSettingsScene[] buildScenes = new EditorBuildSettingsScene[]
        {
            new EditorBuildSettingsScene(MainMenuScenePath, true),
            new EditorBuildSettingsScene(GameplayScenePath, true)
        };
        EditorBuildSettings.scenes = buildScenes;
        Debug.Log("[Runner2D] Scenes successfully configured in EditorBuildSettings!");
    }
}
#endif
