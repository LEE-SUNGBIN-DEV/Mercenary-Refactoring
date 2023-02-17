using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using BehaviourTreePackage;

public abstract class BaseEnemy : BaseActor
{
    public event UnityAction<BaseEnemy> OnEnemySpawn;
    public event UnityAction<BaseEnemy> OnEnemyDie;

    [Header("Base Enemy")]
    [SerializeField] protected EnemyData enemyData;
    protected EnemyStateController state;
    private Vector3 spawnPosition;

    [Header("Skills")]
    [SerializeField] protected EnemySkill currentSkill;
    protected Dictionary<int, EnemySkill> skillDictionary;

    protected NavMeshAgent navMeshAgent;
    protected Rigidbody rigidBody;

    [Header("Target")]
    [SerializeField] protected Transform targetTransform;
    [SerializeField] protected Vector3 targetDirection;
    [SerializeField] protected float targetDistance;
    
    public override void Awake()
    {
        base.Awake();
        TryGetComponent(out rigidBody);
        TryGetComponent(out navMeshAgent);
    }

    public virtual void OnEnable()
    {
        Spawn();
    }

    public virtual void OnDisable()
    {
        Despawn();
    }

    public virtual void Spawn()
    {
        OnEnemySpawn?.Invoke(this);

        navMeshAgent.speed = enemyData.MoveSpeed;
        navMeshAgent.stoppingDistance = enemyData.StopDistance;
        spawnPosition = transform.position;

        isInvincible = false;
        IsDie = false;
        enemyData.CurrentHP = enemyData.MaxHP;
        gameObject.layer = Constants.LAYER_ENEMY;

        UpdateTargetInformation();
        state?.SetState(ENEMY_STATE.Spawn);
    }

    public virtual void Despawn()
    {
        targetTransform = null;
    }

    public virtual void OnDie()
    {
        if (isDie)
            return;

        OnEnemyDie?.Invoke(this);
    }

    public bool UpdateTargetInformation()
    {
        if (targetTransform != null)
        {
            targetDistance = Vector3.Distance(targetTransform.position, transform.position);
            targetDirection = Vector3.Normalize(targetTransform.position - transform.position);
            return true;
        }
        else
            return false;
    }

    public bool IsReadyAnySkill()
    {
        foreach (var skill in skillDictionary.Values)
        {
            if (skill.IsReady(targetDistance))
            {
                currentSkill = skill;
                return true;
            }
        }
        return false;
    }

    public bool IsTargetInChaseDistance()
    {
        if (targetTransform != null
            && targetDistance <= enemyData.ChaseDistance)
        {
            return true;
        }
        return false;
    }
    public bool IsTargetInStopDistance()
    {
        if (targetTransform != null
            && targetDistance <= enemyData.StopDistance)
        {
            return true;
        }
        return false;
    }

    public bool IsTargetInSight()
    {
        return true;
    }

    public void LookTarget()
    {
        targetDirection = Vector3.Normalize(targetTransform.position - transform.position);
        transform.rotation
                = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetDirection), 5f * Time.deltaTime);
    }

    public void DamageProcess(BaseCharacter character, float ratio)
    {
        float damage = (enemyData.AttackPower - character.StatusData.DefensivePower * 0.5f) * 0.5f;

        if (damage < 0)
            damage = 0;

        damage += ((enemyData.AttackPower * 0.125f - enemyData.AttackPower * 0.0625f) + 1f);
        damage *= ratio;

        character.StatusData.CurrentHP -= damage;
    }

    public IEnumerator WaitForDisapear(float time)
    {
        float disapearTime = 0f;

        isDie = true;
        isInvincible = true;
        gameObject.layer = Constants.LAYER_ENEMY_DIE;

        while (disapearTime <= time)
        {
            disapearTime += Time.deltaTime;
            yield return null;
        }
    }

    #region Property
    public EnemyData EnemyData { get { return enemyData; } }
    public EnemyStateController State { get { return state; } }
    public Vector3 SpawnPosition { get { return spawnPosition; } }
    public Dictionary<int, EnemySkill> SkillDictionary { get { return skillDictionary; } }
    public EnemySkill CurrentSkill { get { return currentSkill; } set { currentSkill = value; } }
    public NavMeshAgent NavMeshAgent { get { return navMeshAgent; } }

    // Target
    public Transform TargetTransform { get { return targetTransform; } set { targetTransform = value; } }
    public Vector3 TargetDirection { get { return targetDirection; } }
    public float TargetDistance { get { return targetDistance; } }
    #endregion
}
