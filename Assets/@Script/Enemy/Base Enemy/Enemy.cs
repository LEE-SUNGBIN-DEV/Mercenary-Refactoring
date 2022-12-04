using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public enum ENEMY_STATE
{
    Idle,
    Attack,
    Hit,
    HeavyHit,
    Stun,
    Compete,
    Spawn,
    Die
}

public abstract class Enemy : MonoBehaviour, IEnemy
{
    public event UnityAction<Enemy> OnBirth;
    public event UnityAction<Enemy> OnDie;

    [Header("Enemy Data")]
    [SerializeField] protected EnemyData enemyData;
    [SerializeField] protected ENEMY_STATE state;

    [Header("Skills")]
    protected Dictionary<int, EnemySkill> skillDictionary;
    [SerializeField] protected int skillIndex;

    [Header("Component")]
    protected Rigidbody enemyRigidbody;
    protected NavMeshAgent navMeshAgent;
    protected Animator animator;
    [SerializeField] protected SkinnedMeshRenderer meshRenderer;

    [Header("Target")]
    [SerializeField] protected Transform targetTransform;
    [SerializeField] protected Vector3 targetDirection;
    [SerializeField] protected float targetDistance;
    
    public virtual void Awake()
    {
        enemyRigidbody = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        navMeshAgent.speed = enemyData.MoveSpeed;
        IsSpawn = false;
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

    public abstract void Hit();
    public abstract void HeavyHit();
    public abstract void Stun();
    public abstract void Die();
    public abstract void InitializeAllState();

    #region Common Function
    public IEnumerator WaitForDisapear(float time)
    {
        OnDie(this);
        gameObject.tag = Constants.TAG_INVINCIBILITY;
        gameObject.layer = 10;

        float disapearTime = 0f;
        while(disapearTime <= time)
        {
            disapearTime += Time.deltaTime;

            yield return null;
        }
    }
    public void Rebirth()
    {
        state = ENEMY_STATE.Spawn;
        IsDie = false;
        enemyData.CurrentHP = enemyData.MaxHP;
        InitializeAllState();
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
    public Rigidbody EnemyRigidbody { get { return enemyRigidbody; } }
    public NavMeshAgent NavMeshAgent { get { return navMeshAgent; } }
    public Animator Animator { get { return animator; } }
    public SkinnedMeshRenderer MeshRenderer { get { return meshRenderer; } }

    // Target
    public Transform TargetTransform { get { return targetTransform; } set { targetTransform = value; } }
    public Vector3 TargetDirection { get { return targetDirection; } set { targetDirection = value; } }
    public float TargetDistance { get { return targetDistance; } set { targetDistance = value; } }

    // State
    public bool IsSpawn { get; set; }
    public bool IsHit { get; set; }
    public bool IsHeavyHit { get; set; }
    public bool IsStun { get; set; }
    public bool IsDie { get; set; }
    #endregion
}
