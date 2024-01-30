using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class StatOption
{
    public StatData statData;
    public VALUE_TYPE valueType;
    public float value;

    public StatOption(string statID, VALUE_TYPE valueType, float value)
    {
        if (Managers.DataManager.StatTable.TryGetValue(statID, out statData))
        {
            this.valueType = valueType;
            this.value = value;
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogWarning($"[Warning]: {statID} is not in Status Option Table");
#endif
        }
    }

    public void ApplyToStatus(CharacterStatusData statusData)
    {
        statusData.StatDict[statData.statType].AddModifier(new StatModifier(value, valueType, this));
    }
    public void ReleaseFromStatus(CharacterStatusData statusData)
    {
        statusData.StatDict[statData.statType].RemoveAllModifiersFromSource(this);
    }
    public string GetStatValueUnit()
    {
        string statUnit = string.Empty;
        switch (valueType)
        {
            case VALUE_TYPE.FIXED:
                break;
            case VALUE_TYPE.PERCENTAGE:
                statUnit = "%";
                break;
        }
        return statUnit;
    }

    public string StatOptionID { get { return statData?.statID; } }
    public string StatOptionName { get { return statData?.statName; } }
    public STAT_TYPE StatOptionType { get { return statData.statType; } }

}
