using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public UIManager uiManager; // Reference to the UIManager
    Score score;
    Timer timer;
    private float levelTime = 0f; // Tracks the time for the level
    private bool levelCompleted = false; // To ensure the level is only completed once

    // Start is called before the first frame update
    void Update()
    {
        if (!levelCompleted)
        {
            levelTime += Time.deltaTime; // Track time when the level is not completed
        }
    }

    void Awake()
    {
        score = FindAnyObjectByType<Score>();
        timer = FindAnyObjectByType<Timer>();
        Time.timeScale = 1f; // Reset time scale when the level starts
    }

    // Call this method when the level is completed
    public void CompleteLevel()
    {
        if (!levelCompleted)
        {
            levelCompleted = true;
            //string nextSceneName = GetNextSceneName(); // Get the next level name
            float finalScore = score.score;
            float finalTime = timer.elapsedTime;
            uiManager.ShowLevelComplete(finalScore, finalTime); // Pass score, time, and next level
            //StartCoroutine(LoadNextLevelAfterDelay(10f)); // Wait 10 seconds before loading the next level (you can adjust this)
        }
    }

    // Returns the next level name depending on the current level
    private string GetNextSceneName()
    {
        if (SceneManager.GetActiveScene().name == "Level-1")
        {
            return "Level-2"; // If it's Level1, go to Level2
        }
        else if (SceneManager.GetActiveScene().name == "Level-2")
        {
            return "Level-3"; // If it's Level2, go to Level3
        }
        else if(SceneManager.GetActiveScene().name == "Demo-Level")
        {
            return "BossLevel"; // If it's the Demo Level, go to the boss level
        }
            return "MainMenuScene"; // Default to MainMenu if no next level is defined
    }

    // Coroutine to load the next level after a delay
    IEnumerator LoadNextLevelAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); // Wait in real time (ignores Time.timeScale)
        Time.timeScale = 1f; // Ensure game time is running normally
        SceneManager.LoadScene(GetNextSceneName()); // Load the next scene dynamically
    }
}
