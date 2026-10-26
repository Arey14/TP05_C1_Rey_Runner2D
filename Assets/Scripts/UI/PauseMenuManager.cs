using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Gestiona el Menú de Pausa durante la partida en GameplayScene.
/// </summary>
public class PauseMenuManager : MonoBehaviour
{
    [Header("Paneles")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("Botones")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button settingsBackButton;

    private bool isPaused = false;

    void Start()
    {
        if (resumeButton != null) resumeButton.onClick.AddListener(ResumeGame);
        if (settingsButton != null) settingsButton.onClick.AddListener(OpenSettings);
        if (mainMenuButton != null) mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        if (settingsBackButton != null) settingsBackButton.onClick.AddListener(BackToPauseMenu);

        ResumeGame();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                if (settingsPanel != null && settingsPanel.activeSelf)
                {
                    BackToPauseMenu();
                }
                else
                {
                    ResumeGame();
                }
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayButtonClick();
        isPaused = true;
        Time.timeScale = 0f;

        if (pausePanel != null) pausePanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    public void ResumeGame()
    {
        if (AudioManager.Instance != null && isPaused) AudioManager.Instance.PlayButtonClick();
        isPaused = false;
        Time.timeScale = 1f;

        if (pausePanel != null) pausePanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayButtonClick();
        if (pausePanel != null) pausePanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void BackToPauseMenu()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayButtonClick();
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayButtonClick();
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }
}
