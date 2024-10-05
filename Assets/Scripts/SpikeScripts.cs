using UnityEngine;

public class SpikeScripts : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
  
    private void OnCollisonEnter2D(Collision2D col)
    {
        GameObject whatHit = col.gameObject;
        if (whatHit.CompareTag("Player"))
        {
            HealthManager.health -= 30f; //player takes damage
        }
    }
}
