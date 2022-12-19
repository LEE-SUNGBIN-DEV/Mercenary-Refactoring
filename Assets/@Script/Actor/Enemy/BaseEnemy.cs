using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public abstract class BaseEnemy : BaseActor
{
    public event UnityAction<BaseEnemy> OnBirth;
    public event UnityAction<BaseEnemy> OnDie;

    [Header("Base Enemy")]
    [SerializeField] protected EnemyData enemyData;
    [SerializeField] protected ENEMY_STATE state;

    [Header("Skills")]
    [SerializeField] protected int skillIndex;
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
        gameObject.layer = 8;
        Rebirth();
    }
    public virtual void OnDisable()
    {
        targetTransform = null;
    }

    public abstract void OnLightHit();
    public abstract void OnHeavyHit();
    public abstract void OnStun();
    public abstract void Die();
    public virtual void Rebirth()
    {
        state = ENEMY_STATE.Spawn;
        isInvincible = true;
        IsDie = false;
        enemyData.CurrentHP = enemyData.MaxHP;
    }
    #region Common Function
    public void DamageProcess(BaseCharacter character, float ratio)
    {
        // Damage Process
        float damage = (enemyData.AttackPower - character.StatusData.DefensivePower * 0.5f) * 0.5f;
        if (damage < 0)
        {
            damage = 0;
        }
        damage += ((enemyData.AttackPower / 8f - enemyData.AttackPower / 16f) + 1f);

        // Final Damage
        damage *= ratio;

        character.StatusData.CurrentHP -= damage;
    }
    public IEnumerator WaitForDisapear(float time)
    {
        OnDie(this);
        isInvincible = true;
        gameObject.layer = 10;

        float disapearTime = 0f;
        while(disapearTime <= time)
        {
            disapearTime += Time.deltaTime;

            yield return null;
        }
    }
    #endregion

    #region Animation Event Function
    private void OnEndState()
    {
        state = ENEMY_STATE.Idle;
    }
    #endregion

    #region Property
    public EnemyData EnemyData { get { return enemyData; } }
    public ENEMY_STATE State { get { return state; } set { state = value; } }
    public Dictionary<int, EnemySkill> SkillDictionary { get { return skillDictionary; } }
    public int SkillIndex { get { return skillIndex; } set { skillIndex = value; } }
    public NavMeshAgent NavMeshAgent { get { return navMeshAgent; } }

    // Target
    public Transform TargetTransform { get { return targetTransform; } set { targetTransform = value; } }
    public Vector3 TargetDirection { get { return targetDirection; } set { targetDirection = value; } }
    public float TargetDistance { get { return targetDistance; } set { targetDistance = value; } }

    // State
    public bool IsDie { get; set; }
    #endregion
}
