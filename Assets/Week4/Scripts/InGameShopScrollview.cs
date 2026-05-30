using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;
public class InGameShopScrollview : MonoBehaviour
{
    [SerializeField] private ScrollView shopScrollView;
    [SerializeField] private GameObject contentView;

    public string itemFolderAddress;

    private List<ItemSO> loadedItems = new List<ItemSO>();

    private void Start()
    {
       Addressables.LoadAssetsAsync<ItemSO>("ShopItem", null).Completed += LoadAllShopItems;
    }

    private void LoadAllShopItems(AsyncOperationHandle<IList<ItemSO>> handle)
    {
        if (string.IsNullOrWhiteSpace(itemFolderAddress))
            return;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            foreach (ItemSO item in handle.Result)
            {
                loadedItems.Add(item);
                Debug.Log("Loading " + item.itemName);
            }
        }
    }
}
