using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    //item properties
    public enum Rarity { Common, Uncommon, Rare, VeryRare }
    public enum Element { None, Fire }

    public string itemName;
    public string value;
    public Rarity rarity;

    public Color tint;

    public Texture2D sprite;

    /* drop table chances;
     * Common - (1/5) - 20%
     * Uncommon - (1/10) - 10%
     * Rare - (1/20) - 5%
     * Ultra Rare - (1/40) - 2.5%
     */

}

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : Item
{
    public enum WeaponType { Longsword, Dagger, TwoHander }
    public WeaponType weaponType;
    public int damage;
    public Element element;
}

[CreateAssetMenu(fileName = "New Armor", menuName = "Armor")]
public class Armor : Item
{
    public enum ArmorType { Helmet, Chest, Pants, Shoes, Cape, Gloves }
    public ArmorType armorType;
    public int armorValue;
    public Element element;
}

[CreateAssetMenu(fileName = "New Recipe", menuName = "Recipe")]
public class Recipe : ScriptableObject
{
    public List<Item> recipe = new List<Item>();
    public Item item;
}
