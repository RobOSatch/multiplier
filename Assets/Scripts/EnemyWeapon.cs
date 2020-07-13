using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float shootCooldown = 1.0f;

    float timeSinceLastShot;

    // Start is called before the first frame update
    void Start()
    {
        float timeSinceLastShot = Time.time;
    }

    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (Time.time - timeSinceLastShot >= shootCooldown)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        timeSinceLastShot = Time.time;
        shootCooldown = Random.Range(0.4f, 0.8f);
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
