using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public Text hpText;
    public int hp;
    public GameObject deathEffect;

    // Start is called before the first frame update
    void Start()
    {
        hpText.text = hp.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        hp = Math.Max(0, hp - damage);
        hpText.text = hp.ToString();

        if (hp <= 0)
        {
            Instantiate(deathEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
