using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    public TextMesh hpText;
    public int health = 100;
    public int reward = 10;
    public float speed = 2.0f;
    private GameObject target;
    private bool shouldMultiply;

    public float multiplyCooldown = 0.5f;
    private float timeSinceLastMultiply;

    public Light groundLight;
    public Sprite markedSprite;

    public int multiplyAmount = 1;
    public bool exponential = false;
    public bool autoMultiply = false;

    public void TakeDamage(int damage)
    {
        AudioManager.Instance.Play("EnemyHit");

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
        bool was = shouldMultiply;
        shouldMultiply = marked;
        if (shouldMultiply != was) {
            AudioManager.Instance.Play("Multiply");
        }

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
        AudioManager.Instance.Play("Multiply");

        timeSinceLastMultiply = Time.time;
        GameObject clone = Instantiate(gameObject, transform.position, transform.rotation);

        Enemy enemy = clone.GetComponent<Enemy>();
        enemy.MarkForMultiplication(exponential);
        //enemy.SetMultiplyCooldown(multiplyCooldown * 2);
    }

    void Die()
    {
        GameManager.Instance.score += reward;
        //money.money += reward;
        GameManager.Instance.money += reward;
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

        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private void Awake()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }

        timeSinceLastMultiply = 0f;
        shouldMultiply = false;

        if (autoMultiply)
        {
            MarkForMultiplication(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        LookAtPlayer();
        MoveToPlayer();

        CounterRotateText();

        if (shouldMultiply && (Time.time - timeSinceLastMultiply >= multiplyCooldown))
        {
            for (int i = 0; i < multiplyAmount; ++i)
            {
                Multiply();
            }
        }
    }
}
