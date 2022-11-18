using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CharacterStatData
{
    public event UnityAction<CharacterStatData> OnChangeStatData;

    [Header("Stats")]
    [SerializeField] private string characterClass;
    [SerializeField] private int level;
    [SerializeField] private float currentExperience;
    [SerializeField] private float maxExperience;
    [SerializeField] private int statPoint;
    [SerializeField] private int strength;
    [SerializeField] private int vitality;
    [SerializeField] private int dexterity;
    [SerializeField] private int luck;

    public CharacterStatData(CHARACTER_CLASS selectedClass)
    {
        CreateStatData(selectedClass);
    }

    public void CreateStatData(CHARACTER_CLASS selectedClass)
    {
        characterClass = System.Enum.GetName(typeof(CHARACTER_CLASS), selectedClass);
        level = Constants.CHARACTER_DATA_DEFALUT_LEVEL;
        currentExperience = Constants.CHARACTER_DATA_DEFALUT_EXPERIENCE;
        maxExperience = Managers.DataManager.LevelTable[level];

        statPoint = Constants.CHARACTER_DATA_DEFALUT_STATPOINT;
        strength = Constants.CHARACTER_DATA_DEFALUT_STRENGTH;
        vitality = Constants.CHARACTER_DATA_DEFALUT_VITALITY;
        dexterity = Constants.CHARACTER_DATA_DEFALUT_DEXTERITY;
        luck = Constants.CHARACTER_DATA_DEFALUT_LUCK;
    }

    public void LevelUp()
    {
        currentExperience -= maxExperience;
        maxExperience = Managers.DataManager.LevelTable[Level];
        ++Level;
        StatPoint += 5;
    }

    #region Property
    public string CharacterClass
    {
        get { return characterClass; }
        set
        {
            characterClass = value;
            OnChangeStatData?.Invoke(this);
        }
    }
    public int Level
    {
        get { return level; }
        set
        {
            level = value;
            if (level < 1)
            {
                level = 1;
            }
            OnChangeStatData?.Invoke(this);
        }
    }

    public float CurrentExperience
    {
        get { return currentExperience; }
        set
        {
            currentExperience = value;
            if (currentExperience < 0)
            {
                currentExperience = 0;
            }

            while (currentExperience >= MaxExperience)
            {
                LevelUp();
            }

            OnChangeStatData?.Invoke(this);
        }
    }

    public float MaxExperience
    {
        get { return maxExperience; }
        set
        {
            maxExperience = value;
            if (maxExperience < 1)
            {
                maxExperience = 1;
            }
            OnChangeStatData?.Invoke(this);
        }
    }

    public int StatPoint
    {
        get { return statPoint; }
        set
        {
            statPoint = value;
            if (statPoint < 0)
            {
                statPoint = 0;
            }
            OnChangeStatData?.Invoke(this);
        }
    }
    public int Strength
    {
        get { return strength; }
        set
        {
            strength = value;
            if (strength < 0)
            {
                strength = 0;
            }
            OnChangeStatData?.Invoke(this);
        }
    }

    public int Vitality
    {
        get { return vitality; }
        set
        {
            vitality = value;
            if (vitality < 0)
            {
                vitality = 0;
            }
            OnChangeStatData?.Invoke(this);
        }
    }

    public int Dexterity
    {
        get { return dexterity; }
        set
        {
            dexterity = value;

            if (dexterity < 0)
            {
                dexterity = 0;
            }
            OnChangeStatData?.Invoke(this);
        }
    }
    public int Luck
    {
        get { return luck; }
        set
        {
            luck = value;
            if (luck < 0)
            {
                luck = 0;
            }
            OnChangeStatData?.Invoke(this);
        }
    }
    #endregion
}
