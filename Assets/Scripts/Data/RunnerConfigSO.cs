using UnityEngine;

/// <summary>
/// Configuración global de la partida del Endless Runner.
/// Maneja velocidades de scroll progresivas, rangos de spawn y umbrales de limpieza.
/// </summary>
[CreateAssetMenu(fileName = "RunnerConfig", menuName = "Runner2D/RunnerConfig")]
public class RunnerConfigSO : ScriptableObject
{
    [Header("Velocidad de Scroll y Progresión")]
    [Tooltip("Velocidad inicial con la que se desplaza el escenario hacia la izquierda.")]
    [SerializeField] private float initialScrollSpeed = 7f;

    [Tooltip("Aceleración gradual de la velocidad por cada segundo de supervivencia.")]
    [SerializeField] private float speedIncreasePerSecond = 0.05f;

    [Tooltip("Velocidad máxima absoluta de scroll.")]
    [SerializeField] private float maxScrollSpeed = 16f;

    [Header("Intervalos de Spawn de Obstáculos")]
    [Tooltip("Tiempo mínimo entre la aparición de dos obstáculos.")]
    [SerializeField] private float minObstacleInterval = 1.4f;

    [Tooltip("Tiempo máximo entre la aparición de dos obstáculos.")]
    [SerializeField] private float maxObstacleInterval = 3.2f;

    [Header("Intervalos de Spawn de Coleccionables / Power-Ups")]
    [Tooltip("Tiempo mínimo entre la aparición de ítems recogibles.")]
    [SerializeField] private float minPickableInterval = 4f;

    [Tooltip("Tiempo máximo entre la aparición de ítems recogibles.")]
    [SerializeField] private float maxPickableInterval = 8.5f;

    [Header("Límites de Pantalla y Despawn")]
    [Tooltip("Posición X donde se instancian los objetos (fuera de la pantalla a la derecha).")]
    [SerializeField] private float spawnXPosition = 12f;

    [Tooltip("Posición X donde los objetos son reciclados o destruidos (fuera a la izquierda).")]
    [SerializeField] private float despawnXPosition = -14f;

    public float InitialScrollSpeed => initialScrollSpeed;
    public float SpeedIncreasePerSecond => speedIncreasePerSecond;
    public float MaxScrollSpeed => maxScrollSpeed;
    public float MinObstacleInterval => minObstacleInterval;
    public float MaxObstacleInterval => maxObstacleInterval;
    public float MinPickableInterval => minPickableInterval;
    public float MaxPickableInterval => maxPickableInterval;
    public float SpawnXPosition => spawnXPosition;
    public float DespawnXPosition => despawnXPosition;

    public static RunnerConfigSO CreateDefault()
    {
        RunnerConfigSO config = CreateInstance<RunnerConfigSO>();
        config.initialScrollSpeed = 7f;
        config.speedIncreasePerSecond = 0.05f;
        config.maxScrollSpeed = 16f;
        config.minObstacleInterval = 1.4f;
        config.maxObstacleInterval = 3.2f;
        config.minPickableInterval = 4f;
        config.maxPickableInterval = 8.5f;
        config.spawnXPosition = 12f;
        config.despawnXPosition = -14f;
        return config;
    }
}
