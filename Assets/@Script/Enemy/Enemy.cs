using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public abstract class Enemy : MonoBehaviour, IEnemy
{
    #region Event
    public event UnityAction<Enemy> OnBirth;
    public event UnityAction<Enemy> OnDie;
    public event UnityAction<Enemy> OnChangeCurrentHP;
    #endregion

    // Monster Data
    [SerializeField] private string enemyName;
    [SerializeField] private float attackPower;
    [SerializeField] private float defensivePower;
    [SerializeField] private float maxHitPoint;
    [SerializeField] private float currentHitPoint;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveStopDistance;
    [SerializeField] private float experienceAmount;
    [SerializeField] private Vector3 rotationOffset;

    // Component
    private Rigidbody enemyRigidbody;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    [SerializeField] private SkinnedMeshRenderer meshRenderer;

    private Transform target;
    private float distanceFromTarget;
    private Vector3 targetDirection;
    
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
        NavMeshAgent.enabled = false;
        if (Target != null)
        {
            Target = null;
        }
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
    public void Move()
    {
        distanceFromTarget = (Target.position - transform.position).magnitude;

        if (DistanceFromTarget <= TraceRange)
            StopTrace();

        else
        {
            LookTarget(RotationOffset);
            StartTrace();
        }
        Animator.SetBool("isMove", IsMove);
    }
    public void LookTarget()
    {
        targetDirection = (Target.position - transform.position).normalized;
        transform.rotation
                = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetDirection), 5f * Time.deltaTime);
    }
    public void LookTarget(Vector3 rotationOffset)
    {
        targetDirection = (Target.position - transform.position).normalized;
        transform.rotation
                = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetDirection), 5f * Time.deltaTime);
    }
    public void StartTrace()
    {
        NavMeshAgent.isStopped = false;
        NavMeshAgent.SetDestination(Target.position);
        IsMove = true;
    }
    public void StopTrace()
    {
        NavMeshAgent.isStopped = true;
        IsMove = false;
    }
    public void ReturnObject()
    {
        if (Managers.ObjectPoolManager.ObjectPoolDictionary.ContainsKey(enemyName))
        {
            Managers.ObjectPoolManager.ReturnObject(enemyName, gameObject);
        }
    }
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

        ReturnObject();
    }
    public void Rebirth()
    {
        IsDie = false;
        CurrentHitPoint = MaxHitPoint;
        InitializeAllState();
    }
    public void InitializeTarget(CharacterStatus characterStats)
    {
        Target = null;
    }
    public void FreezeVelocity()
    {
        MonsterRigidbody.velocity = Vector3.zero;
        MonsterRigidbody.angularVelocity = Vector3.zero;
    }
    #endregion

    #region Animation Event Function
    public void InSpawn()
    {
        IsSpawn = true;
    }
    public void InSkill()
    {
        IsMove = false;
        IsAttack = true;
    }

    public void OutSpawn()
    {
        IsMove = false;
        IsAttack = false;
        IsHit = false;
        IsHeavyHit = false;
        IsStun = false;
        IsSpawn = false;

        gameObject.tag = Constants.TAG_ENEMY;

        NavMeshAgent.enabled = true;
        NavMeshAgent.speed = MoveSpeed;
    }

    public void OutSkill()
    {
        IsMove = false;
        IsAttack = false;
    }
    public void OutHit()
    {
        IsMove = false;
        IsAttack = false;
        IsHit = false;
    }
    public void OutHeavyHit()
    {
        IsMove = false;
        IsAttack = false;
        IsHit = false;
        IsHeavyHit = false;
    }
    public void OutStun()
    {
        IsMove = false;
        IsAttack = false;
        IsHit = false;
        IsHeavyHit = false;
        IsStun = false;
    }
    #endregion

    #region Property
    public Rigidbody MonsterRigidbody { get { return enemyRigidbody; } }
    public NavMeshAgent NavMeshAgent { get { return navMeshAgent; } }
    public Animator Animator { get { return animator; } }
    public SkinnedMeshRenderer MeshRenderer { get { return meshRenderer; } }
    public Transform Target
    {
        get { return target; }
        set { target = value; }
    }
    public float DistanceFromTarget { get { return distanceFromTarget; } }
    public Vector3 RotationOffset { get { return rotationOffset; } }
    public string EnemyName { get { return enemyName; } }
    public float AttackPower
    {
        get { return attackPower; }
        set
        {
            attackPower = value;

            if (attackPower < 0)
            {
                attackPower = 0;
            }
        }
    }
    public float DefensivePower
    {
        get { return defensivePower; }
        set
        {
            defensivePower = value;

            if (defensivePower < 0)
            {
                defensivePower = 0;
            }
        }
    }
    public float MaxHitPoint
    {
        get { return maxHitPoint; }
        set
        {
            maxHitPoint = value;
            if (maxHitPoint <= 0)
            {
                maxHitPoint = 1;
            }
        }
    }
    public float CurrentHitPoint
    {
        get { return currentHitPoint; }
        set
        {
            currentHitPoint = value;
            if (currentHitPoint > MaxHitPoint)
            {
                currentHitPoint = MaxHitPoint;
            }

            if (currentHitPoint < 0)
            {
                currentHitPoint = 0;
                if (IsDie == false)
                {
                    gameObject.layer = 10;
                    Die();
                }
            }
            OnChangeCurrentHP?.Invoke(this);
        }
    }

    public float MoveSpeed
    {
        get { return moveSpeed; }
        set
        {
            moveSpeed = value;

            if (moveSpeed < 0)
            {
                moveSpeed = 0;
            }
        }
    }

    public float TraceRange
    {
        get { return moveStopDistance; }
        set
        {
            moveStopDistance = value;
            if (moveStopDistance < 0)
            {
                moveStopDistance = 0;
            }
        }
    }

    public float ExperienceAmount
    {
        get { return experienceAmount; }
        set
        {
            experienceAmount = value;
            if (experienceAmount < 0)
            {
                experienceAmount = 0;
            }
        }
    }

    // State
    public bool IsSpawn { get; set; }
    public bool IsMove { get; set; }
    public bool IsAttack { get; set; }
    public bool IsHit { get; set; }
    public bool IsHeavyHit { get; set; }
    public bool IsStun { get; set; }
    public bool IsDie { get; set; }
    #endregion
}
