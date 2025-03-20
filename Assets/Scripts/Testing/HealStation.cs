using UnityEngine;

public class HealStation : MonoBehaviour
{
    public int healAmount = 50;
    public int healTimes = 1;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Press E to heal");
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            HealthManagerScript healthManager = other.GetComponent<HealthManagerScript>();
            if (healthManager != null)
            {
                if (healTimes > 0)
                {
                    healthManager.heal(healAmount);
                    healTimes--;
                }
            }
            else
            {
                Debug.LogWarning("Can't find healthManager");
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
