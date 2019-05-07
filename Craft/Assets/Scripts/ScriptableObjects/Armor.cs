using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor", menuName = "Armor")]
public class Armor : Item
{
    public enum ArmorType { Helmet, Chest, Pants, Shoes, Ring }
    [Header("Armor Information")]
    public ArmorType armorType;
    public int armorValue;
    public Element element;

    public List<StatModifier> modifiers = new List<StatModifier>();

    public void OnEquip(PlayerData player)
    {
        foreach(StatModifier mod in modifiers)
        {
            player.DEFENSE.AddModifier(mod);
        }
    }

    public void OnUnequip(PlayerData player)
    {

        foreach (StatModifier mod in modifiers)
        {
            player.DEFENSE.RemoveModifier(mod);
        }
    }
}

