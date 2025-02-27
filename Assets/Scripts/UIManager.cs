using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject levelCompletePanel; // UI Panel to show when level is completed
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;

    private string nextSceneName;
    private bool isLevelCompleted = false;

    void Start()
    {
        levelCompletePanel.SetActive(false); // Hide menu at start
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

    // Button function to return to the main menu
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // Resume game time
        SceneManager.LoadScene("MainMenuScene"); // Change this to your actual main menu scene name
    }
}
