using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Recipe")]
public class Recipe : ScriptableObject
{
    [Header("Ingredients")]
    public List<Item> recipe = new List<Item>();
    [Header("Result")]
    public Item item;
}