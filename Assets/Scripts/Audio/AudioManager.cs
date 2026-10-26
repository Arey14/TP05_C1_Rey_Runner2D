using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Gestor centralizado de audio y AudioMixer persistente entre escenas (DontDestroyOnLoad).
/// Cumple con TP05.1:
/// - Volumen configurable directo a AudioMixer (Master, Música, Efectos).
/// - Integración de sonido en 6 interacciones clave (Menú, Gameplay, Salto, Aterrizaje, Coleccionable, Derrota, UI).
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("AudioMixer (TP05.1)")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string masterParam = "MasterVolume";
    [SerializeField] private string musicParam = "MusicVolume";
    [SerializeField] private string sfxParam = "SFXVolume";

    [Header("Canales de Reproducción")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Clips de Música (Opcional - asignables en Inspector)")]
    [SerializeField] private AudioClip menuMusicClip;
    [SerializeField] private AudioClip gameplayMusicClip;

    [Header("Clips de Efectos (Opcional - fallback procedural automático)")]
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip landClip;
    [SerializeField] private AudioClip collectClip;
    [SerializeField] private AudioClip gameOverClip;
    [SerializeField] private AudioClip uiClickClip;

    private const string PrefMaster = "Vol_Master";
    private const string PrefMusic = "Vol_Music";
    private const string PrefSFX = "Vol_SFX";

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        EnsureAudioSources();
        GenerateProceduralFallbacks();
        LoadSavedVolumes();
    }

    void OnEnable()
    {
        PlayerController2D.OnJumpTriggered += HandlePlayerJump;
        PlayerController2D.OnLanded += HandlePlayerLand;
        PlayerController2D.OnPlayerDied += HandlePlayerDeath;
        PickableItem.OnItemCollected += HandleItemCollected;
    }

    void OnDisable()
    {
        PlayerController2D.OnJumpTriggered -= HandlePlayerJump;
        PlayerController2D.OnLanded -= HandlePlayerLand;
        PlayerController2D.OnPlayerDied -= HandlePlayerDeath;
        PickableItem.OnItemCollected -= HandleItemCollected;
    }

    private void EnsureAudioSources()
    {
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;
        }

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
        }
    }

    private void GenerateProceduralFallbacks()
    {
        if (jumpClip == null) jumpClip = ProceduralAudioHelper.CreateJumpSound();
        if (landClip == null) landClip = ProceduralAudioHelper.CreateLandSound();
        if (collectClip == null) collectClip = ProceduralAudioHelper.CreateCollectSound();
        if (gameOverClip == null) gameOverClip = ProceduralAudioHelper.CreateGameOverSound();
        if (uiClickClip == null) uiClickClip = ProceduralAudioHelper.CreateButtonClickSound();
    }

    private void LoadSavedVolumes()
    {
        SetMasterVolume(PlayerPrefs.GetFloat(PrefMaster, 0.8f));
        SetMusicVolume(PlayerPrefs.GetFloat(PrefMusic, 0.75f));
        SetSFXVolume(PlayerPrefs.GetFloat(PrefSFX, 0.85f));
    }

    // ==========================================
    // Control de AudioMixer y Volúmenes en dB
    // ==========================================
    public void SetMasterVolume(float linear)
    {
        linear = Mathf.Clamp(linear, 0.0001f, 1f);
        float dB = Mathf.Log10(linear) * 20f;
        if (audioMixer != null) audioMixer.SetFloat(masterParam, dB);
        PlayerPrefs.SetFloat(PrefMaster, linear);
    }

    public void SetMusicVolume(float linear)
    {
        linear = Mathf.Clamp(linear, 0.0001f, 1f);
        float dB = Mathf.Log10(linear) * 20f;
        if (audioMixer != null) audioMixer.SetFloat(musicParam, dB);
        if (musicSource != null) musicSource.volume = linear;
        PlayerPrefs.SetFloat(PrefMusic, linear);
    }

    public void SetSFXVolume(float linear)
    {
        linear = Mathf.Clamp(linear, 0.0001f, 1f);
        float dB = Mathf.Log10(linear) * 20f;
        if (audioMixer != null) audioMixer.SetFloat(sfxParam, dB);
        if (sfxSource != null) sfxSource.volume = linear;
        PlayerPrefs.SetFloat(PrefSFX, linear);
    }

    public float GetSavedMasterVolume() => PlayerPrefs.GetFloat(PrefMaster, 0.8f);
    public float GetSavedMusicVolume() => PlayerPrefs.GetFloat(PrefMusic, 0.75f);
    public float GetSavedSFXVolume() => PlayerPrefs.GetFloat(PrefSFX, 0.85f);

    // ==========================================
    // Reproducción de Música
    // ==========================================
    public void PlayMenuMusic()
    {
        if (menuMusicClip != null && musicSource.clip != menuMusicClip)
        {
            musicSource.clip = menuMusicClip;
            musicSource.Play();
        }
    }

    public void PlayGameplayMusic()
    {
        if (gameplayMusicClip != null && musicSource.clip != gameplayMusicClip)
        {
            musicSource.clip = gameplayMusicClip;
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        if (musicSource != null) musicSource.Stop();
    }

    // ==========================================
    // Handlers de Efectos de Sonido
    // ==========================================
    private void HandlePlayerJump()
    {
        PlaySFX(jumpClip);
    }

    private void HandlePlayerLand()
    {
        PlaySFX(landClip);
    }

    private void HandlePlayerDeath()
    {
        PlaySFX(gameOverClip);
    }

    private void HandleItemCollected(PowerUpType type)
    {
        PlaySFX(collectClip);
    }

    public void PlayButtonClick()
    {
        PlaySFX(uiClickClip);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}
