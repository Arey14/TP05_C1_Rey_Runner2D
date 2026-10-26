using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Controlador principal del jugador para el Endless Runner 2D.
/// Implementa salto variable, TrailRenderer, ParticleSystem en aterrizaje, invencibilidad y vidas.
/// Cumple con las reglas de CCGS: desacoplado de la UI mediante eventos C# y data-driven mediante PlayerStatsSO.
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerController2D : MonoBehaviour
{
    // ==========================================
    // Eventos C# desacoplados (CCGS Architecture)
    // ==========================================
    public static event Action<int> OnLivesChanged;
    public static event Action<float> OnInvincibilityTimerUpdated;
    public static event Action OnPlayerDied;
    public static event Action OnJumpTriggered;
    public static event Action OnLanded;

    [Header("Configuración Orientada a Datos")]
    [SerializeField] private PlayerStatsSO playerStats;

    [Header("Detección de Suelo")]
    [Tooltip("Punto de referencia para chequear contacto con el suelo.")]
    [SerializeField] private Transform groundCheckPoint;

    [Tooltip("Radio del círculo de detección de suelo.")]
    [SerializeField] private float groundCheckRadius = 0.2f;

    [Tooltip("Máscara de capas considerada como suelo.")]
    [SerializeField] private LayerMask groundLayer;

    [Header("Efectos Visuales (TP05.1 & TP05.2)")]
    [Tooltip("TrailRenderer en la cola del dragón para el salto.")]
    [SerializeField] private TrailRenderer dragonTailTrail;

    [Tooltip("ParticleSystem de polvo generado al aterrizar en el suelo.")]
    [SerializeField] private ParticleSystem landingDustParticles;

    [Tooltip("SpriteRenderer del dragón para feedback de parpadeo.")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    // Componentes y estado interno
    private Rigidbody2D rb;
    private Collider2D col;
    private bool isGrounded = false;
    private bool wasGrounded = true;
    private bool isJumping = false;
    private float jumpHoldTimer = 0f;

    private int currentLives;
    private float invincibilityTimer = 0f;
    private bool isInvincible = false;
    private float fixedXPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

        if (playerStats == null)
        {
            playerStats = PlayerStatsSO.CreateDefault();
        }

        fixedXPosition = transform.position.x;
        rb.gravityScale = playerStats.BaseGravityScale;
        currentLives = playerStats.StartingLives;
    }

    void Start()
    {
        OnLivesChanged?.Invoke(currentLives);
        if (dragonTailTrail != null) dragonTailTrail.emitting = false;
    }

    void Update()
    {
        CheckGroundStatus();
        HandleJumpInput();
        UpdateInvincibility();
        ApplyVisualRotation();
    }

    void FixedUpdate()
    {
        ApplyFallGravity();

        // Asegura que el personaje permanezca fijo en su carril X
        if (Mathf.Abs(transform.position.x - fixedXPosition) > 0.01f)
        {
            transform.position = new Vector3(fixedXPosition, transform.position.y, transform.position.z);
        }
    }

    // ==========================================
    // Mecánica de Salto y Detección de Suelo
    // ==========================================
    private void CheckGroundStatus()
    {
        Vector2 checkPos = groundCheckPoint != null ? (Vector2)groundCheckPoint.position : (Vector2)transform.position;
        isGrounded = Physics2D.OverlapCircle(checkPos, groundCheckRadius, groundLayer);

        // Feedback al aterrizar (ParticleSystem y Evento de Audio)
        if (!wasGrounded && isGrounded)
        {
            if (landingDustParticles != null)
            {
                landingDustParticles.Play();
            }
            if (dragonTailTrail != null)
            {
                dragonTailTrail.emitting = false;
            }
            OnLanded?.Invoke();
        }

        wasGrounded = isGrounded;
    }

    private void HandleJumpInput()
    {
        bool jumpPressed = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
        bool jumpHeld = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        bool jumpReleased = Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow);

        // 1. Inicio de salto
        if (jumpPressed && isGrounded)
        {
            isJumping = true;
            jumpHoldTimer = playerStats.MaxJumpHoldTime;

            rb.linearVelocity = new Vector2(0f, playerStats.InitialJumpForce);

            if (dragonTailTrail != null)
            {
                dragonTailTrail.emitting = true;
            }

            OnJumpTriggered?.Invoke();
        }

        // 2. Salto sostenido (variable height)
        if (jumpHeld && isJumping)
        {
            if (jumpHoldTimer > 0f)
            {
                rb.linearVelocity = new Vector2(0f, playerStats.SustainedJumpForce);
                jumpHoldTimer -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        // 3. Corte temprano al soltar la tecla
        if (jumpReleased)
        {
            isJumping = false;
        }
    }

    private void ApplyFallGravity()
    {
        if (rb.linearVelocity.y < 0f)
        {
            rb.gravityScale = playerStats.BaseGravityScale * playerStats.FallGravityMultiplier;
        }
        else
        {
            rb.gravityScale = playerStats.BaseGravityScale;
        }
    }

    private void ApplyVisualRotation()
    {
        // Inclinación aerodinámica procedural según velocidad vertical
        float targetTilt = Mathf.Clamp(rb.linearVelocity.y * 3.5f, -30f, 30f);
        transform.rotation = Quaternion.Euler(0f, 0f, targetTilt);
    }

    // ==========================================
    // Sistema de Vidas, Invencibilidad y Power-Ups
    // ==========================================
    public void TakeDamage()
    {
        if (isInvincible) return;

        currentLives--;
        OnLivesChanged?.Invoke(currentLives);

        if (currentLives <= 0)
        {
            Die();
        }
        else
        {
            // Otorga 1.5s de invencibilidad tras golpe recibido
            ActivateInvincibility(1.5f);
        }
    }

    public void ActivateInvincibility(float duration)
    {
        invincibilityTimer = Mathf.Max(invincibilityTimer, duration);
        isInvincible = true;
        StartCoroutine(InvincibilityVisualRoutine());
    }

    public void AddLife(int amount = 1)
    {
        currentLives = Mathf.Min(currentLives + amount, playerStats.MaxLives);
        OnLivesChanged?.Invoke(currentLives);
    }

    private void UpdateInvincibility()
    {
        if (invincibilityTimer > 0f)
        {
            invincibilityTimer -= Time.deltaTime;
            OnInvincibilityTimerUpdated?.Invoke(invincibilityTimer);

            if (invincibilityTimer <= 0f)
            {
                invincibilityTimer = 0f;
                isInvincible = false;
                OnInvincibilityTimerUpdated?.Invoke(0f);
            }
        }
    }

    private IEnumerator InvincibilityVisualRoutine()
    {
        if (spriteRenderer == null) yield break;

        Color originalColor = spriteRenderer.color;

        while (isInvincible)
        {
            float alpha = (Mathf.Sin(Time.time * playerStats.InvincibilityFlickerRate) + 1f) * 0.35f + 0.3f;
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        spriteRenderer.color = originalColor;
    }

    private void Die()
    {
        enabled = false;
        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true;
        OnPlayerDied?.Invoke();
    }

    public bool IsInvincible => isInvincible;
    public int CurrentLives => currentLives;

    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        }
    }
}
