using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Configuración de Victoria")]
    public int enemiesToWin = 5;
    private int currentKills = 0;

    [Header("Interfaz de Usuario (UI)")]
    public TextMeshProUGUI counterText;
    public GameObject winPanel;
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public TextMeshProUGUI nextLevelButtonText;

    private bool isPaused = false;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        Time.timeScale = 1f;

        if (winPanel != null) winPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);

        UpdateKillUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool winActive = winPanel != null && winPanel.activeSelf;
            bool gameOverActive = gameOverPanel != null && gameOverPanel.activeSelf;

            if (!winActive && !gameOverActive)
            {
                if (isPaused) ResumeGame();
                else PauseGame();
            }
        }
    }

    public void RegisterKill()
    {
        currentKills++;
        UpdateKillUI();

        if (currentKills >= enemiesToWin)
        {
            WinGame();
        }
    }

    void UpdateKillUI()
    {
        if (counterText != null)
        {
            counterText.text = "Fantasmas: " + currentKills + " / " + enemiesToWin;
        }
    }

    void WinGame()
    {
        if (winPanel != null) winPanel.SetActive(true);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextLevelButtonText != null)
        {
            if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
            {
                nextLevelButtonText.text = "Menú Principal";
            }
            else
            {
                nextLevelButtonText.text = "Siguiente Nivel";
            }
        }

        Time.timeScale = 0f;
    }

    public void GameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    // --- PAUSA CON MÚSICA ---

    public void PauseGame()
    {
        isPaused = true;
        if (pausePanel != null) pausePanel.SetActive(true);

        if (AudioManager.instance != null)
        {
            AudioManager.instance.PauseMusic();
        }

        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        if (pausePanel != null) pausePanel.SetActive(false);

        if (AudioManager.instance != null)
        {
            AudioManager.instance.ResumeMusic();
        }

        Time.timeScale = 1f;
    }

    // --- NAVEGACIÓN Y REINICIO ---

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    public void LoadPreviousLevel()
    {
        Time.timeScale = 1f;
        int previousSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;

        if (previousSceneIndex > 0)
        {
            SceneManager.LoadScene(previousSceneIndex);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        // Reiniciar la música desde el comienzo
        if (AudioManager.instance != null)
        {
            AudioManager.instance.RestartMusic();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}