using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;

[System.Serializable]
public class EnemyData
{
    public event UnityAction<EnemyData> OnChanageEnemyData;

    [Header("Identifier")]
    [SerializeField] private uint enemyID;
    [SerializeField] private string enemyName;

    [Header("Status")]
    [SerializeField] private float maxHP;
    [JsonIgnore][SerializeField] private float currentHP;
    [SerializeField] private float attackPower;
    [SerializeField] private float defensivePower;
    [SerializeField] private float criticalChance;
    [SerializeField] private float criticalDamage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float moveSpeed;
    [JsonIgnore][SerializeField] private int hitLevel;

    [SerializeField] private float stopDistance;
    [SerializeField] private float detectionDistance;
    [SerializeField] private float chaseDistance;

    [Header("Reward Info")]
    [SerializeField] private float expReward;
    [SerializeField] private float stoneReward;

    public void Initialize()
    {
        currentHP = maxHP;
    }

    #region Status Property
    public uint EnemyID { get { return enemyID; } private set { enemyID = value; } }
    public string EnemyName { get { return enemyName; } private set { enemyName = value; } }
    public float MaxHP
    {
        get { return maxHP; }
        set
        {
            maxHP = value;
            if (maxHP < 0)
                maxHP = 0;

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
                currentHP = 0;

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

    public float CriticalChance
    {
        get { return criticalChance; }
        set
        {
            criticalChance = value;
            if (criticalChance < Constants.PLAYER_STAT_CRITICAL_CHANCE_MIN)
                criticalChance = Constants.PLAYER_STAT_CRITICAL_CHANCE_MIN;

            if (criticalChance > Constants.PLAYER_STAT_CRITICAL_CHANCE_MAX)
                criticalChance = Constants.PLAYER_STAT_CRITICAL_CHANCE_MAX;

            OnChanageEnemyData?.Invoke(this);
        }
    }
    public float CriticalDamage
    {
        get { return criticalDamage; }
        set
        {
            criticalDamage = value;
            if (criticalDamage < Constants.PLAYER_STAT_CRITICAL_DAMAGE_MIN)
                criticalDamage = Constants.PLAYER_STAT_CRITICAL_DAMAGE_MIN;

            OnChanageEnemyData?.Invoke(this);
        }
    }
    public float AttackSpeed
    {
        get { return attackSpeed; }
        set
        {
            attackSpeed = value;

            if (attackSpeed < Constants.PLAYER_STAT_ATTACK_SPEED_MIN)
                attackSpeed = Constants.PLAYER_STAT_ATTACK_SPEED_MIN;

            if (attackSpeed > Constants.PLAYER_STAT_ATTACK_SPEED_MAX)
                attackSpeed = Constants.PLAYER_STAT_ATTACK_SPEED_MAX;

            OnChanageEnemyData?.Invoke(this);
        }
    }
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set
        {
            moveSpeed = value;

            if (moveSpeed < Constants.PLAYER_STAT_MOVE_SPEED_MIN)
                moveSpeed = Constants.PLAYER_STAT_MOVE_SPEED_MIN;

            if (moveSpeed > Constants.PLAYER_STAT_MOVE_SPEED_MAX)
                moveSpeed = Constants.PLAYER_STAT_MOVE_SPEED_MAX;

            OnChanageEnemyData?.Invoke(this);
        }
    }

    public int HitLevel
    {
        get { return hitLevel; }
        set
        {
            hitLevel = value;
            if (hitLevel < 0)
                hitLevel = 0;

            OnChanageEnemyData?.Invoke(this);
        }
    }

    public float StopDistance
    {
        get { return stopDistance; }
        set
        {
            stopDistance = value;
            if (stopDistance < 0)
                stopDistance = 0;

            OnChanageEnemyData?.Invoke(this);
        }
    }

    public float DetectionDistance
    {
        get { return detectionDistance; }
        set
        {
            detectionDistance = value;
            if (detectionDistance < 0)
                detectionDistance = 0;

            OnChanageEnemyData?.Invoke(this);
        }
    }

    public float ChaseDistance
    {
        get { return chaseDistance; }
        set
        {
            chaseDistance = value;
            if (chaseDistance < 0)
                chaseDistance = 0;

            OnChanageEnemyData?.Invoke(this);
        }
    }

    // Reward
    public float ExpReward
    {
        get { return expReward; }
        set
        {
            expReward = value;
            if (expReward < 0)
                expReward = 0;

            OnChanageEnemyData?.Invoke(this);
        }
    }
    public float StoneReward
    {
        get { return stoneReward; }
        set
        {
            stoneReward = value;
            if (stoneReward < 0)
                stoneReward = 0;

            OnChanageEnemyData?.Invoke(this);
        }
    }
    #endregion
}
