using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiplier : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    public GameObject hitEffect;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.Play("MultiplyShot");
        rb.velocity = Quaternion.AngleAxis(90f, new Vector3(0.0f, 0.0f, 1f)) * transform.right * speed;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.MarkForMultiplication(true);
        }

        Instantiate(hitEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
