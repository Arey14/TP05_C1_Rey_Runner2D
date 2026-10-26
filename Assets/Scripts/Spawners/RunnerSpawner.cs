using System.Collections;
using UnityEngine;

/// <summary>
/// Generador modular de obstáculos y coleccionables en posiciones y tiempos aleatorios.
/// Cumple con la consigna de TP05.1 ("Los obstáculos o plataformas aparecen en posiciones aleatorias").
/// </summary>
public class RunnerSpawner : MonoBehaviour
{
    [Header("Configuración Orientada a Datos")]
    [SerializeField] private RunnerConfigSO runnerConfig;

    [Header("Prefabs de Obstáculos")]
    [SerializeField] private GameObject[] obstaclePrefabs;
    [SerializeField] private Sprite fallbackObstacleSprite;

    [Header("Prefabs de Power-Ups")]
    [SerializeField] private GameObject invincibilityPrefab;
    [SerializeField] private GameObject extraLifePrefab;

    [Header("Rangos Verticales de Spawn")]
    [Tooltip("Altura Y para obstáculos a nivel de suelo.")]
    [SerializeField] private float groundObstacleY = -1.2f;

    [Tooltip("Rango de altura Y para obstáculos o plataformas aéreas.")]
    [SerializeField] private Vector2 airObstacleYRange = new Vector2(0.5f, 2.5f);

    [Tooltip("Probabilidad de que un obstáculo sea aéreo (0 = todos en suelo, 1 = todos aéreos).")]
    [Range(0f, 1f)]
    [SerializeField] private float airObstacleChance = 0.3f;

    [Tooltip("Probabilidad de que el ítem spawneado sea Invencibilidad vs Vida.")]
    [Range(0f, 1f)]
    [SerializeField] private float invincibilityChance = 0.5f;

    private Coroutine obstacleCoroutine;
    private Coroutine pickableCoroutine;
    private bool isSpawning = true;

    void OnEnable()
    {
        PlayerController2D.OnPlayerDied += StopSpawning;
    }

    void OnDisable()
    {
        PlayerController2D.OnPlayerDied -= StopSpawning;
    }

    void Start()
    {
        if (runnerConfig == null)
        {
            runnerConfig = RunnerConfigSO.CreateDefault();
        }

        isSpawning = true;
        obstacleCoroutine = StartCoroutine(ObstacleSpawnLoop());
        pickableCoroutine = StartCoroutine(PickableSpawnLoop());
    }

    private IEnumerator ObstacleSpawnLoop()
    {
        while (isSpawning)
        {
            float waitTime = Random.Range(runnerConfig.MinObstacleInterval, runnerConfig.MaxObstacleInterval);
            yield return new WaitForSeconds(waitTime);

            if (isSpawning)
            {
                SpawnRandomObstacle();
            }
        }
    }

    private IEnumerator PickableSpawnLoop()
    {
        while (isSpawning)
        {
            float waitTime = Random.Range(runnerConfig.MinPickableInterval, runnerConfig.MaxPickableInterval);
            yield return new WaitForSeconds(waitTime);

            if (isSpawning)
            {
                SpawnRandomPickable();
            }
        }
    }

    public void SpawnRandomObstacle()
    {
        float spawnY = groundObstacleY;
        if (Random.value < airObstacleChance)
        {
            spawnY = Random.Range(airObstacleYRange.x, airObstacleYRange.y);
        }

        Vector3 spawnPos = new Vector3(runnerConfig.SpawnXPosition, spawnY, 0f);

        if (obstaclePrefabs != null && obstaclePrefabs.Length > 0)
        {
            int index = Random.Range(0, obstaclePrefabs.Length);
            if (obstaclePrefabs[index] != null)
            {
                Instantiate(obstaclePrefabs[index], spawnPos, Quaternion.identity);
                return;
            }
        }

        // Fallback procedural con ScrollingObstacle2D
        CreateProceduralObstacle(spawnPos);
    }

    private void CreateProceduralObstacle(Vector3 position)
    {
        GameObject obj = new GameObject("Obstacle_Dynamic");
        obj.transform.position = position;

        SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
        if (fallbackObstacleSprite != null)
        {
            sr.sprite = fallbackObstacleSprite;
        }
        else
        {
            Texture2D tex = Texture2D.whiteTexture;
            sr.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 1f);
            sr.color = new Color(0.9f, 0.3f, 0.2f, 1f);
            obj.transform.localScale = new Vector3(0.8f, 1.2f, 1f);
        }
        sr.sortingOrder = 5;

        BoxCollider2D col = obj.AddComponent<BoxCollider2D>();
        col.isTrigger = true;

        obj.AddComponent<ScrollingObstacle2D>();
    }

    public void SpawnRandomPickable()
    {
        float spawnY = Random.Range(airObstacleYRange.x, airObstacleYRange.y);
        Vector3 spawnPos = new Vector3(runnerConfig.SpawnXPosition, spawnY, 0f);

        bool isInvincible = Random.value < invincibilityChance;
        GameObject prefab = isInvincible ? invincibilityPrefab : extraLifePrefab;

        if (prefab != null)
        {
            Instantiate(prefab, spawnPos, Quaternion.identity);
        }
        else
        {
            CreateProceduralPickable(spawnPos, isInvincible ? PowerUpType.Invincibility : PowerUpType.ExtraLife);
        }
    }

    private void CreateProceduralPickable(Vector3 position, PowerUpType type)
    {
        GameObject obj = new GameObject(type == PowerUpType.Invincibility ? "PowerUp_Invincibility" : "PowerUp_ExtraLife");
        obj.transform.position = position;

        SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
        Texture2D tex = Texture2D.whiteTexture;
        sr.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 1f);
        sr.color = type == PowerUpType.Invincibility ? new Color(1f, 0.85f, 0.1f, 1f) : new Color(0.2f, 0.9f, 0.3f, 1f);
        obj.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
        sr.sortingOrder = 6;

        CircleCollider2D col = obj.AddComponent<CircleCollider2D>();
        col.isTrigger = true;

        PickableItem item = obj.AddComponent<PickableItem>();
        // Serializar campos o usar defaults
    }

    public void StopSpawning()
    {
        isSpawning = false;
        if (obstacleCoroutine != null) StopCoroutine(obstacleCoroutine);
        if (pickableCoroutine != null) StopCoroutine(pickableCoroutine);
    }
}
