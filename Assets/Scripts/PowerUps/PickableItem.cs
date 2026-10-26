using System;
using UnityEngine;

public enum PowerUpType
{
    Invincibility,
    ExtraLife
}

/// <summary>
/// Coleccionable / Power-Up con animación continua de subida/bajada y rotación.
/// Cumple con la consigna de TP05.2: "Cualquier pickable debe tener animación por lo menos una que suba y baje o rote al sprite".
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class PickableItem : MonoBehaviour
{
    public static event Action<PowerUpType> OnItemCollected;

    [Header("Tipo de Power-Up")]
    [SerializeField] private PowerUpType powerUpType = PowerUpType.ExtraLife;

    [Tooltip("Duración de invencibilidad si el tipo es Invincibility (5s según TP05.2).")]
    [SerializeField] private float invincibilityDuration = 5f;

    [Header("Animación de Subida y Bajada (Bobbing)")]
    [SerializeField] private bool enableBobbing = true;
    [SerializeField] private float bobbingAmplitude = 0.35f;
    [SerializeField] private float bobbingFrequency = 3.5f;

    [Header("Animación de Rotación")]
    [SerializeField] private bool enableRotation = true;
    [SerializeField] private float rotationSpeed = 90f;

    [Header("Límite de Despawn")]
    [SerializeField] private float despawnX = -14f;

    private float startY;
    private float timeOffset;

    void Start()
    {
        startY = transform.position.y;
        timeOffset = UnityEngine.Random.Range(0f, 5f);
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    void Update()
    {
        // 1. Desplazamiento horizontal con el mundo
        float speed = WorldSpeedController.Instance != null ? WorldSpeedController.Instance.CurrentSpeed : 7f;
        transform.Translate(Vector3.left * (speed * Time.deltaTime), Space.World);

        // 2. Animación de subida y bajada (Bobbing)
        if (enableBobbing)
        {
            float newY = startY + Mathf.Sin((Time.time + timeOffset) * bobbingFrequency) * bobbingAmplitude;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }

        // 3. Animación de rotación continua
        if (enableRotation)
        {
            transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime, Space.Self);
        }

        // 4. Limpieza fuera de pantalla
        if (transform.position.x <= despawnX)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController2D player = collision.GetComponent<PlayerController2D>();
        if (player != null)
        {
            switch (powerUpType)
            {
                case PowerUpType.Invincibility:
                    player.ActivateInvincibility(invincibilityDuration);
                    break;
                case PowerUpType.ExtraLife:
                    player.AddLife(1);
                    break;
            }

            OnItemCollected?.Invoke(powerUpType);
            Destroy(gameObject);
        }
    }
}
