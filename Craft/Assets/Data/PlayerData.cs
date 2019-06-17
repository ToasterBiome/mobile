using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerData", menuName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    public List<Armor> equipment = new List<Armor>(5);
    public int maxInventory = 16;

    public ItemContainer inv = new ItemContainer(16);
    public ItemContainer starterEquipment = new ItemContainer(4);

    public int hp = 100;
    public int maxhp = 100;

    public CharacterStat ATTACK = new CharacterStat(5);
    public CharacterStat DEFENSE = new CharacterStat(5);


}
