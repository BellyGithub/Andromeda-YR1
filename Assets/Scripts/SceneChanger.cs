using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string Scene; // Name of the target scene

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Ensure the Player has the correct tag
        {
            SceneManager.LoadScene(Scene); // Load the specified scene
        }
    }
}
