using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    [SerializeField] private string sceneName;

    [System.Obsolete]
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Score scoreManager = FindObjectOfType<Score>();
            if (scoreManager != null)
            {
                Debug.Log("Loading next level... beep boop...");
                scoreManager.LevelCompleted();
            }
            else
            {
                Debug.LogWarning("ScoreManager not found");
            }
        }
    }
}
