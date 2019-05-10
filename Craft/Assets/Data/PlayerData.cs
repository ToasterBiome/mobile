using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerData", menuName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    public List<Armor> equipment = new List<Armor>(5);
    public List<ItemStack> inventory = new List<ItemStack>();
    public int maxInventory = 16;

    public int hp = 100;
    public int maxhp = 100;

    public CharacterStat ATTACK = new CharacterStat(5);
    public CharacterStat DEFENSE = new CharacterStat(5);



    public bool addToInventory(int slot, ItemStack item)
    {
        if (inventory.Count < 16)
        {
            //it can fit
            item.SetSlot(slot);
            inventory.Add(item);
            return true;
        } else
        {
            return false;
        }
    }
    public bool removeFromInventory(int slot)
    {
        inventory[slot] = new ItemStack(null, 0);
        //return inventory.Remove(inventory[slot]);
        return true;
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
            addToInventory(inventory.Count, new ItemStack(tempArmor,1));
        }
        return true;
    }

    public bool Unequip(Armor armor)
    {
        armor.OnUnequip(this);
        equipment[(int)armor.armorType] = null;
        addToInventory(inventory.Count, new ItemStack(armor, 1));
        return true;
    }
   


}
