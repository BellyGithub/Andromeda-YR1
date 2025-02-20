using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private float startTime;
    private int score = 0;

    public void LevelCompleted(string sceneName)
    {
        int initialScore = 100000;
        float elapsedTime = Time.time - startTime;
        int penalty = Mathf.FloorToInt(elapsedTime) * 1000;
        SceneManager.LoadScene(sceneName);
        score += Mathf.Max(0, initialScore - penalty);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startTime = Time.time;
        UpdateScoreUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}
