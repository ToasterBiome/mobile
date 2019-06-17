using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BasicInventory : EditorWindow
{
    // Add menu item
    [MenuItem("CONTEXT/PlayerData/Set Basic Inventory")]
    static void SetBasicInventory(MenuCommand command)
    {
        PlayerData player = (PlayerData)command.context;
        player.inv.AddFromContainer(player.starterEquipment);

    }
}