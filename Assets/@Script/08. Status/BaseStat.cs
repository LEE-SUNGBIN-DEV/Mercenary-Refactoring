using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#region Status Data
[System.Serializable]
public class BaseStat
{
    public event UnityAction OnStatModified;
    private float baseValue;
    private float finalValue;

    private float minValue;
    private float maxValue;

    private bool isModified;
    private List<StatModifier> statusModifierList = new List<StatModifier>();

    public float BaseValue
    {
        get { return baseValue; }
        set
        {
            isModified = true;
            baseValue = value;
            OnStatModified?.Invoke();
        }
    }

    public BaseStat(float defaultValue, float minValue = 0, float maxValue = float.PositiveInfinity)
    {
        this.baseValue = defaultValue;
        this.minValue = minValue;
        this.maxValue = maxValue;
        isModified = true;
        OnStatModified?.Invoke();
    }

    public void AddModifier(StatModifier statusModifier)
    {
        statusModifierList.Add(statusModifier);
        isModified = true;
        OnStatModified?.Invoke();
    }

    public bool RemoveModifier(StatModifier statusModifier)
    {
        bool isRemoved = statusModifierList.Remove(statusModifier);
        isModified = true;
        OnStatModified?.Invoke();

        return isRemoved;
    }

    public void RemoveAllModifiersFromSource(object source)
    {
        // From Back
        for (int i = statusModifierList.Count - 1; i >= 0; i--)
        {
            if (statusModifierList[i].source == source)
            {
                statusModifierList.RemoveAt(i);
                isModified = true;
                OnStatModified?.Invoke();
            }
        }
    }

    public float GetIncreaseRate()
    {
        return baseValue != 0 ? ((GetFinalValue() / baseValue) - 1) * 100 : GetFinalValue();
    }
    public float GetFinalValue()
    {
        if (!isModified)
            return finalValue;

        else
        {
            finalValue = baseValue;
            float percentageSum = 0;
            for (int i = 0; i < statusModifierList.Count; ++i)
            {
                switch (statusModifierList[i].calculationType)
                {
                    case VALUE_TYPE.FIXED:
                        finalValue += statusModifierList[i].modifierValue;
                        break;
                    case VALUE_TYPE.PERCENTAGE:
                        percentageSum += statusModifierList[i].modifierValue;
                        break;
                }
            }
            finalValue *= (percentageSum * 0.01f + 1);
            isModified = false;
            return Mathf.Clamp(finalValue, minValue, maxValue);
        }
    }
}

public class StatModifier
{
    public float modifierValue;
    public VALUE_TYPE calculationType;
    public object source;

    public StatModifier(float modifierValue, VALUE_TYPE calculationType, object source)
    {
        this.modifierValue = modifierValue;
        this.calculationType = calculationType;
        this.source = source;
    }
}
#endregion
