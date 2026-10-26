using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Panel de configuración de volumen vinculado directamente al AudioMixer vía AudioManager.
/// Cumple con la consigna de TP05.1 ("El volumen del sonido debe ser configurable desde Settings directo a un AudioMixer").
/// </summary>
public class AudioSettingsUI : MonoBehaviour
{
    [Header("Sliders de Volumen")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Botón de Regreso (Opcional)")]
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject previousPanel;

    void OnEnable()
    {
        InitializeSliders();
    }

    void Start()
    {
        InitializeSliders();

        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackClicked);
        }
    }

    private void InitializeSliders()
    {
        if (AudioManager.Instance == null) return;

        if (masterSlider != null)
        {
            masterSlider.minValue = 0.0001f;
            masterSlider.maxValue = 1f;
            masterSlider.value = AudioManager.Instance.GetSavedMasterVolume();
            masterSlider.onValueChanged.RemoveListener(OnMasterChanged);
            masterSlider.onValueChanged.AddListener(OnMasterChanged);
        }

        if (musicSlider != null)
        {
            musicSlider.minValue = 0.0001f;
            musicSlider.maxValue = 1f;
            musicSlider.value = AudioManager.Instance.GetSavedMusicVolume();
            musicSlider.onValueChanged.RemoveListener(OnMusicChanged);
            musicSlider.onValueChanged.AddListener(OnMusicChanged);
        }

        if (sfxSlider != null)
        {
            sfxSlider.minValue = 0.0001f;
            sfxSlider.maxValue = 1f;
            sfxSlider.value = AudioManager.Instance.GetSavedSFXVolume();
            sfxSlider.onValueChanged.RemoveListener(OnSFXChanged);
            sfxSlider.onValueChanged.AddListener(OnSFXChanged);
        }
    }

    private void OnMasterChanged(float val)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMasterVolume(val);
        }
    }

    private void OnMusicChanged(float val)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicVolume(val);
        }
    }

    private void OnSFXChanged(float val)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXVolume(val);
        }
    }

    private void OnBackClicked()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayButtonClick();
        gameObject.SetActive(false);
        if (previousPanel != null) previousPanel.SetActive(true);
    }
}
