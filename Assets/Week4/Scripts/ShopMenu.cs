using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class ShopMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentCurrencyAmount;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private GameObject inGameMenu;

    private PlayerInventory playerInventory;

    private void Start()
    {
        playerInventory = UniversalPlayerReference.Instance.gameObject.GetComponent<PlayerInventory>();

        currentCurrencyAmount.text = playerInventory.PlayerCurrency.ToString();

        ChangeMenuVisibility(false);
    }

    public void ChangeMenuVisibility(bool turnOn)
    {
        if (inGameMenu == null)
        {
            inGameMenu = GameObject.Find("InGameStoreContainer");
        }

        inGameMenu.SetActive(turnOn);
        Debug.Log("Changing Visibility");
    }

    public void BuyItem(ShopItemSO assignedItemSO, int shopID, Button buyButton)
    {

        if (assignedItemSO.itemCost > playerInventory.PlayerCurrency)
        {
            ChangeItemDescription($"You lacking in funds for {assignedItemSO.itemName}");
            return;
        }

        playerInventory.PlayerCurrency -= assignedItemSO.itemCost;
        playerInventory.AddItemToInventory(shopID, assignedItemSO.itemName);

        assignedItemSO.isBought = true;

        SwapButtonToBought(buyButton);

        currentCurrencyAmount.text = playerInventory.PlayerCurrency.ToString();
        ChangeItemDescription($"Bought {assignedItemSO.itemName}!");
    }

    public void SwapButtonToBought(Button buyButton)
    {
        buyButton.interactable = false;

        buyButton.GetComponentInChildren<TextMeshProUGUI>().text = "BOUGHT";
    }

    public void ChangeItemDescription(string desc)
    {
        itemDescription.text = "";

        itemDescription.text = desc;
    }
}
