using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : Item
{
    public enum WeaponType { Longsword, Dagger, TwoHander }
    [Header("Weapon Information")]
    public WeaponType weaponType;
    public int damage;
    public Element element;
}
