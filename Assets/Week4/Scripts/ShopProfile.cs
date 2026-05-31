using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class ShopProfile : MonoBehaviour
{
    public ShopItemSO assignedItemSO;

    [SerializeField] private Button buyButton;
    [SerializeField] private Button itemIconButton;

    private int shopID;

    private ShopMenu shopMenu;
    

    private void Start()
    {
        shopMenu = FindShopMenu();

        GenerateID();

        SetUpListeners();

        if (assignedItemSO.isBought)
            shopMenu.SwapButtonToBought(buyButton);
            
    }

    private void SetUpListeners()
    {
        buyButton.onClick.AddListener(() => shopMenu.BuyItem(assignedItemSO, shopID, buyButton));
        
        itemIconButton.onClick.AddListener(() => shopMenu.ChangeItemDescription(assignedItemSO.itemDescription));
    }    

    private ShopMenu FindShopMenu()
    {
        return GetComponentInParent<ShopMenu>();
    }

    public void GenerateID()
    {
       string tempId = System.Guid.NewGuid().ToString();

       shopID = assignedItemSO.itemCost + (tempId.GetHashCode() & 0x7FFFFFFF);

        #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
        #endif
    }
}
