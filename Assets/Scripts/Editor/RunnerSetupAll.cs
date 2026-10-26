#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class RunnerSetupAll
{
    [MenuItem("Runner2D/Build Complete Project Setup (One-Click)")]
    public static void RunAll()
    {
        Debug.Log("[Runner2D] Starting Complete Project Setup...");
        
        RunnerAssetGenerator.GenerateAssets();
        RunnerPlayerPrefabBuilder.CreatePlayer();
        RunnerSceneBuilder.SetupScenes();

        Debug.Log("[Runner2D] Complete Project Setup finished successfully!");
    }
}
#endif
