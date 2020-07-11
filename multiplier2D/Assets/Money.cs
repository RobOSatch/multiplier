using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Money : MonoBehaviour
{
    public Text myText;
    public int money = 0;

    void Update()
    {
        myText.text = money.ToString();
    }
}
