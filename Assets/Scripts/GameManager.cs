using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public enum PowerUp
{
    RAPIDFIRE = 0,
    HEALTHBOOST = 1,
    SHIELD = 2,
    SHOTGUN = 3
}

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    private int currentScene = 0;
    private int enemies = 0;

    public int money = 0;
    public int health = 500;
    public int shield = 0;

    [HideInInspector]
    public int currShield = 0;
    
    public int score = 0;

    public int[] activePowerUps;

    public int startMoney = 1000;

    public int numberOfLevels = 4;

    public bool gameInProgress = false;

    public void ResetGame()
    {
        money = startMoney;
        health = 500;
        shield = 0;
        score = 0;
        currentScene = 1;

        for (int i = 0; i < activePowerUps.Length; ++i)
        {
            activePowerUps[i] = 0;
        }
    }

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        AudioManager.Instance.Play("MenuTheme");

        activePowerUps = new int[4];
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Level1");
        currentScene = 1;

        ResetGame();
    }

    public void LoadNextLevel()
    {
        currentScene++;

        if (currentScene == numberOfLevels + 1)
        {
            LoadEndScreen();
            return;
        }

        SceneManager.LoadScene("Level" + (currentScene).ToString());
    }

    public void LoadShop()
    {
        Gamepad.current.PauseHaptics();
        
        if (currentScene == numberOfLevels)
        {
            LoadEndScreen();
        } else
        {
            SceneManager.LoadScene("Shop");
        }
    }

    public void LoadEndScreen()
    {
        Gamepad.current.PauseHaptics();
        SceneManager.LoadScene("EndScreen");
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "Menu" && SceneManager.GetActiveScene().name != "Shop" && SceneManager.GetActiveScene().name != "EndScreen")
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length == 0)
            {
                Gamepad.current.PauseHaptics();
                LoadShop();
            }
        }
    }
}
