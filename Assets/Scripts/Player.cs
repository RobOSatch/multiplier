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
    public int hp;
    public GameObject deathEffect;
    public float damageCooldown = 0.5f;
    private float timeSinceDamage = 0.0f;

    private bool takenDamage = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Handle updates
        for (int i = 0; i < GameManager.Instance.activePowerUps.Length; ++i)
        {
            int powerup = GameManager.Instance.activePowerUps[i];
            if (powerup == 0) continue;
            Weapon weapon = GetComponent<Weapon>();

            switch (i)
            { 
                case (int) PowerUp.RAPIDFIRE:
                    weapon.shootCooldown = 0.07f;
                    break;
                case (int) PowerUp.HEALTHBOOST:
                    GameManager.Instance.health += 100;
                    GameManager.Instance.activePowerUps[i] -= 1;
                    break;
                case (int) PowerUp.SHIELD:
                    GameManager.Instance.shield += 50;
                    GameManager.Instance.activePowerUps[i] -= 1;
                    break;
                case (int)PowerUp.SHOTGUN:
                    weapon.shotgun = true;
                    break;
                default:
                    break;
            }
        }

        if (!takenDamage)
        {
            GameManager.Instance.currShield = GameManager.Instance.shield;
        }
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
        takenDamage = true;

        if (Time.time - timeSinceDamage < damageCooldown)
        {
            return;
        }

        timeSinceDamage = Time.time;

        AudioManager.Instance.Play("PlayerHit");
        ShakeCamera();
        RumbleController();

        int passedDamage = Math.Max(0, damage - GameManager.Instance.currShield);
        GameManager.Instance.currShield = Math.Max(0, GameManager.Instance.currShield - damage);
        
        hp = Math.Max(0, hp - passedDamage);
        GameManager.Instance.health = hp;
        
        if (hp <= 0)
        {
            Gamepad.current.PauseHaptics();
            Instantiate(deathEffect, transform.position, transform.rotation);
            GameManager.Instance.LoadEndScreen();
            Destroy(gameObject);
        }
    }
}
