using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Monster", menuName = "Monster")]
public class Monster : ScriptableObject
{
    public string monsterName;
    public Sprite sprite;
    public int baseHP;
    public int baseDEF;
    public int level = 10;


    public bool hasShield = false;
    public Sprite sheildSprite;
    public int baseShieldHP;
    public int baseShieldDEF;

    public LootTable loot;

    public float strongAttackChance = 0f;
}



