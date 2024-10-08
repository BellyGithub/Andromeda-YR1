using UnityEngine;
using UnityEngine.UI;


public class HealthManager : MonoBehaviour
{
    public Image healthBar;
    public float healthAmount = 100f;
    public static float health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = healthAmount;
    }

    // Update is called once per frame
    void Update()
    {
        if (healthAmount <= 0)
        {
            Application.LoadLevel(Application.loadedLevel);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            TakeDamage(20);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(5);
        }
    }

    public void TakeDamage(float damage)
    {
        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / 100f;
    }

    public void Heal(float healingAmount)
    {
        healthAmount += healingAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100);

        healthBar.fillAmount = healthAmount / 100f;
    }

}
