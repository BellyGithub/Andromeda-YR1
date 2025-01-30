using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    private Transform player;
    private float detectionRadius = 10f;

    public GameObject bulletPrefab;
    public Transform firePoint;
    private float bulletSpeed = 5f;
    private float fireRate = 1f;
    private float nextFireTime = 0f;

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        if (bulletRb != null)
        {
            Vector2 direction = (player.position - firePoint.position).normalized;
            bulletRb.linearVelocity = direction * bulletSpeed;
            Destroy(bullet, 3f);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject playerobj = GameObject.FindGameObjectWithTag("Player");
        player = playerobj.transform;
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
