﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Money : MonoBehaviour
{
    public Text myText;

    void Update()
    {
        myText.text = GameManager.Instance.money.ToString();
    }
}
