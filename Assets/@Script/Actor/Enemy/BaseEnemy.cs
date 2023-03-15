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
    [SerializeField] protected EnemyData status;
    protected Vector3 spawnPosition;

    [Header("Status Effect")]
    [SerializeField] protected StatusEffectController<BaseEnemy> statusEffectControler;

    [Header("Skill")]
    [SerializeField] protected EnemySkill[] skillArray;
    [SerializeField] protected EnemySkill currentSkill;

    protected NavMeshAgent navMeshAgent;

    [Header("Target")]
    [SerializeField] protected Transform targetTransform;
    [SerializeField] protected Vector3 targetDirection;
    [SerializeField] protected float targetDistance;
    
    public override void Awake()
    {
        base.Awake();

        TryGetComponent(out navMeshAgent);
        skillArray = GetComponents<EnemySkill>();

        for (int i = 0; i < skillArray.Length; ++i)
        {
            skillArray[i].Initialize(this);
        }

        state.StateDictionary.Add(ACTION_STATE.ENEMY_IDLE, new EnemyStateIdle(this));
        state.StateDictionary.Add(ACTION_STATE.COMMON_DIE, new EnemyStateDie(this));
        state.SetState(ACTION_STATE.PLAYER_IDLE, STATE_SWITCH_BY.FORCED);
    }

    public virtual void OnEnable()
    {
        Spawn();
    }

    public virtual void OnDisable()
    {
        Despawn();
    }

    public virtual void Update()
    {
        UpdateTargetInformation();
        state.Update();
    }

    public virtual void Spawn()
    {
        OnEnemySpawn?.Invoke(this);

        if (navMeshAgent != null)
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.updatePosition = false;
            navMeshAgent.updateRotation = false;
            navMeshAgent.speed = status.MoveSpeed;
            navMeshAgent.stoppingDistance = status.StopDistance;
            navMeshAgent.velocity = Vector3.zero;
        }
        spawnPosition = transform.position;

        isInvincible = false;
        IsDie = false;
        status.CurrentHP = status.MaxHP;
        gameObject.layer = Constants.LAYER_ENEMY;

        UpdateTargetInformation();
        state?.SetState(ACTION_STATE.ENEMY_SPAWN, STATE_SWITCH_BY.FORCED);
    }

    public virtual void Despawn()
    {
        targetTransform = null;
    }

    public void StartMoveTo(Vector3 destination, float speedRatio = 1f)
    {
        navMeshAgent.SetDestination(destination);
        targetDirection = Vector3.Normalize(navMeshAgent.steeringTarget - transform.position);
        targetDirection.y = 0f;

        // Rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetDirection), 10f * Time.deltaTime);

        // Move
        characterController.SimpleMove(speedRatio * status.MoveSpeed * targetDirection);
    }

    public void LookTarget()
    {
        targetDirection = Vector3.Normalize(targetTransform.position - transform.position);
        transform.rotation
                = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetDirection), 5f * Time.deltaTime);
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
        for (int i = 0; i < skillArray.Length; ++i)
        {
            if (skillArray[i].IsReady(targetDistance))
            {
                currentSkill = skillArray[i];
                return true;
            }
        }
        currentSkill = null;
        return false;
    }

    public bool IsTargetInStopDistance()
    {
        if (targetTransform != null && targetDistance < status.StopDistance)
        {
            return true;
        }
        return false;
    }
    public bool IsTargetInDetectionDistance()
    {
        if (targetTransform != null && targetDistance < status.DetectionDistance)
        {
            return true;
        }
        return false;
    }

    public bool IsTargetInChaseDistance()
    {
        if (targetTransform != null && targetDistance < status.ChaseDistance)
        {
            return true;
        }
        return false;
    }

    public bool IsTargetInSight()
    {
        return true;
    }

    public void DamageProcess(BaseCharacter character, float ratio)
    {
        float damage = (status.AttackPower - character.Status.DefensivePower * 0.5f) * 0.5f;

        if (damage < 0)
            damage = 0;

        damage += ((status.AttackPower * 0.125f - status.AttackPower * 0.0625f) + 1f);
        damage *= ratio;

        character.Status.CurrentHP -= damage;
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

    public abstract void OnLightHit();
    public abstract void OnHeavyHit();
    public virtual void OnDie()
    {
        if (isDie)
            return;

        OnEnemyDie?.Invoke(this);
    }

    #region Property
    public EnemyData Status { get { return status; } }
    public Vector3 SpawnPosition { get { return spawnPosition; } }
    public EnemySkill CurrentSkill { get { return currentSkill; } set { currentSkill = value; } }
    public NavMeshAgent NavMeshAgent { get { return navMeshAgent; } }
    public StatusEffectController<BaseEnemy> StatusEffectControler { get { return statusEffectControler; } }

    public Transform TargetTransform { get { return targetTransform; } set { targetTransform = value; } }
    public Vector3 TargetDirection { get { return targetDirection; } }
    public float TargetDistance { get { return targetDistance; } }
    #endregion
}
