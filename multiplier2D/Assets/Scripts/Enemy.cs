using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    public Money money;
    public TextMesh hpText;
    public int health = 100;
    public int reward = 10;
    public float speed = 2.0f;
    public GameObject target;
    private bool shouldMultiply;

    public float multiplyCooldown = 0.5f;
    private float timeSinceLastMultiply;

    public Light groundLight;
    public Sprite markedSprite;

    public void TakeDamage(int damage)
    {
        health -= damage;
        hpText.text = health.ToString();

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
            hpText.color = Color.yellow;
        } else
        {
            groundLight.color = Color.red;
            groundLight.intensity = 2.5f;
            hpText.color = Color.red;
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
        money.money += reward;
        Destroy(gameObject);
    }

    void LookAtPlayer()
    {
        Vector3 dir = target.transform.position - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + 90f, Vector3.forward);
    }

    void MoveToPlayer()
    {
        Vector3 dir = target.transform.position - transform.position;
        transform.position += dir.normalized * speed * Time.deltaTime;
    }

    void CounterRotateText()
    {
        hpText.transform.position = new Vector3(transform.position.x, transform.position.y + 0.75f, 0f);
        hpText.transform.rotation = Quaternion.Euler(0f, 0f, gameObject.transform.rotation.z * -1.0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        hpText.text = health.ToString();
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
        MoveToPlayer();

        CounterRotateText();

        if (shouldMultiply && (Time.time - timeSinceLastMultiply >= multiplyCooldown))
        {
            Multiply();
        }
    }
}
