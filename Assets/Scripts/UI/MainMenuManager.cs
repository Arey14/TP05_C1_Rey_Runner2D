using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Gestiona la navegación y acciones del Menú Principal en la escena MainMenuScene.
/// Cumple con la consigna de TP05.2 ("Debe estar el Menú Principal y el Gameplay separados en 2 escenas").
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    [Header("Nombre de la Escena de Juego")]
    [SerializeField] private string gameplaySceneName = "GameplayScene";

    [Header("Paneles de la UI")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditsPanel;

    [Header("Botones del Menú Principal")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button exitButton;

    [Header("Botones de Regreso")]
    [SerializeField] private Button settingsBackButton;
    [SerializeField] private Button creditsBackButton;

    void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMenuMusic();
        }

        if (playButton != null) playButton.onClick.AddListener(OnPlayClicked);
        if (settingsButton != null) settingsButton.onClick.AddListener(OnSettingsClicked);
        if (creditsButton != null) creditsButton.onClick.AddListener(OnCreditsClicked);
        if (exitButton != null) exitButton.onClick.AddListener(OnExitClicked);

        if (settingsBackButton != null) settingsBackButton.onClick.AddListener(OnBackToMainMenu);
        if (creditsBackButton != null) creditsBackButton.onClick.AddListener(OnBackToMainMenu);

        ShowMainMenu();
    }

    public void OnPlayClicked()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayButtonClick();
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void OnSettingsClicked()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayButtonClick();
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void OnCreditsClicked()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayButtonClick();
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(true);
    }

    public void OnBackToMainMenu()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayButtonClick();
        ShowMainMenu();
    }

    private void ShowMainMenu()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(false);
    }

    public void OnExitClicked()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayButtonClick();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
