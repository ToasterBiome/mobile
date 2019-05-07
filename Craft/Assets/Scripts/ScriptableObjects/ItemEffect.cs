using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public enum EffectType { Heal, StatModifier, Teleport }

    public StatModifier modifier;

    public EffectType effectType;

    public int amount;

    public ItemEffect(EffectType et,StatModifier mod)
    {
        effectType = et;
        modifier = mod;
    }
}
