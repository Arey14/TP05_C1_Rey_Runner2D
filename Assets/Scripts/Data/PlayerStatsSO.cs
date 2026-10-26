using UnityEngine;

/// <summary>
/// ScriptableObject que centraliza todos los datos iniciales y parámetros de balance del jugador.
/// Cumple con la consigna de TP05.1 y los estándares data-driven de CCGS.
/// </summary>
[CreateAssetMenu(fileName = "PlayerStats", menuName = "Runner2D/PlayerStats")]
public class PlayerStatsSO : ScriptableObject
{
    [Header("Vidas y Salud")]
    [Tooltip("Cantidad inicial de vidas con las que comienza el dragón.")]
    [SerializeField] private int startingLives = 3;

    [Tooltip("Máximo de vidas que puede acumular con power-ups.")]
    [SerializeField] private int maxLives = 5;

    [Header("Mecánica de Salto Variable")]
    [Tooltip("Impulso vertical inicial al presionar la tecla de salto.")]
    [SerializeField] private float initialJumpForce = 12f;

    [Tooltip("Fuerza adicional aplicada mientras se mantiene presionada la tecla de salto.")]
    [SerializeField] private float sustainedJumpForce = 18f;

    [Tooltip("Tiempo máximo en segundos durante el cual se puede sostener el salto.")]
    [SerializeField] private float maxJumpHoldTime = 0.25f;

    [Tooltip("Escala de gravedad base aplicada al Rigidbody2D.")]
    [SerializeField] private float baseGravityScale = 3f;

    [Tooltip("Multiplicador de caída rápida cuando el jugador suelta el botón antes del ápice.")]
    [SerializeField] private float fallGravityMultiplier = 1.5f;

    [Header("Power-Ups")]
    [Tooltip("Duración en segundos del estado de invencibilidad.")]
    [SerializeField] private float invincibilityDuration = 5f;

    [Tooltip("Velocidad de parpadeo visual del sprite durante la invencibilidad.")]
    [SerializeField] private float invincibilityFlickerRate = 12f;

    // Getters públicos de solo lectura (inmutabilidad externa)
    public int StartingLives => startingLives;
    public int MaxLives => maxLives;
    public float InitialJumpForce => initialJumpForce;
    public float SustainedJumpForce => sustainedJumpForce;
    public float MaxJumpHoldTime => maxJumpHoldTime;
    public float BaseGravityScale => baseGravityScale;
    public float FallGravityMultiplier => fallGravityMultiplier;
    public float InvincibilityDuration => invincibilityDuration;
    public float InvincibilityFlickerRate => invincibilityFlickerRate;

    /// <summary>
    /// Factory estático para inicializar una instancia en tiempo de ejecución si no hay asset asignado.
    /// </summary>
    public static PlayerStatsSO CreateDefault()
    {
        PlayerStatsSO stats = CreateInstance<PlayerStatsSO>();
        stats.startingLives = 3;
        stats.maxLives = 5;
        stats.initialJumpForce = 12f;
        stats.sustainedJumpForce = 18f;
        stats.maxJumpHoldTime = 0.25f;
        stats.baseGravityScale = 3f;
        stats.fallGravityMultiplier = 1.5f;
        stats.invincibilityDuration = 5f;
        stats.invincibilityFlickerRate = 12f;
        return stats;
    }
}
