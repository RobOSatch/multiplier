
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    
    public void PlayGame(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
            GameManager.Instance.LoadGame();
        }
    }
}
