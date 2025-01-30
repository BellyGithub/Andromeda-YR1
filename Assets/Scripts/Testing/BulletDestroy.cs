using UnityEngine;

public class BulletDestroy : MonoBehaviour
{
    int damage = 30;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HealthManagerScript healthManager = other.GetComponent<HealthManagerScript>();
            if (healthManager != null)
            {
                healthManager.TakeDamage(damage);
            }
            else
            {
                Debug.LogWarning("Can't find healthManager");
            }
        }
        Destroy(gameObject);
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
