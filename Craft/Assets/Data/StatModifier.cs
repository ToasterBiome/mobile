using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatModifier
{
    public float value;

    public enum StatModType { Additive, Multiplicitive}

    public StatModType statType;

    public StatModifier(float _value,StatModType _type)
    {
        value = _value;
        statType = _type;
    }
}
