using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EnemyData
{
    public event UnityAction<EnemyData> OnChanageEnemyData;
    public event UnityAction<EnemyData> OnDie;

    [Header("Identifier")]
    [SerializeField] private uint enemyID;
    [SerializeField] private string enemyName;

    [Header("Status")]
    [SerializeField] private float maxHP;
    [SerializeField] private float currentHP;
    [SerializeField] private float attackPower;
    [SerializeField] private float defensivePower;
    [SerializeField] private float criticalChance;
    [SerializeField] private float criticalDamage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float moveSpeed;

    [Header("Reward")]
    [SerializeField] private float expAmount;
    [SerializeField] private float moneyAmount;

    public void Initialize()
    {
        currentHP = maxHP;
    }

    #region Status Property
    public uint EnemyID { get { return enemyID; } }
    public string EnemyName
    {
        get { return enemyName; }
    }
    public float MaxHP
    {
        get { return maxHP; }
        set
        {
            maxHP = value;
            if (maxHP <= 0)
                maxHP = 1;

            OnChanageEnemyData?.Invoke(this);
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

            OnChanageEnemyData?.Invoke(this);
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

            OnChanageEnemyData?.Invoke(this);
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

            OnChanageEnemyData?.Invoke(this);
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

            OnChanageEnemyData?.Invoke(this);
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

            OnChanageEnemyData?.Invoke(this);
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

            OnChanageEnemyData?.Invoke(this);
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

            OnChanageEnemyData?.Invoke(this);
        }
    }
    #endregion
}
