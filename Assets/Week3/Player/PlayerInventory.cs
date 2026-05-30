/*
    Handles add/removing items into/from inventory
*/

using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    Dictionary<int, string> inventory = new Dictionary<int, string>();

    public void AddItemToInventory(int id, string name)
    {
        if (!inventory.ContainsKey(id))
            inventory.Add(id, name);
        else
            Debug.LogError("Item is either already present in inventory, or shares an id");
    }

    public void RemoveItemFromInventory(int id)
    {
        if (inventory.ContainsKey(id))
            inventory.Remove(id);
    }
}