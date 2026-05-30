using UnityEngine;

[CreateAssetMenu(fileName = "ItemS0", menuName = "Scriptable Objects/ItemS0")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public int itemCost;
    [Multiline] public string itemDescription;
    public Sprite itemSprite;
}
