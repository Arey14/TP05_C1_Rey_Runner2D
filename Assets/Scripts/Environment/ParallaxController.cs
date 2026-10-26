using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sistema de Parallax multicapa (mínimo 3 fondos) que permite intercambio de biomas.
/// Cumple con la consigna de TP05.2 mediante el uso de BiomeDataSO y soporte para SwapBiome().
/// </summary>
public class ParallaxController : MonoBehaviour
{
    [System.Serializable]
    public class RuntimeParallaxLayer
    {
        public string name;
        public float speedMultiplier;
        public Transform segmentA;
        public Transform segmentB;
        public SpriteRenderer srA;
        public SpriteRenderer srB;
        public float width;
    }

    [Header("Configuración de Bioma")]
    [SerializeField] private BiomeDataSO currentBiome;

    [Tooltip("Ancho de repetición de cada capa de fondo en unidades de mundo.")]
    [SerializeField] private float layerWidth = 24f;

    [Tooltip("Límite en X hacia la izquierda antes de reciclar el segmento.")]
    [SerializeField] private float recycleThresholdX = -24f;

    private readonly List<RuntimeParallaxLayer> activeLayers = new List<RuntimeParallaxLayer>();

    void Start()
    {
        InitializeBiomeLayers();
    }

    void Update()
    {
        float baseSpeed = WorldSpeedController.Instance != null ? WorldSpeedController.Instance.CurrentSpeed : 7f;

        for (int i = 0; i < activeLayers.Count; i++)
        {
            RuntimeParallaxLayer layer = activeLayers[i];
            float layerSpeed = baseSpeed * layer.speedMultiplier;
            float step = layerSpeed * Time.deltaTime;

            if (layer.segmentA != null)
            {
                layer.segmentA.Translate(Vector3.left * step);
                if (layer.segmentA.position.x <= recycleThresholdX)
                {
                    float nextX = layer.segmentB.position.x + layer.width - 0.05f;
                    layer.segmentA.position = new Vector3(nextX, layer.segmentA.position.y, layer.segmentA.position.z);
                }
            }

            if (layer.segmentB != null)
            {
                layer.segmentB.Translate(Vector3.left * step);
                if (layer.segmentB.position.x <= recycleThresholdX)
                {
                    float nextX = layer.segmentA.position.x + layer.width - 0.05f;
                    layer.segmentB.position = new Vector3(nextX, layer.segmentB.position.y, layer.segmentB.position.z);
                }
            }
        }
    }

    /// <summary>
    /// Inicializa las capas en base al BiomeDataSO configurado.
    /// Crea al menos 3 capas de parallax con sus segmentos duplicados para bucle continuo.
    /// </summary>
    public void InitializeBiomeLayers()
    {
        // Limpiar capas previas
        foreach (var l in activeLayers)
        {
            if (l.segmentA != null) Destroy(l.segmentA.gameObject);
            if (l.segmentB != null) Destroy(l.segmentB.gameObject);
        }
        activeLayers.Clear();

        if (currentBiome == null) return;

        var configs = currentBiome.ParallaxLayers;
        if (configs == null || configs.Length == 0) return;

        for (int i = 0; i < configs.Length; i++)
        {
            var cfg = configs[i];
            GameObject layerRoot = new GameObject($"Layer_{i}_{cfg.layerName}");
            layerRoot.transform.SetParent(transform);

            // Segmento A
            GameObject segA = new GameObject("SegA");
            segA.transform.SetParent(layerRoot.transform);
            segA.transform.position = new Vector3(0f, cfg.verticalOffset, 10f - i);
            SpriteRenderer srA = segA.AddComponent<SpriteRenderer>();
            srA.sprite = cfg.layerSprite;
            srA.color = cfg.tint;
            srA.sortingOrder = -20 + i;

            // Segmento B (continuación hacia la derecha)
            GameObject segB = new GameObject("SegB");
            segB.transform.SetParent(layerRoot.transform);
            segB.transform.position = new Vector3(layerWidth, cfg.verticalOffset, 10f - i);
            SpriteRenderer srB = segB.AddComponent<SpriteRenderer>();
            srB.sprite = cfg.layerSprite;
            srB.color = cfg.tint;
            srB.sortingOrder = -20 + i;

            RuntimeParallaxLayer runtimeLayer = new RuntimeParallaxLayer
            {
                name = cfg.layerName,
                speedMultiplier = cfg.scrollSpeedMultiplier,
                segmentA = segA.transform,
                segmentB = segB.transform,
                srA = srA,
                srB = srB,
                width = layerWidth
            };

            activeLayers.Add(runtimeLayer);
        }
    }

    /// <summary>
    /// Intercambia el bioma activo en tiempo de ejecución o desde inspector.
    /// Requisito directo de TP05.2: "El Parallax debe ser uno que permita el intercambio de biomas".
    /// </summary>
    public void SwapBiome(BiomeDataSO newBiome)
    {
        if (newBiome == null) return;
        currentBiome = newBiome;
        InitializeBiomeLayers();
        Debug.Log($"[ParallaxController] Swapped to Biome: {newBiome.BiomeName}");
    }

    public BiomeDataSO CurrentBiome => currentBiome;
}
