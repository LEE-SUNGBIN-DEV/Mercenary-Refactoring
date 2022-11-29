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
    [SerializeField] protected bool isAnimationDone;

    [Header("Component")]
    protected Rigidbody enemyRigidbody;
    protected NavMeshAgent navMeshAgent;
    protected Animator animator;
    [SerializeField] protected SkinnedMeshRenderer meshRenderer;

    [Header("Target")]
    [SerializeField] protected Transform targetTransform;

    [Header("Behaviour Tree")]
    [SerializeField] protected BlackDragonBehaviourTree behavourTree;
    
    #region Virtual Function
    public virtual void Awake()
    {
        enemyRigidbody = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        navMeshAgent.speed = enemyData.MoveSpeed;
    }
    public virtual void OnEnable()
    {
        gameObject.tag = Constants.TAG_INVINCIBILITY;
        gameObject.layer = 8;

        Rebirth();
        Spawn();
        OnBirth?.Invoke(this);
    }
    public virtual void OnDisable()
    {
        targetTransform = null;
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
    public void Spawn()
    {
        tag = Constants.TAG_INVINCIBILITY;

        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(targetTransform.position);
        animator.SetTrigger("doSpawn");
    }
    #endregion

    #region Animation Event Function
    public void OnAnimationDone()
    {
        isAnimationDone = true;
    }
    #endregion

    #region Property
    public EnemyData EnemyData { get { return enemyData; } }
    public Rigidbody EnemyRigidbody { get { return enemyRigidbody; } }
    public NavMeshAgent NavMeshAgent { get { return navMeshAgent; } }
    public Animator Animator { get { return animator; } }
    public SkinnedMeshRenderer MeshRenderer { get { return meshRenderer; } }
    public Transform TargetTransform { get { return targetTransform; } set { targetTransform = value; } }
    public bool IsAnimationDone { get { return isAnimationDone; } }

    // State
    public bool IsHit { get; set; }
    public bool IsHeavyHit { get; set; }
    public bool IsStun { get; set; }
    public bool IsDie { get; set; }
    #endregion
}
