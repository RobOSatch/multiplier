﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public void PlayGame(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
            GameManager.Instance.LoadGame();
            AudioManager.Instance.Stop("MenuTheme");

            if (!GameManager.Instance.gameInProgress)
            {
                AudioManager.Instance.Play("MainTheme");
            }

            GameManager.Instance.gameInProgress = true;
        }
    }
}
