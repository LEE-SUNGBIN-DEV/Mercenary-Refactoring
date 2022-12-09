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

public abstract class Enemy : MonoBehaviour
{
    public event UnityAction<Enemy> OnBirth;
    public event UnityAction<Enemy> OnDie;

    [Header("Enemy Data")]
    [SerializeField] protected EnemyData enemyData;
    [SerializeField] protected ENEMY_STATE state;
    [SerializeField] protected bool isInvincible;

    [Header("Skills")]
    protected Dictionary<int, EnemySkill> skillDictionary;
    [SerializeField] protected int skillIndex;

    [Header("Component")]
    protected Rigidbody enemyRigidbody;
    protected NavMeshAgent navMeshAgent;
    protected Animator animator;
    [SerializeField] protected SkinnedMeshRenderer meshRenderer;

    [Header("Object Pool")]
    [SerializeField] protected ObjectPoolController objectPoolController = new ObjectPoolController();

    [Header("Target")]
    [SerializeField] protected Transform targetTransform;
    [SerializeField] protected Vector3 targetDirection;
    [SerializeField] protected float targetDistance;
    
    public virtual void Awake()
    {
        TryGetComponent(out enemyRigidbody);
        TryGetComponent(out navMeshAgent);
        TryGetComponent(out animator);
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>(true);

        objectPoolController.Initialize(gameObject);

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

    public abstract void OnHit();
    public abstract void OnHeavyHit();
    public abstract void OnStun();
    public abstract void Die();
    public abstract void InitializeAllState();

    #region Common Function
    public void DamageProcess(Character character, float ratio)
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
    public bool IsInvincible { get { return isInvincible; } }
    public Dictionary<int, EnemySkill> SkillDictionary { get { return skillDictionary; } }
    public int SkillIndex { get { return skillIndex; } set { skillIndex = value; } }
    public Rigidbody EnemyRigidbody { get { return enemyRigidbody; } }
    public NavMeshAgent NavMeshAgent { get { return navMeshAgent; } }
    public Animator Animator { get { return animator; } }
    public SkinnedMeshRenderer MeshRenderer { get { return meshRenderer; } }
    public ObjectPoolController ObjectPoolController { get { return objectPoolController; } }

    // Target
    public Transform TargetTransform { get { return targetTransform; } set { targetTransform = value; } }
    public Vector3 TargetDirection { get { return targetDirection; } set { targetDirection = value; } }
    public float TargetDistance { get { return targetDistance; } set { targetDistance = value; } }

    // State
    public bool IsDie { get; set; }
    #endregion
}
