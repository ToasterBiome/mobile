using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterStat
{
    public float baseValue;

    public bool isDirty = false;

    public List<StatModifier> statModifiers = new List<StatModifier>();

    public CharacterStat(float baseValue)
    {
        this.baseValue = baseValue;
    }

    private float _value;

    public float Value
    {
        get
        {
            if(isDirty)
            {
                _value = calculateValue();
                isDirty = false;
            }
            return _value;
        }
    }

    public void AddModifier(StatModifier mod)
    {
        isDirty = true;
        statModifiers.Add(mod);
    }

    public bool RemoveModifier(StatModifier mod)
    {  
        isDirty = true;
        return statModifiers.Remove(mod);
    }

    public float calculateValue()
    {
        float finalValue = baseValue;

        float multiplyValue = 1f;

        foreach(StatModifier sm in statModifiers)
        {
            switch (sm.statType)
            {
                case StatModifier.StatModType.Additive:
                    finalValue += sm.value;
                    break;
                case StatModifier.StatModType.Multiplicitive:
                    multiplyValue += sm.value;
                    break;
                default:
                    finalValue += sm.value;
                    break;
            }
            
        }
        finalValue *= multiplyValue;
        return finalValue;
    }
}
