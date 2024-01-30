using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;

[System.Serializable]
public class EnemyStatus
{
    public event UnityAction<EnemyStatus> OnChangeEnemyData;

    [Header("Identifier")]
    [SerializeField] private string enemyID;
    [SerializeField] private string enemyName;
    [SerializeField] private ENEMY_TYPE enemyType;

    [Header("Status")]
    [SerializeField] private float maxHP;
    [SerializeField] private float currentHP;
    [SerializeField] private float attackPower;
    [SerializeField] private float defensePower;
    [SerializeField] private float criticalChance;
    [SerializeField] private float criticalDamage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float fixedDamage;
    [SerializeField] private float defensePenetration;
    [SerializeField] private float damageReduction;

    [SerializeField] private float stopDistance;
    [SerializeField] private float detectionDistance;
    [SerializeField] private float chaseDistance;

    [Header("Drop Reward")]
    [SerializeField] private string dropID;

    public EnemyStatus(EnemyData enemyData)
    {
        LoadData(enemyData);
    }

    public void LoadData(EnemyData enemyData)
    {
        enemyID = enemyData.enemyID;
        enemyName = enemyData.enemyName;
        enemyType = enemyData.enemyType;

        maxHP = enemyData.maxHP;
        attackPower = enemyData.attackPower;
        defensePower = enemyData.defensePower;
        criticalChance = enemyData.criticalChance;
        criticalDamage = enemyData.criticalDamage;
        attackSpeed = enemyData.attackSpeed;
        moveSpeed = enemyData.moveSpeed;
        fixedDamage = enemyData.fixedDamage;
        defensePenetration = enemyData.defensePenetration;
        damageReduction = enemyData.damageReduction;

        detectionDistance = enemyData.detectionDistance;
        chaseDistance = enemyData.chaseDistance;

        dropID = enemyData.dropID;

        currentHP = maxHP;
    }

    public float GetHPRatio()
    {
        return currentHP / maxHP;
    }

    public void DropReward(CharacterData characterData)
    {
        Managers.DataManager.DropTable.TryGetValue(dropID, out DropTableData dropData);
        Functions.DropItem(characterData, dropData);
    }

    #region Status Property
    public string EnemyID { get { return enemyID; } set { enemyID = value; } }
    public string EnemyName { get { return enemyName; } set { enemyName = value; } }
    public ENEMY_TYPE EnemyType { get { return enemyType; } set { enemyType = value; } }
    public float MaxHP
    {
        get { return maxHP; }
        set
        {
            maxHP = value;
            if (maxHP < 0)
                maxHP = 0;

            OnChangeEnemyData?.Invoke(this);
        }
    }
    [JsonIgnore]
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
            }

            OnChangeEnemyData?.Invoke(this);
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

            OnChangeEnemyData?.Invoke(this);
        }
    }
    public float DefensePower
    {
        get { return defensePower; }
        set
        {
            defensePower = value;
            if (defensePower < 0)
                defensePower = 0;

            OnChangeEnemyData?.Invoke(this);
        }
    }

    public float CriticalChance
    {
        get { return criticalChance; }
        set
        {
            criticalChance = value;
            OnChangeEnemyData?.Invoke(this);
        }
    }
    public float CriticalDamage
    {
        get { return criticalDamage; }
        set
        {
            criticalDamage = value;
            OnChangeEnemyData?.Invoke(this);
        }
    }
    public float AttackSpeed
    {
        get { return attackSpeed; }
        set
        {
            attackSpeed = value;
            OnChangeEnemyData?.Invoke(this);
        }
    }
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set
        {
            moveSpeed = value;
            OnChangeEnemyData?.Invoke(this);
        }
    }
    public float FixedDamage
    {
        get { return fixedDamage; }
        set
        {
            fixedDamage = value;
            OnChangeEnemyData?.Invoke(this);
        }
    }
    public float DefensePenetration
    {
        get { return defensePenetration; }
        set
        {
            defensePenetration = value;
            OnChangeEnemyData?.Invoke(this);
        }
    }
    public float DamageReduction
    {
        get { return damageReduction; }
        set
        {
            damageReduction = value;
            OnChangeEnemyData?.Invoke(this);
        }
    }

    [JsonIgnore]
    public float StopDistance
    {
        get { return stopDistance; }
        set
        {
            stopDistance = value;
            if (stopDistance < 0)
                stopDistance = 0;

            OnChangeEnemyData?.Invoke(this);
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

            OnChangeEnemyData?.Invoke(this);
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

            OnChangeEnemyData?.Invoke(this);
        }
    }

    // Reward
    public string DropDataID
    {
        get { return dropID; }
        set
        {
            dropID = value;
            OnChangeEnemyData?.Invoke(this);
        }
    }

    #endregion
}
