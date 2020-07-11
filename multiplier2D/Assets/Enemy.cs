using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{

    public int health = 100;
    public GameObject target;
    private bool shouldMultiply;

    public float multiplyCooldown = 0.5f;
    private float timeSinceLastMultiply;

    public Light groundLight;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    public void SetMultiplyCooldown(float cooldown)
    {
        multiplyCooldown = cooldown;
    }

    public void MarkForMultiplication(bool marked)
    {
        shouldMultiply = marked;

        if (marked)
        {
            timeSinceLastMultiply = Time.time;
            groundLight.color = Color.cyan;
            groundLight.intensity = 1f;
        } else
        {
            groundLight.color = Color.red;
            groundLight.intensity = 2.5f;
        }
    }

    public void Multiply()
    {
        timeSinceLastMultiply = Time.time;
        GameObject clone = Instantiate(gameObject, transform.position, transform.rotation);

        Enemy enemy = clone.GetComponent<Enemy>();
        enemy.MarkForMultiplication(false);
        //enemy.SetMultiplyCooldown(multiplyCooldown * 2);
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void LookAtPlayer()
    {
        Vector3 dir = target.transform.position - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + 90f, Vector3.forward);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
        timeSinceLastMultiply = 0f;
        shouldMultiply = false;
    }

    // Update is called once per frame
    void Update()
    {
        LookAtPlayer();

        Debug.Log(shouldMultiply);
        if (shouldMultiply && (Time.time - timeSinceLastMultiply >= multiplyCooldown))
        {
            Multiply();
        }
    }
}
