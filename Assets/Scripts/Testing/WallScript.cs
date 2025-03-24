using UnityEngine;

public class WallScript : MonoBehaviour
{
    [SerializeField] private GameObject BossWalls;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (BossWalls != null)
            BossWalls.SetActive(false);
        else Debug.Log("can't find boss walls");
    }

    // Update is called once per frame
    void Update()
    {

    }

    // OnTriggerEnter is called when another collider enters this object's trigger collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (BossWalls != null)
            {
                BossWalls.SetActive(true);
            }
            else Debug.Log("can't find boss walls");
        }
    }
}
