using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private float startTime;
    public int score = 0;

    public void LevelCompleted()
    {
        float initialScore = 100f;
        float timeTaken = 10f;
        float maxTime = 45f;   // Maximum time for full bonus eligibility

        timeTaken = Mathf.Clamp(timeTaken, 0f, maxTime);
        float timeFactor = 1f - (timeTaken / maxTime); // 1 when fast, 0 when slow
        float bonus = initialScore * timeFactor; // scale the bonus by speed
        float finalScore = initialScore + bonus;
        finalScore = score;
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
