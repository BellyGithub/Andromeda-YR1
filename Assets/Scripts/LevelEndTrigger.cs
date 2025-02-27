using UnityEngine;

public class LevelEndTrigger : MonoBehaviour
{
    public GameManager gameManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Make sure the player has the "Player" tag
        {
            gameManager.CompleteLevel();
        }
    }
}
