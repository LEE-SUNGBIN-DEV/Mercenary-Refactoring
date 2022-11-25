using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StatusData
{
    public event UnityAction<StatusData> OnCharacterStatusChanged;
    public event UnityAction<StatusData> OnDie;

    // Stat
    private float attackPower;
    private float defensivePower;
    private float maxHP;
    private float currentHP;
    private float maxSP;
    private float currentSP;
    private float criticalChance;
    private float criticalDamage;
    private float attackSpeed;
    private float moveSpeed;

    // Equipment
    private float equipAttackPower;
    private float equipDefensePower;
    private float equipMaxHP;
    private float equipMaxSP;
    private float equipCriticalChance;
    private float equipCriticalDamage;
    private float equipAttackSpeed;
    private float equipMoveSpeed;

    public StatusData(StatData statData)
    {
        if(statData != null)
        {
            statData.OnChangeStatData -= UpdateStats;
            statData.OnChangeStatData += UpdateStats;
            UpdateStats(statData);
        }
    }

    public void UpdateStats(StatData statData)
    {
        AttackPower = statData.Strength * 2 + equipAttackPower;
        DefensivePower = statData.Strength + equipDefensePower;
        MaxHP = statData.Vitality * 10 + equipMaxHP;
        MaxSP = statData.Vitality * 10 + equipMaxSP;
        CriticalChance = CriticalChance = statData.Luck + equipCriticalChance;
        CriticalDamage = Constants.CHARACTER_STAT_CRITICAL_DAMAGE_DEFAULT + statData.Luck + equipCriticalDamage;
        AttackSpeed = Constants.CHARACTER_STAT_ATTACK_SPEED_DEFAULT + statData.Dexterity * 0.01f + equipAttackSpeed;
        MoveSpeed = Constants.CHARACTER_STAT_MOVE_SPEED_DEFAULT + statData.Dexterity * 0.02f + equipMoveSpeed;
    }

    #region Property
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
                OnDie(this);
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
    public float EquipAttackPower { get { return equipAttackPower; } set { equipAttackPower = value; OnCharacterStatusChanged?.Invoke(this); } }
    public float EquipDefensePower { get { return equipDefensePower; } set { equipDefensePower = value; OnCharacterStatusChanged?.Invoke(this); } }
    public float EquipMaxHP { get { return equipMaxHP; } set { equipMaxHP = value; OnCharacterStatusChanged?.Invoke(this); } }
    public float EquipMaxSP { get { return equipMaxSP; } set { equipMaxSP = value; OnCharacterStatusChanged?.Invoke(this); } }
    public float EquipCriticalChance { get { return equipCriticalChance; } set { equipCriticalChance = value; OnCharacterStatusChanged?.Invoke(this); } }
    public float EquipCriticalDamage { get { return equipCriticalDamage; } set { equipCriticalDamage = value; OnCharacterStatusChanged?.Invoke(this); } }
    public float EquipAttackSpeed { get { return equipAttackSpeed; } set { equipAttackSpeed = value; OnCharacterStatusChanged?.Invoke(this); } }
    public float EquipMoveSpeed { get { return equipMoveSpeed; } set { equipMoveSpeed = value; OnCharacterStatusChanged?.Invoke(this); } }
    #endregion
}
