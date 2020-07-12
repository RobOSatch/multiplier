using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPUpdater : MonoBehaviour
{

    public Text hpText;
    public int hp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hpText.text = hp.ToString();
    }

    void TakeDamage(int damage)
    {
        hp = Math.Max(0, hp - damage);
    }
}
