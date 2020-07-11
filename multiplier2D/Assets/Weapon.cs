﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{

    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject multiplierPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }

    public void FireMultiplier(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
            Instantiate(multiplierPrefab, firePoint.position, firePoint.rotation);
        }
    }
}