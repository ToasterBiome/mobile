using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect
{
    public enum EffectType { HP, Time };

    public EffectType effectType;
    public int effectAmount;
}