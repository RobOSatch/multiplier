﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 40;
    public Rigidbody2D rb;
    public GameObject hitEffect;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = Quaternion.AngleAxis(-90f, new Vector3(0.0f, 0.0f, 1f)) * transform.right * speed;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null)
        {
            player.TakeDamage(damage);
        }

        Instantiate(hitEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
