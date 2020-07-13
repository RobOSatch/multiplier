using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{

    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject multiplierPrefab;
    public float shootCooldown = 0.2f;

    private float timeSinceLastShot;
    private bool shooting = false;
    private bool multiplying = false;
    public bool shotgun = false;

    // Start is called before the first frame update
    void Start()
    {
        timeSinceLastShot = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (shooting && Time.time - timeSinceLastShot >= shootCooldown)
        {
            timeSinceLastShot = Time.time;
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            if (shotgun)
            {
                Instantiate(bulletPrefab, firePoint.position, Quaternion.AngleAxis(12.5f, firePoint.transform.forward) * firePoint.rotation);
                Instantiate(bulletPrefab, firePoint.position, Quaternion.AngleAxis(-12.5f, firePoint.transform.forward) * firePoint.rotation);

                Instantiate(bulletPrefab, firePoint.position, Quaternion.AngleAxis(25.0f, firePoint.transform.forward) * firePoint.rotation);
                Instantiate(bulletPrefab, firePoint.position, Quaternion.AngleAxis(-25.0f, firePoint.transform.forward) * firePoint.rotation);
            }
        } else if (multiplying && Time.time - timeSinceLastShot >= shootCooldown)
        {
            timeSinceLastShot = Time.time;
            Instantiate(multiplierPrefab, firePoint.position, firePoint.rotation);
        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
            shooting = true;
        } else
        {
            shooting = false;
        }
    }

    public void FireMultiplier(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
            multiplying = true;
        }
        else
        {
            multiplying = false;
        }
    }
}
