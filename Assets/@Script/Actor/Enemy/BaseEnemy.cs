using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using BehaviourTreePackage;

public abstract class BaseEnemy : BaseActor
{
    public event UnityAction<BaseEnemy> OnEnemyBirth;
    public event UnityAction<BaseEnemy> OnEnemyDie;

    [Header("Base Enemy")]
    [SerializeField] protected bool isSpawn;
    [SerializeField] protected EnemyData enemyData;
    protected EnemyStateController state;

    [Header("Skills")]
    [SerializeField] protected EnemySkill selectSkill;
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
        navMeshAgent.speed = enemyData.MoveSpeed;
    }

    public virtual void OnEnable()
    {
        OnBirth();
    }

    public virtual void OnDisable()
    {
        targetTransform = null;
    }

    public virtual void OnBirth()
    {
        OnEnemyBirth?.Invoke(this);
        isInvincible = true;
        IsDie = false;
        enemyData.CurrentHP = enemyData.MaxHP;
        gameObject.layer = Constants.LAYER_ENEMY;
    }
    public virtual void OnDie()
    {
        if (isDie)
            return;

        OnEnemyDie?.Invoke(this);
    }

    public virtual void BaseEnemyBehaviour()
    {
        if (targetTransform != null)
        {
            foreach (var skill in skillDictionary.Values)
            {
                if (skill.CheckCondition(targetDistance))
                {
                    selectSkill = skill;
                    break;
                }
            }
            state.TrySwitchState(ENEMY_STATE.Skill);

            if (targetDistance > enemyData.MinChaseRange)
                state.TrySwitchState(ENEMY_STATE.Move);
        }
    }

    public void LookTarget()
    {
        targetDirection = (targetTransform.position - transform.position).normalized;
        transform.rotation
                = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetDirection), 2f * Time.deltaTime);
    }

    public void DamageProcess(BaseCharacter character, float ratio)
    {
        float damage = (enemyData.AttackPower - character.StatusData.DefensivePower * 0.5f) * 0.5f;

        if (damage < 0)
            damage = 0;

        damage += ((enemyData.AttackPower / 8f - enemyData.AttackPower / 16f) + 1f);
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

    public void TrySwitchState(ENEMY_STATE targetState)
    {
        state.TrySwitchState(targetState);
    }
    public void SwitchState(ENEMY_STATE targetState)
    {
        state.SwitchState(targetState);
    }

    #region Property
    public EnemyData EnemyData { get { return enemyData; } }
    public EnemyStateController State { get { return state; } }
    public Dictionary<int, EnemySkill> SkillDictionary { get { return skillDictionary; } }
    public EnemySkill SelectSkill { get { return selectSkill; } set { selectSkill = value; } }
    public NavMeshAgent NavMeshAgent { get { return navMeshAgent; } }

    // Target
    public Transform TargetTransform { get { return targetTransform; } set { targetTransform = value; } }
    public Vector3 TargetDirection
    {
        get
        {
            if (targetTransform != null)
                targetDirection = (targetTransform.position - transform.position).normalized;

            return targetDirection;
        }
        set { targetDirection = value; }
    }
    public float TargetDistance
    {
        get
        {
            if (targetTransform != null)
                targetDistance = (targetTransform.position - transform.position).magnitude;

            return targetDistance;
        }
        set { targetDistance = value; }
    }
    #endregion
}
