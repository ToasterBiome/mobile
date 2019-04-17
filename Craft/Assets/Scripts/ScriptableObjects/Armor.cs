using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor", menuName = "Armor")]
public class Armor : Item
{
    public enum ArmorType { Helmet, Chest, Pants, Shoes, Cape, Gloves }
    [Header("Armor Information")]
    public ArmorType armorType;
    public int armorValue;
    public Element element;
}

