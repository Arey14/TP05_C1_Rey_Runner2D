using UnityEngine;

/// <summary>
/// Gestiona el desplazamiento infinito del suelo mediante 2 segmentos que se alternan.
/// </summary>
public class ScrollingGround2D : MonoBehaviour
{
    [Header("Segmentos de Suelo")]
    [SerializeField] private Transform groundSegmentA;
    [SerializeField] private Transform groundSegmentB;

    [Tooltip("Ancho de cada segmento de suelo en unidades de mundo.")]
    [SerializeField] private float segmentWidth = 24f;

    [Tooltip("Límite en X hacia la izquierda antes de reciclar el segmento.")]
    [SerializeField] private float resetThresholdX = -20f;

    void Start()
    {
        if (groundSegmentA != null && groundSegmentB != null)
        {
            groundSegmentA.position = new Vector3(0f, groundSegmentA.position.y, groundSegmentA.position.z);
            groundSegmentB.position = new Vector3(segmentWidth, groundSegmentB.position.y, groundSegmentB.position.z);
        }
    }

    void Update()
    {
        float speed = WorldSpeedController.Instance != null ? WorldSpeedController.Instance.CurrentSpeed : 7f;
        float displacement = speed * Time.deltaTime;

        if (groundSegmentA != null)
        {
            groundSegmentA.Translate(Vector3.left * displacement);
            if (groundSegmentA.position.x <= resetThresholdX)
            {
                float newX = groundSegmentB.position.x + segmentWidth - 0.05f;
                groundSegmentA.position = new Vector3(newX, groundSegmentA.position.y, groundSegmentA.position.z);
            }
        }

        if (groundSegmentB != null)
        {
            groundSegmentB.Translate(Vector3.left * displacement);
            if (groundSegmentB.position.x <= resetThresholdX)
            {
                float newX = groundSegmentA.position.x + segmentWidth - 0.05f;
                groundSegmentB.position = new Vector3(newX, groundSegmentB.position.y, groundSegmentB.position.z);
            }
        }
    }
}
