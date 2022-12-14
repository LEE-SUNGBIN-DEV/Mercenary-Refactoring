using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StatusData
{
    public event UnityAction<StatusData> OnCharacterStatusChanged;
    public event UnityAction<StatusData> OnDie;

    [Header("Stats")]
    [SerializeField] private string characterClass;
    [SerializeField] private int level;
    [SerializeField] private int statPoint;
    [SerializeField] private int strength;
    [SerializeField] private int vitality;
    [SerializeField] private int dexterity;
    [SerializeField] private int luck;

    [Header("Final Status")]
    [SerializeField] private float maxExp;
    [SerializeField] private float currentExp;
    [SerializeField] private float maxHP;
    [SerializeField] private float currentHP;
    [SerializeField] private float maxSP;
    [SerializeField] private float currentSP;
    [SerializeField] private float attackPower;
    [SerializeField] private float defensivePower;
    [SerializeField] private float criticalChance;
    [SerializeField] private float criticalDamage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float moveSpeed;

    [Header("Equipment Effect")]
    private float equipAttackPower;
    private float equipDefensePower;
    private float equipMaxHP;
    private float equipMaxSP;
    private float equipCriticalChance;
    private float equipCriticalDamage;
    private float equipAttackSpeed;
    private float equipMoveSpeed;

    public void Initialize(CHARACTER_TYPE selectedClass)
    {
        characterClass = System.Enum.GetName(typeof(CHARACTER_TYPE), selectedClass);
        level = Constants.CHARACTER_DATA_DEFALUT_LEVEL;

        maxExp = Managers.DataManager.LevelTable[level];
        currentExp = Constants.CHARACTER_DATA_DEFALUT_EXPERIENCE;

        statPoint = Constants.CHARACTER_DATA_DEFALUT_STATPOINT;
        strength = Constants.CHARACTER_DATA_DEFALUT_STRENGTH;
        vitality = Constants.CHARACTER_DATA_DEFALUT_VITALITY;
        dexterity = Constants.CHARACTER_DATA_DEFALUT_DEXTERITY;
        luck = Constants.CHARACTER_DATA_DEFALUT_LUCK;

        UpdateStatus();
        currentHP = maxHP;
        currentSP = maxSP;
    }

    public void UpdateStatus()
    {
        maxExp = Managers.DataManager.LevelTable[level];
        MaxHP = (vitality * 10) + equipMaxHP;
        MaxSP = (vitality * 10) + equipMaxSP;
        AttackPower = (strength * 2) + equipAttackPower;
        DefensivePower = (strength) + equipDefensePower;
        CriticalChance = (luck) + equipCriticalChance;
        CriticalDamage = Constants.CHARACTER_STAT_CRITICAL_DAMAGE_DEFAULT + (luck) + equipCriticalDamage;
        AttackSpeed = Constants.CHARACTER_STAT_ATTACK_SPEED_DEFAULT + (dexterity * 0.01f) + equipAttackSpeed;
        MoveSpeed = Constants.CHARACTER_STAT_MOVE_SPEED_DEFAULT + (dexterity * 0.02f) + equipMoveSpeed;
    }

    public void LevelUp()
    {
        currentExp -= maxExp;
        maxExp = Managers.DataManager.LevelTable[Level];
        ++Level;
        StatPoint += 5;
    }

    #region Stat Property
    public string CharacterClass
    {
        get { return characterClass; }
        set
        {
            characterClass = value;
            OnCharacterStatusChanged?.Invoke(this);
        }
    }
    public int Level
    {
        get { return level; }
        set
        {
            level = value;
            if (level < 1)
                level = 1;

            OnCharacterStatusChanged?.Invoke(this);
        }
    }
    public int StatPoint
    {
        get { return statPoint; }
        set
        {
            statPoint = value;
            if (statPoint < 0)
                statPoint = 0;

            OnCharacterStatusChanged?.Invoke(this);
        }
    }
    public int Strength
    {
        get { return strength; }
        set
        {
            strength = value;
            if (strength < 0)
                strength = 0;

            UpdateStatus();
            OnCharacterStatusChanged?.Invoke(this);
        }
    }

    public int Vitality
    {
        get { return vitality; }
        set
        {
            vitality = value;
            if (vitality < 0)
                vitality = 0;

            UpdateStatus();
            OnCharacterStatusChanged?.Invoke(this);
        }
    }

    public int Dexterity
    {
        get { return dexterity; }
        set
        {
            dexterity = value;

            if (dexterity < 0)
                dexterity = 0;

            UpdateStatus();
            OnCharacterStatusChanged?.Invoke(this);
        }
    }
    public int Luck
    {
        get { return luck; }
        set
        {
            luck = value;
            if (luck < 0)
                luck = 0;

            UpdateStatus();
            OnCharacterStatusChanged?.Invoke(this);
        }
    }
    #endregion

    #region Status Property
    public float MaxExp
    {
        get { return maxExp; }
        set
        {
            maxExp = value;
            if (maxExp < 1)
                maxExp = 1;

            OnCharacterStatusChanged?.Invoke(this);
        }
    }
    public float CurrentExp
    {
        get { return currentExp; }
        set
        {
            currentExp = value;
            if (currentExp < 0)
                currentExp = 0;

            while (currentExp >= MaxExp)
                LevelUp();

            OnCharacterStatusChanged?.Invoke(this);
        }
    }
    public float MaxHP
    {
        get { return maxHP; }
        set
        {
            maxHP = value;
            if (maxHP <= 0)
                maxHP = 1;

            OnCharacterStatusChanged?.Invoke(this);
        }
    }
    public float CurrentHP
    {
        get { return currentHP; }
        set
        {
            currentHP = value;
            if (currentHP > MaxHP)
                currentHP = MaxHP;

            if (currentHP < 0)
            {
                currentHP = 0;
                OnDie?.Invoke(this);
            }

            OnCharacterStatusChanged?.Invoke(this);
        }
    }
    public float MaxSP
    {
        get { return maxSP; }
        set
        {
            maxSP = value;
            if (maxSP <= 0)
                maxSP = 1;

            OnCharacterStatusChanged?.Invoke(this);
        }
    }
    public float CurrentSP
    {
        get { return currentSP; }
        set
        {
            currentSP = value;
            if (currentSP > MaxSP)
                currentSP = MaxSP;

            if (currentSP < 0)
                currentSP = 0;

            OnCharacterStatusChanged?.Invoke(this);
        }
    }
    public float AttackPower
    {
        get { return attackPower; }
        set
        {
            attackPower = value;
            if (attackPower < 0)
                attackPower = 0;

            OnCharacterStatusChanged?.Invoke(this);
        }
    }
    public float DefensivePower
    {
        get { return defensivePower; }
        set
        {
            defensivePower = value;
            if (defensivePower < 0)
                defensivePower = 0;

            OnCharacterStatusChanged?.Invoke(this);
        }
    }
    public float AttackSpeed
    {
        get { return attackSpeed; }
        set
        {
            attackSpeed = value;

            if (attackSpeed < Constants.CHARACTER_STAT_ATTACK_SPEED_MIN)
                attackSpeed = Constants.CHARACTER_STAT_ATTACK_SPEED_MIN;

            if (attackSpeed > Constants.CHARACTER_STAT_ATTACK_SPEED_MAX)
                attackSpeed = Constants.CHARACTER_STAT_ATTACK_SPEED_MAX;

            OnCharacterStatusChanged?.Invoke(this);
        }
    }
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set
        {
            moveSpeed = value;

            if (moveSpeed < Constants.CHARACTER_STAT_MOVE_SPEED_MIN)
                moveSpeed = Constants.CHARACTER_STAT_MOVE_SPEED_MIN;

            if (moveSpeed > Constants.CHARACTER_STAT_MOVE_SPEED_MAX)
                moveSpeed = Constants.CHARACTER_STAT_MOVE_SPEED_MAX;

            OnCharacterStatusChanged?.Invoke(this);
        }
    }
    public float CriticalChance
    {
        get { return criticalChance; }
        set
        {
            criticalChance = value;
            if (criticalChance < Constants.CHARACTER_STAT_CRITICAL_CHANCE_MIN)
                criticalChance = Constants.CHARACTER_STAT_CRITICAL_CHANCE_MIN;

            if (criticalChance > Constants.CHARACTER_STAT_CRITICAL_CHANCE_MAX)
                criticalChance = Constants.CHARACTER_STAT_CRITICAL_CHANCE_MAX;
            
            OnCharacterStatusChanged?.Invoke(this);
        }
    }
    public float CriticalDamage
    {
        get { return criticalDamage; }
        set
        {
            criticalDamage = value;
            if (criticalDamage < Constants.CHARACTER_STAT_CRITICAL_DAMAGE_MIN)
                criticalDamage = Constants.CHARACTER_STAT_CRITICAL_DAMAGE_MIN;

            OnCharacterStatusChanged?.Invoke(this);
        }
    }
    public float EquipAttackPower
    { get { return equipAttackPower; } set { equipAttackPower = value; UpdateStatus(); } }
    public float EquipDefensePower
    { get { return equipDefensePower; } set { equipDefensePower = value; UpdateStatus(); } }
    public float EquipMaxHP
    { get { return equipMaxHP; } set { equipMaxHP = value; UpdateStatus(); } }
    public float EquipMaxSP
    { get { return equipMaxSP; } set { equipMaxSP = value; UpdateStatus(); } }
    public float EquipCriticalChance
    { get { return equipCriticalChance; } set { equipCriticalChance = value; UpdateStatus(); } }
    public float EquipCriticalDamage
    { get { return equipCriticalDamage; } set { equipCriticalDamage = value; UpdateStatus(); } }
    public float EquipAttackSpeed
    { get { return equipAttackSpeed; } set { equipAttackSpeed = value; UpdateStatus(); } }
    public float EquipMoveSpeed
    { get { return equipMoveSpeed; } set { equipMoveSpeed = value; UpdateStatus(); } }
    #endregion
}
