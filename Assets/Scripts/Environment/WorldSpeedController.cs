using UnityEngine;

/// <summary>
/// Gestiona la velocidad global de desplazamiento del mundo en el Endless Runner.
/// Acelera de forma progresiva según la configuración de RunnerConfigSO.
/// </summary>
public class WorldSpeedController : MonoBehaviour
{
    public static WorldSpeedController Instance { get; private set; }

    [Header("Configuración")]
    [SerializeField] private RunnerConfigSO config;

    private float currentSpeed;
    private bool isPaused = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (config == null)
        {
            config = RunnerConfigSO.CreateDefault();
        }

        currentSpeed = config.InitialScrollSpeed;
    }

    void Update()
    {
        if (isPaused) return;

        // Aceleración gradual con el paso del tiempo
        if (currentSpeed < config.MaxScrollSpeed)
        {
            currentSpeed += config.SpeedIncreasePerSecond * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, config.MaxScrollSpeed);
        }
    }

    public float CurrentSpeed => isPaused ? 0f : currentSpeed;

    public void SetPaused(bool paused)
    {
        isPaused = paused;
    }

    public void ResetSpeed()
    {
        currentSpeed = config.InitialScrollSpeed;
        isPaused = false;
    }
}
