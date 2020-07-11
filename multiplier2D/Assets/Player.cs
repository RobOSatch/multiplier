using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EZCameraShake;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.InputSystem;

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

    void RumbleController()
    {
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0.25f, 0.75f);
            StartCoroutine(StopRumble());
        }
    }

    private IEnumerator StopRumble()
    {
        yield return new WaitForSeconds(0.05f);
        Gamepad.current.PauseHaptics();
    }

    private void ShakeCamera()
    {
        CameraShaker.Instance.ShakeOnce(4f, 2f, 0.1f, 1.0f);
    }

    public void TakeDamage(int damage)
    {
        ShakeCamera();
        RumbleController();

        hp = Math.Max(0, hp - damage);
        hpText.text = hp.ToString();

        if (hp <= 0)
        {
            Gamepad.current.PauseHaptics();
            Instantiate(deathEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
