using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    private Transform player;
    private float detectionRadius = 10f;

    public GameObject bulletPrefab;
    private float bulletSpeed = 5f;
    private float fireRate = 1f;
    private float nextFireTime = 0f;

    private BoxCollider2D boxCollider;
    Vector2 firePoint;

    void Shoot()
    {
        Bounds boxBounds = boxCollider.bounds;
        if (player.transform.position.x < boxBounds.min.x)
        {
            firePoint = new Vector2(boxBounds.min.x - 0.5f, boxBounds.center.y);
        }
        else if (player.transform.position.x > boxBounds.max.x)
        {
            firePoint = new Vector2(boxBounds.max.x + 0.5f, boxBounds.center.y);
        }
        else if (player.transform.position.y > boxBounds.max.y)
        {
            firePoint = new Vector2(boxBounds.center.x + 0.5f, boxBounds.max.y);
        }
            GameObject bullet = Instantiate(bulletPrefab, firePoint, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        if (bulletRb != null)
        {
            Vector2 direction = (player.position - (Vector3)firePoint).normalized;
            bulletRb.linearVelocity = direction * bulletSpeed;
            Destroy(bullet, 3f);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject playerobj = GameObject.FindGameObjectWithTag("Player");
        player = playerobj.transform;
        boxCollider = GetComponent<BoxCollider2D>();
        Vector2 firePoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance < detectionRadius && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }
}
