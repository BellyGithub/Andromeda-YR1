using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public GameObject levelCompletePanel; // UI Panel to show when level is completed
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public UnityEngine.UI.Slider volumeSlider;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;

    public static bool GamePaused = false;
    public static float volume = 0.5f;
    private string nextSceneName;
    private bool isLevelCompleted = false;

    void Start()
    {
        // Initialize volume from PlayerPrefs if available
        if (PlayerPrefs.HasKey("GameVolume"))
        {
            volume = PlayerPrefs.GetFloat("GameVolume");
        }
        volumeSlider.value = volume;
        UpdateAllAudioSources();
        GamePaused = false; // In case of restart
        levelCompletePanel.SetActive(false); // Hide menu at start
        pauseMenu.SetActive(false); // Hide menu at start
        optionsMenu.SetActive(false); // Hide menu at start
    }

    // Method to show/hide pause menu
    public void PauseMenuFunction()
    {
        if (GamePaused)
        {
            Resume();
        }
        else if (!GamePaused)
        {
            Pause();
        }

        // Resume Game
        void Resume()
        {
            volume = volumeSlider.value;
            pauseMenu.SetActive(false);
            optionsMenu.SetActive(false);
            Time.timeScale = 1f;
            GamePaused = false;
        }
        
        // Pause Game
        void Pause()
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            GamePaused = true;
        }
    }

    // This method now accepts 3 arguments: score, time, and next scene name
    public void ShowLevelComplete(int score, float time, string sceneName)
    {
        if (!isLevelCompleted)
        {
            isLevelCompleted = true;
            levelCompletePanel.SetActive(true);
            scoreText.text = "Score: " + score;
            timeText.text = "Time: " + time.ToString("F2") + "s"; // Format to 2 decimal places
            Time.timeScale = 0f; // Pause the game
            nextSceneName = sceneName; // Store the next level name correctly
        }
    }

    // Button function to replay the current level
    public void ReplayLevel()
    {
        Time.timeScale = 1f; // Resume game time
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the same level
    }

    // Button function to load the next level
    public void LoadNextLevel()
    {
        if (!string.IsNullOrEmpty(nextSceneName)) // Ensure scene name is valid
        {
            Time.timeScale = 1f; // Resume game time
            SceneManager.LoadScene(nextSceneName); // Load the next level
        }
        else
        {
            Debug.LogWarning("Next scene name is not set! Make sure ShowLevelComplete() is called with a valid scene name.");
        }
    }

    public void UpdateVolume()
    {
        volume = volumeSlider.value;
        PlayerPrefs.SetFloat("GameVolume", volume); // Save volume preference
        PlayerPrefs.Save();
        UpdateAllAudioSources();
    }

    void UpdateAllAudioSources()
    {
        AudioSource[] audioSources = Object.FindObjectsByType<AudioSource>(FindObjectsSortMode.None); // Include all AudioSources
        foreach (AudioSource source in audioSources)
        {
            source.volume = volume;
        }
    }


    // Button function to return to the main menu
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // Resume game time
        SceneManager.LoadScene("MainMenuScene");
    }

    // Button function to show options menu
    public void showOptions()
    {
        optionsMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }
}
