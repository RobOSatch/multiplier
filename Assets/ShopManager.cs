using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[System.Serializable]
public class PowerUpItem
{
    public Sprite icon;
    public string name;
    public int price;
    public int index;
}

public class ShopManager : MonoBehaviour
{

    public Image itemIcon;
    public Text itemName;
    public Text itemPrice;

    public PowerUpItem[] powerups;
    private int currentItem = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        DisplayCurrentItem();
    }

    public void PurchaseItem(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
            int money = GameManager.Instance.money;
            PowerUpItem purchase = powerups[currentItem];

            if (currentItem == (int) PowerUp.RAPIDFIRE ||
                currentItem == (int) PowerUp.SHOTGUN)
            {
                if (GameManager.Instance.activePowerUps[currentItem] > 0)
                {
                    AudioManager.Instance.Play("MenuFail");
                    return;
                }
            }

            if (money < purchase.price)
            {
                AudioManager.Instance.Play("MenuFail");
            } else
            {
                AudioManager.Instance.Play("MenuBuy");
                GameManager.Instance.money -= purchase.price;
                GameManager.Instance.activePowerUps[purchase.index] += 1;
            }
        }
    }

    public void NextItem(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
            if (currentItem < powerups.Length - 1)
            {
                AudioManager.Instance.Play("MenuControl");
                currentItem++;
            }

            DisplayCurrentItem();
        }
    }

    public void PreviousItem(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
            if (currentItem > 0)
            {
                AudioManager.Instance.Play("MenuControl");
                currentItem--;
            }

            DisplayCurrentItem();
        }
    }

    public void Dismiss(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
            context.action.Disable();
            GameManager.Instance.LoadNextLevel();
        }
    }

    void DisplayCurrentItem()
    {
        PowerUpItem current = powerups[currentItem];
        itemIcon.sprite = current.icon;
        itemName.text = current.name;
        itemPrice.text = current.price.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
