#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class RunnerPlayerPrefabBuilder
{
    private const string DragonSpritePath = "Assets/Sprites/Dragon.png";
    private const string PrefabSavePath = "Assets/Prefabs/Player_Dragon.prefab";

    [MenuItem("Runner2D/Create Player Prefab")]
    public static void CreatePlayer()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Prefabs"))
        {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        }

        GameObject playerObj = new GameObject("Player_Dragon");
        playerObj.transform.position = new Vector3(-5f, -1f, 0f);

        // 1. SpriteRenderer
        SpriteRenderer sr = playerObj.AddComponent<SpriteRenderer>();
        Sprite dragonSprite = AssetDatabase.LoadAssetAtPath<Sprite>(DragonSpritePath);
        if (dragonSprite != null)
        {
            sr.sprite = dragonSprite;
            sr.sortingOrder = 10;
        }

        // 2. Rigidbody2D
        Rigidbody2D rb = playerObj.AddComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        // 3. CapsuleCollider2D
        CapsuleCollider2D col = playerObj.AddComponent<CapsuleCollider2D>();
        col.size = new Vector2(1.2f, 1.2f);

        // 4. TrailRenderer
        TrailRenderer trail = playerObj.AddComponent<TrailRenderer>();
        trail.time = 0.35f;
        trail.startWidth = 0.5f;
        trail.endWidth = 0.05f;
        trail.emitting = false;
        trail.sortingOrder = 9;

        // Gradient para la estela (Fuego / Mágico)
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(new Color(0.2f, 0.8f, 1f), 0f), new GradientColorKey(new Color(1f, 0.4f, 0.1f), 1f) },
            new GradientAlphaKey[] { new GradientAlphaKey(0.8f, 0f), new GradientAlphaKey(0f, 1f) }
        );
        trail.colorGradient = gradient;

        // 5. GroundCheck Child
        GameObject groundCheck = new GameObject("GroundCheck");
        groundCheck.transform.SetParent(playerObj.transform);
        groundCheck.transform.localPosition = new Vector3(0f, -0.65f, 0f);

        // 6. ParticleSystem Child (Polvo de aterrizaje)
        GameObject dustObj = new GameObject("LandingDustParticles");
        dustObj.transform.SetParent(playerObj.transform);
        dustObj.transform.localPosition = new Vector3(0f, -0.65f, 0f);

        ParticleSystem ps = dustObj.AddComponent<ParticleSystem>();
        var main = ps.main;
        main.playOnAwake = false;
        main.loop = false;
        main.duration = 0.4f;
        main.startLifetime = 0.3f;
        main.startSpeed = 2f;
        main.startSize = 0.25f;
        main.startColor = new Color(0.85f, 0.85f, 0.85f, 0.6f);

        var emission = ps.emission;
        emission.rateOverTime = 0;
        emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0f, 10) });

        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.3f;

        // 7. PlayerController2D Component
        PlayerController2D controller = playerObj.AddComponent<PlayerController2D>();

        // Asignar referencias por SerializedObject
        SerializedObject so = new SerializedObject(controller);
        so.FindProperty("groundCheckPoint").objectReferenceValue = groundCheck.transform;
        so.FindProperty("dragonTailTrail").objectReferenceValue = trail;
        so.FindProperty("landingDustParticles").objectReferenceValue = ps;
        so.FindProperty("spriteRenderer").objectReferenceValue = sr;
        so.FindProperty("groundLayer").intValue = LayerMask.GetMask("Default", "Ground");

        PlayerStatsSO stats = AssetDatabase.LoadAssetAtPath<PlayerStatsSO>("Assets/Data/PlayerStats.asset");
        if (stats != null)
        {
            so.FindProperty("playerStats").objectReferenceValue = stats;
        }
        so.ApplyModifiedProperties();

        // Guardar Prefab
        PrefabUtility.SaveAsPrefabAsset(playerObj, PrefabSavePath);
        GameObject.DestroyImmediate(playerObj);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"[Runner2D] Player Prefab successfully created at: {PrefabSavePath}");
    }
}
#endif
