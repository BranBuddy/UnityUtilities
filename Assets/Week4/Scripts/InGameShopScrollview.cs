using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI; 
public class InGameShopScrollview : MonoBehaviour
{
    [SerializeField] private GameObject contentView;
    [SerializeField] private GameObject shopProfilePrefab;

    public string itemFolderAddress;

    private List<ShopItemSO> loadedItems = new List<ShopItemSO>();

    private void Start()
    {
       Addressables.LoadAssetsAsync<ShopItemSO>("ShopItem", null).Completed += LoadAllShopItems;
    }

    private void LoadAllShopItems(AsyncOperationHandle<IList<ShopItemSO>> handle)
    {
        if (string.IsNullOrWhiteSpace(itemFolderAddress))
            return;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            foreach (ShopItemSO item in handle.Result)
            {
                loadedItems.Add(item);
                BuildShopProfile(item);
                Debug.Log("Loading " + item.itemName);
            }
        }
    }

    public void BuildShopProfile(ShopItemSO item)
    {
        GameObject profile = Instantiate(shopProfilePrefab, contentView.transform);

        Image image = profile.GetComponentInChildren<Image>();
        TextMeshProUGUI text = profile.GetComponentInChildren<TextMeshProUGUI>();

        ShopProfile shopProfile = profile.GetComponent<ShopProfile>();

        shopProfile.assignedItemSO = item;
        
        if (image != null)
            image.sprite = item.itemSprite;
        if (text != null)
            text.text = $"{item.itemName}:\n{item.itemCost}";

    }
}
