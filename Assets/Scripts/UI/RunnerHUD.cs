using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Gestiona la visualización del HUD durante la partida:
/// - Vidas actuales.
/// - Distancia / Puntuación en tiempo real.
/// - Timer y barra de cuenta regresiva de Invencibilidad (TP05.2).
/// - Pantalla de Game Over con reinicio y regreso al menú principal.
/// </summary>
public class RunnerHUD : MonoBehaviour
{
    [Header("Indicador de Vidas")]
    [SerializeField] private TextMeshProUGUI livesText;

    [Header("Indicador de Distancia / Score")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Panel de Invencibilidad (TP05.2)")]
    [Tooltip("Panel o contenedor que se activa únicamente mientras la invencibilidad está en curso.")]
    [SerializeField] private GameObject invincibilityPanel;
    [SerializeField] private TextMeshProUGUI invincibilityTimerText;
    [SerializeField] private Slider invincibilitySlider;

    [Header("Pantalla de Game Over")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    private float distanceScore = 0f;
    private bool isGameOver = false;

    void Start()
    {
        if (invincibilityPanel != null) invincibilityPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(GoToMainMenu);
        }
    }

    void OnEnable()
    {
        PlayerController2D.OnLivesChanged += UpdateLivesUI;
        PlayerController2D.OnInvincibilityTimerUpdated += UpdateInvincibilityUI;
        PlayerController2D.OnPlayerDied += ShowGameOver;
    }

    void OnDisable()
    {
        PlayerController2D.OnLivesChanged -= UpdateLivesUI;
        PlayerController2D.OnInvincibilityTimerUpdated -= UpdateInvincibilityUI;
        PlayerController2D.OnPlayerDied -= ShowGameOver;
    }

    void Update()
    {
        if (!isGameOver)
        {
            float speed = WorldSpeedController.Instance != null ? WorldSpeedController.Instance.CurrentSpeed : 7f;
            distanceScore += speed * Time.deltaTime;

            if (scoreText != null)
            {
                scoreText.text = $"Distancia: {Mathf.FloorToInt(distanceScore)}m";
            }
        }
    }

    private void UpdateLivesUI(int currentLives)
    {
        if (livesText != null)
        {
            livesText.text = $"Vidas: {currentLives}";
        }
    }

    /// <summary>
    /// Actualiza el timer y slider de invencibilidad en la UI según TP05.2.
    /// </summary>
    private void UpdateInvincibilityUI(float remainingTime)
    {
        if (invincibilityPanel != null)
        {
            bool active = remainingTime > 0.05f;
            if (invincibilityPanel.activeSelf != active)
            {
                invincibilityPanel.SetActive(active);
            }
        }

        if (invincibilityTimerText != null)
        {
            invincibilityTimerText.text = $"Invencible: {remainingTime:0.0}s";
        }

        if (invincibilitySlider != null)
        {
            invincibilitySlider.value = Mathf.Clamp01(remainingTime / 5f);
        }
    }

    private void ShowGameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        if (finalScoreText != null)
        {
            finalScoreText.text = $"Puntaje Final: {Mathf.FloorToInt(distanceScore)}m";
        }
    }

    public void RestartGame()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayButtonClick();
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayButtonClick();
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }
}
