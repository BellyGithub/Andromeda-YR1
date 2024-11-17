using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class EnemyWander : MonoBehaviour
{
    public float speed = 2.0f;
    public float leftLimit = -3.0f;
    public float rightLimit = 3.0f;
    private bool movingRight = false;
    private float dt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        leftLimit += transform.position.x;
        leftLimit += transform.localScale.x / 2;
        rightLimit += transform.position.x;
        rightLimit -= transform.localScale.x / 2;
    }

    // Update is called once per frame
    void Update()
    {
        dt = Time.deltaTime;
        if (movingRight)
        {
            transform.Translate(Vector2.right * speed * dt);
            if (transform.position.x >= rightLimit)
            {
                transform.position = new Vector3(rightLimit, transform.position.y, transform.position.z);
                movingRight = false;
            }
        }
        else
        {
            transform.Translate(Vector2.left * speed * dt);
            if (transform.position.x <= leftLimit)
            {
                transform.position = new Vector3(leftLimit, transform.position.y, transform.position.z);
                movingRight = true;
            }
        }
    }
}
