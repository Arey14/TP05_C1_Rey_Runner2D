#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

public static class RunnerAssetGenerator
{
    private const string TargetFolder = "Assets/Data";

    [MenuItem("Runner2D/Generate Default Assets")]
    public static void GenerateAssets()
    {
        if (!AssetDatabase.IsValidFolder(TargetFolder))
        {
            AssetDatabase.CreateFolder("Assets", "Data");
        }

        // 1. PlayerStatsSO
        string playerStatsPath = Path.Combine(TargetFolder, "PlayerStats.asset");
        if (AssetDatabase.LoadAssetAtPath<PlayerStatsSO>(playerStatsPath) == null)
        {
            PlayerStatsSO stats = PlayerStatsSO.CreateDefault();
            AssetDatabase.CreateAsset(stats, playerStatsPath);
            Debug.Log($"[Runner2D] Created: {playerStatsPath}");
        }

        // 2. RunnerConfigSO
        string runnerConfigPath = Path.Combine(TargetFolder, "RunnerConfig.asset");
        if (AssetDatabase.LoadAssetAtPath<RunnerConfigSO>(runnerConfigPath) == null)
        {
            RunnerConfigSO config = RunnerConfigSO.CreateDefault();
            AssetDatabase.CreateAsset(config, runnerConfigPath);
            Debug.Log($"[Runner2D] Created: {runnerConfigPath}");
        }

        // 3. BiomeDataSO
        string biomePath = Path.Combine(TargetFolder, "DefaultBiome.asset");
        if (AssetDatabase.LoadAssetAtPath<BiomeDataSO>(biomePath) == null)
        {
            BiomeDataSO biome = ScriptableObject.CreateInstance<BiomeDataSO>();
            AssetDatabase.CreateAsset(biome, biomePath);
            Debug.Log($"[Runner2D] Created: {biomePath}");
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
#endif
