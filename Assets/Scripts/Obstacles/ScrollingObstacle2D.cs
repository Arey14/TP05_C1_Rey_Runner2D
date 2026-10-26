using UnityEngine;

/// <summary>
/// Componente de obstáculo con desplazamiento y animación sutil de balanceo/rotación.
/// Cumple con TP05.1 y TP05.2 ("Integrar animaciones: Obstáculos").
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class ScrollingObstacle2D : MonoBehaviour
{
    [Header("Animación Procedural")]
    [Tooltip("Habilita oscilación/rotación sutil del sprite.")]
    [SerializeField] private bool animateSway = true;

    [Tooltip("Amplitud máxima de balanceo en grados.")]
    [SerializeField] private float swayAngle = 8f;

    [Tooltip("Frecuencia de oscilación.")]
    [SerializeField] private float swayFrequency = 3f;

    [Header("Límite de Despawn")]
    [SerializeField] private float despawnX = -14f;

    private float baseRotationZ;
    private float timeOffset;

    void Start()
    {
        baseRotationZ = transform.rotation.eulerAngles.z;
        timeOffset = Random.Range(0f, 10f);
    }

    void Update()
    {
        // 1. Desplazamiento hacia la izquierda
        float speed = WorldSpeedController.Instance != null ? WorldSpeedController.Instance.CurrentSpeed : 7f;
        transform.Translate(Vector3.left * (speed * Time.deltaTime), Space.World);

        // 2. Animación procedural de balanceo (TP05.2)
        if (animateSway)
        {
            float zRot = baseRotationZ + Mathf.Sin((Time.time + timeOffset) * swayFrequency) * swayAngle;
            transform.rotation = Quaternion.Euler(0f, 0f, zRot);
        }

        // 3. Despawn al salir de pantalla
        if (transform.position.x <= despawnX)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.GetComponent<PlayerController2D>() != null)
        {
            PlayerController2D player = collision.GetComponent<PlayerController2D>();
            if (player != null && !player.IsInvincible)
            {
                player.TakeDamage();
            }
        }
    }
}
