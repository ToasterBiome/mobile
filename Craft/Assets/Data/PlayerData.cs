using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerData", menuName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    public List<Armor> equipment = new List<Armor>(4);
    public List<Item> inventory = new List<Item>();
    public int maxInventory = 16;

    public CharacterStat ATTACK = new CharacterStat(5);
    public CharacterStat DEFENSE = new CharacterStat(5);



    public bool addToInventory(Item item)
    {
        if (inventory.Count >= 16)
        {
            return false;
        }
        else
        {
            inventory.Add(item);
            return true;
        }
    }

    public bool Equip(Armor armor)
    {
        Armor tempArmor = null;
        //check if it is full already
        if(equipment[(int)armor.armorType] != null)
        {
            //there is something already equipped
            tempArmor = (Armor)equipment[(int)armor.armorType];
            tempArmor.OnUnequip(this);
        }
        equipment[(int)armor.armorType] = armor;
        armor.OnEquip(this);
        if (tempArmor != null)
        {
            //if there is armor
            addToInventory(tempArmor);
        }
        return true;
    }

    public bool Unequip(Armor armor)
    {
        armor.OnUnequip(this);
        equipment[(int)armor.armorType] = null;
        addToInventory(armor);
        return true;
    }

   


}
