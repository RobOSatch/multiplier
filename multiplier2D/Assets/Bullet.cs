using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 30f;
    public int damage = 40;
    public Rigidbody2D rb;
    public GameObject hitEffect;
    public float lerpSpeed = 10f;

    private float currentSpeed = 1.0f;
    private Vector3 targetRotation;

    // Start is called before the first frame update
    void Start()
    {
        targetRotation = Quaternion.AngleAxis(90f, new Vector3(0.0f, 0.0f, 1f)) * transform.right;
        rb.velocity =  targetRotation * currentSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        currentSpeed = Mathf.Lerp(currentSpeed, speed, 0.99f * lerpSpeed * Time.deltaTime);
        rb.velocity = targetRotation * currentSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        Instantiate(hitEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
