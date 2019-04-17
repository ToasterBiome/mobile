using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Consumable")]
public class Consumable : Item
{
    [Header("Consumable Information")]
    public int hpBonus;
    public int timeBonus;
}
