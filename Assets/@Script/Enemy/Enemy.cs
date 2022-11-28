using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public abstract class Enemy : MonoBehaviour, IEnemy
{
    public event UnityAction<Enemy> OnBirth;
    public event UnityAction<Enemy> OnDie;

    [Header("Enemy Data")]
    [SerializeField] protected EnemyData enemyData;
    [SerializeField] protected float traceRange;

    [Header("Component")]
    protected Rigidbody enemyRigidbody;
    protected NavMeshAgent navMeshAgent;
    protected Animator animator;
    [SerializeField] protected SkinnedMeshRenderer meshRenderer;

    [Header("Target")]
    [SerializeField] protected Transform targetTransform;

    [Header("Behaviour Tree")]
    [SerializeField] protected EnemyBehaviourTree behavourTree;
    
    #region Virtual Function
    public virtual void Awake()
    {
        enemyRigidbody = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    public virtual void OnEnable()
    {
        gameObject.tag = Constants.TAG_INVINCIBILITY;
        gameObject.layer = 8;

        OnBirth?.Invoke(this);
        Rebirth();
        Spawn();
    }
    public virtual void OnDisable()
    {
        targetTransform = null;
    }
    public virtual void Spawn()
    {
        IsSpawn = true;
        Animator.SetTrigger("doSpawn");
    }
    #endregion

    #region Abstract Function
    public abstract void Attack();
    public abstract void Hit();
    public abstract void HeavyHit();
    public abstract void Stun();
    public abstract void Die();
    public abstract void InitializeAllState();
    #endregion

    #region Common Function
    public IEnumerator WaitForDisapear(float time)
    {
        OnDie(this);
        gameObject.tag = Constants.TAG_INVINCIBILITY;

        float disapearTime = 0f;
        while(disapearTime <= time)
        {
            disapearTime += Time.deltaTime;

            yield return null;
        }
        gameObject.layer = 10;
    }
    public void Rebirth()
    {
        IsDie = false;
        enemyData.CurrentHP = enemyData.MaxHP;
        InitializeAllState();
    }
    public void FreezeVelocity()
    {
        EnemyRigidbody.velocity = Vector3.zero;
        EnemyRigidbody.angularVelocity = Vector3.zero;
    }
    #endregion

    #region Animation Event Function
    public void InSpawn()
    {
        IsSpawn = true;
    }
    public void InSkill()
    {
        IsAttack = true;
    }

    public void OutSpawn()
    {
        IsAttack = false;
        IsHit = false;
        IsHeavyHit = false;
        IsStun = false;
        IsSpawn = false;

        gameObject.tag = Constants.TAG_ENEMY;

        NavMeshAgent.enabled = true;
        NavMeshAgent.speed = enemyData.MoveSpeed;
    }

    public void OutSkill()
    {
        IsAttack = false;
    }
    public void OutHit()
    {
        IsAttack = false;
        IsHit = false;
    }
    public void OutHeavyHit()
    {
        IsAttack = false;
        IsHit = false;
        IsHeavyHit = false;
    }
    public void OutStun()
    {
        IsAttack = false;
        IsHit = false;
        IsHeavyHit = false;
        IsStun = false;
    }
    #endregion

    #region Property
    public EnemyData EnemyData { get { return enemyData; } }
    public float TraceRange
    {
        get { return traceRange; }
        set
        {
            traceRange = value;
            if (traceRange < 0)
            {
                traceRange = 0;
            }
        }
    }
    public Rigidbody EnemyRigidbody { get { return enemyRigidbody; } }
    public NavMeshAgent NavMeshAgent { get { return navMeshAgent; } }
    public Animator Animator { get { return animator; } }
    public SkinnedMeshRenderer MeshRenderer { get { return meshRenderer; } }
    public Transform TargetTransform { get { return targetTransform; } set { targetTransform = value; } }
    
    // State
    public bool IsSpawn { get; set; }
    public bool IsAttack { get; set; }
    public bool IsHit { get; set; }
    public bool IsHeavyHit { get; set; }
    public bool IsStun { get; set; }
    public bool IsDie { get; set; }
    #endregion
}
