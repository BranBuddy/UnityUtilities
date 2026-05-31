using UnityEngine;

[CreateAssetMenu(fileName = "ShopItemSO", menuName = "Scriptable Objects/ItemS0")]
public class ShopItemSO : ScriptableObject
{
    public string itemName;
    public int itemCost;
    [Multiline] public string itemDescription;
    public Sprite itemSprite;
    public bool isBought = false;
}
