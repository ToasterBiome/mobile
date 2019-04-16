using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerData", menuName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    public List<Item> equipment = new List<Item>(5);
    public List<Item> inventory = new List<Item>();
    public int maxInventory = 16;

    public bool addToInventory(Item item)
    {
        if (inventory.Count >= 16)
        {
            return false;
        } else {
            inventory.Add(item);
            return true;
        }
    }
}
