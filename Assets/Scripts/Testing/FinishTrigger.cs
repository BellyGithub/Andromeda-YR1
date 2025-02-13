using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Score scoreManager = FindObjectOfType<Score>();
            if (scoreManager != null)
            {
                scoreManager.LevelCompleted();
            }
            else
            {
                Debug.LogWarning("ScoreManager not found");
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
