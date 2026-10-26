using System;
using UnityEngine;

/// <summary>
/// Define los datos y capas visuales de un bioma para el sistema de Parallax.
/// Cumple con la consigna TP05.2 permitiendo el intercambio modular de biomas en runtime o Inspector.
/// </summary>
[CreateAssetMenu(fileName = "NewBiome", menuName = "Runner2D/BiomeData")]
public class BiomeDataSO : ScriptableObject
{
    [System.Serializable]
    public struct ParallaxLayerConfig
    {
        [Tooltip("Nombre identificador de la capa (ej. Cielo, Montañas Lejanas, Silueta).")]
        public string layerName;

        [Tooltip("Sprite o textura repetible para el fondo.")]
        public Sprite layerSprite;

        [Tooltip("Multiplicador de velocidad relativo al scroll del juego (0.05 = fondo muy lejano, 0.7 = fondo cercano).")]
        [Range(0f, 1f)]
        public float scrollSpeedMultiplier;

        [Tooltip("Tinte o modulación de color para la capa.")]
        public Color tint;

        [Tooltip("Desplazamiento vertical opcional en coordenadas de mundo.")]
        public float verticalOffset;
    }

    [Header("Identificación")]
    [SerializeField] private string biomeName = "Volcanic Cavern";

    [Header("Capas de Parallax (mínimo 3 según TP05.2)")]
    [SerializeField] private ParallaxLayerConfig[] parallaxLayers = new ParallaxLayerConfig[]
    {
        new ParallaxLayerConfig { layerName = "Sky / Deep Cavern", scrollSpeedMultiplier = 0.05f, tint = Color.white, verticalOffset = 0f },
        new ParallaxLayerConfig { layerName = "Distant Peaks", scrollSpeedMultiplier = 0.25f, tint = new Color(0.85f, 0.85f, 0.95f, 1f), verticalOffset = 0f },
        new ParallaxLayerConfig { layerName = "Foreground Rocks", scrollSpeedMultiplier = 0.6f, tint = new Color(0.7f, 0.7f, 0.8f, 1f), verticalOffset = 0f }
    };

    [Header("Decoración y Suelo")]
    [Tooltip("Sprite del suelo correspondiente a este bioma.")]
    [SerializeField] private Sprite groundSprite;

    [Tooltip("Color ambiental o niebla para este bioma.")]
    [SerializeField] private Color ambientColor = new Color(0.9f, 0.9f, 1f, 1f);

    public string BiomeName => biomeName;
    public ParallaxLayerConfig[] ParallaxLayers => parallaxLayers;
    public Sprite GroundSprite => groundSprite;
    public Color AmbientColor => ambientColor;
}
