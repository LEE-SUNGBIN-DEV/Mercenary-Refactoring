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
    protected EnemyFSM state;
    protected StatusEffectController<BaseEnemy> statusEffectControler;

    [Header("Skill")]
    protected EnemySkill[] skillArray;
    [SerializeField] protected EnemySkill currentSkill;

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
        state = new EnemyFSM(this);
        skillArray = GetComponents<EnemySkill>();

        for (int i = 0; i < skillArray.Length; ++i)
        {
            skillArray[i].Initialize(this);
        }
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

        navMeshAgent.speed = status.MoveSpeed;
        navMeshAgent.stoppingDistance = status.StopDistance;
        spawnPosition = transform.position;

        isInvincible = false;
        IsDie = false;
        status.CurrentHP = status.MaxHP;
        gameObject.layer = Constants.LAYER_ENEMY;

        UpdateTargetInformation();
        state?.SetState(ACTION_STATE.ENEMY_SPAWN);
    }

    public virtual void Despawn()
    {
        targetTransform = null;
    }

    public abstract void OnLightHit();
    public abstract void OnHeavyHit();
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
        for(int i=0; i<skillArray.Length; ++i)
        {
            if (skillArray[i].IsReady(targetDistance))
            {
                currentSkill = skillArray[i];
                return true;
            }
        }
        return false;
    }

    public bool IsTargetInChaseDistance()
    {
        if (targetTransform != null
            && targetDistance <= status.ChaseDistance)
        {
            return true;
        }
        return false;
    }
    public bool IsTargetInStopDistance()
    {
        if (targetTransform != null
            && targetDistance <= status.StopDistance)
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

    #region Property
    public EnemyData Status { get { return status; } }
    public Vector3 SpawnPosition { get { return spawnPosition; } }
    public EnemySkill CurrentSkill { get { return currentSkill; } set { currentSkill = value; } }
    public NavMeshAgent NavMeshAgent { get { return navMeshAgent; } }
    public EnemyFSM State { get { return state; } }
    public StatusEffectController<BaseEnemy> StatusEffectControler { get { return statusEffectControler; } }

    // Target
    public Transform TargetTransform { get { return targetTransform; } set { targetTransform = value; } }
    public Vector3 TargetDirection { get { return targetDirection; } }
    public float TargetDistance { get { return targetDistance; } }
    #endregion
}
