﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public enum Rarity { Common, Uncommon, Rare, VeryRare }
    public enum Element { None, Fire }

    [Header("Item Information")]
    public string itemName;
    public string itemDescription;
    public string goldValue;
    public Rarity rarity;
    
    [Header("Sprite Information")]
    public Texture2D sprite;
    public Color color;

    public List<ItemEffect> effects = new List<ItemEffect>();

    /* drop table chances;
     * Common - (1/5) - 20%
     * Uncommon - (1/10) - 10%
     * Rare - (1/20) - 5%
     * Ultra Rare - (1/40) - 2.5%
     */

    public void Use()
    {
        PlayerData player = BattleManager.instance.player;
        Debug.Log("Using " + itemName);
        foreach (ItemEffect effect in effects)
        {
            if(effect.effectType == ItemEffect.EffectType.Heal)
            {
                player.hp += effect.amount;
                if(player.hp > player.maxhp)
                {
                    player.hp = player.maxhp;
                }
                BattleManager.instance.refreshUI();
                BattleManager.instance.healthRestore.Play();
            }
        }
    }
 

}






