using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private int damage = 0;
    [SerializeField] HealthManagerScript healthManager;

    private void Start()
    {
        healthManager = FindAnyObjectByType<HealthManagerScript>();
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            healthManager.TakeDamage(damage);
        }
    }
}
