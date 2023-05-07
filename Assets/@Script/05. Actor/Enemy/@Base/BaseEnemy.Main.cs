using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public abstract partial class BaseEnemy : BaseActor
{
    public event UnityAction<BaseEnemy> OnEnemySpawn;
    public event UnityAction<BaseEnemy> OnEnemyDie;
    public event UnityAction<BaseEnemy> OnEnemyHit;

    [Header("Base Enemy")]
    [SerializeField] protected EnemyData status;
    protected EnemyHitbox[] hitBoxes;
    [SerializeField] protected Vector3 spawnPosition;

    [Header("Controllers")]
    [SerializeField] protected StatusEffectController<BaseEnemy> statusEffectControler;

    [Header("Skills")]
    [SerializeField] protected EnemySkill[] skillArray;
    [SerializeField] protected EnemySkill currentSkill;

    [Header("Move")]
    [SerializeField] protected EnemyMoveController moveController;
    [SerializeField] protected Vector3 moveDirection;
    [SerializeField] protected float moveDistance;
    protected NavMeshPath path;

    protected override void Awake()
    {
        base.Awake();
        status.OnChanageEnemyData -= OnDie;
        status.OnChanageEnemyData += OnDie;

        moveController = new EnemyMoveController(this);

        // Initialize Hitbox
        hitBoxes = GetComponentsInChildren<EnemyHitbox>(true);
        for(int i=0; i<hitBoxes.Length; ++i)
            hitBoxes[i].Initialize(this);

        // Initialize Skill
        skillArray = GetComponents<EnemySkill>();
        for (int i = 0; i < skillArray.Length; ++i)
            skillArray[i].Initialize(this);

        // Initialize State
        state.StateDictionary.Add(ACTION_STATE.ENEMY_IDLE, new EnemyStateIdle(this));
        state.StateDictionary.Add(ACTION_STATE.ENEMY_DIE, new EnemyStateDie(this));
        state.SetState(ACTION_STATE.ENEMY_IDLE, STATE_SWITCH_BY.FORCED);

        // Nav Mesh
        path = new();
    }

    public virtual void Update()
    {
        UpdateTarget();
        moveController?.UpdateGroundState();
        state.Update();
    }

    public virtual void LateUpdate()
    {
        moveController?.UpdatePosition();
    }

    public virtual void Spawn(Vector3 spawnPosition)
    {
        this.spawnPosition = spawnPosition;

        characterController.enabled = false;
        transform.position = spawnPosition;
        characterController.enabled = true;

        hitState = HIT_STATE.Hittable;
        IsDie = false;
        status.CurrentHP = status.MaxHP;
        gameObject.layer = Constants.LAYER_ENEMY;

        for (int i = 0; i < hitBoxes.Length; ++i)
            hitBoxes[i].gameObject.layer = Constants.LAYER_DEFAULT;

        UpdateTarget();
        state?.SetState(ACTION_STATE.ENEMY_SPAWN, STATE_SWITCH_BY.FORCED);

        OnEnemySpawn?.Invoke(this);
    }

    public virtual void Despawn()
    {
    }

    public abstract void OnLightHit();
    public abstract void OnHeavyHit();
    public virtual void OnDie()
    {
        if (!isDie)
        {
            for (int i = 0; i < hitBoxes.Length; ++i)
                hitBoxes[i].gameObject.layer = Constants.LAYER_DIE;

            targetTransform = null;
            gameObject.layer = Constants.LAYER_DIE;
            hitState = HIT_STATE.Invincible;
            IsDie = true;
            state?.SetState(ACTION_STATE.ENEMY_DIE, STATE_SWITCH_BY.WEIGHT);
            StartCoroutine(CoWaitForDisapear(Constants.TIME_NORMAL_MONSTER_DISAPEAR));

            OnEnemyDie?.Invoke(this);
        }
    }
    public virtual void OnDie(EnemyData enemyData)
    {
        if (enemyData.CurrentHP <= 0)
            OnDie();
    }

    public void MoveTo(Vector3 destination, float speedRatio = 1f)
    {
        NavMesh.SamplePosition(transform.position, out NavMeshHit startHit, characterController.radius, NavMesh.AllAreas);
        NavMesh.SamplePosition(destination, out NavMeshHit endHit, status.StopDistance, NavMesh.AllAreas);
        if (NavMesh.CalculatePath(startHit.position, endHit.position, NavMesh.AllAreas, path))
        {
            moveDistance = Vector3.Distance(destination, transform.position);
            for (int i = 0; i < path.corners.Length; ++i)
            {
                if ((path.corners[i] - transform.position).magnitude > status.StopDistance)
                {
                    moveDirection = path.corners[i] - transform.position;
                    break;
                }
            }
#if UNITY_EDITOR
            for (int i = 0; i < path.corners.Length - 1; i++)
                Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
#endif
        }
        else
        {
            moveDistance = 0f;
            moveDirection = Vector3.zero;
        }

        moveController.SetMovement(moveDirection, status.MoveSpeed * speedRatio);
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

    public DamageInformation TakeDamage(PlayerCharacter attacker, float damageRatio)
    {
        // Basic Damage Process
        float damage = (attacker.Status.AttackPower - status.DefensivePower * 0.5f) * 0.5f;

        if (damage < 0)
            damage = 0;

        damage += ((attacker.Status.AttackPower / 8f - attacker.Status.AttackPower / 16f) + 1f);

        // Critical Process
        bool isCritical;
        if (Random.Range(0.0f, 100.0f) <= attacker.Status.CriticalChance)
        {
            isCritical = true;
            damage *= (1 + attacker.Status.CriticalDamage * 0.01f);
        }
        else
        {
            isCritical = false;
        }

        // Damage Ratio Process
        damage *= damageRatio * Random.Range(0.9f, 1.1f);

        status.CurrentHP -= damage;

        if (isDie)
            Managers.GameEventManager.EventQueue.Enqueue(new GameEventMessage(GAME_EVENT_TYPE.OnPlayerKillEnemy, this));

        return new DamageInformation(damage, isCritical);
    }

    public void TakeHit(PlayerCharacter attacker, float damageRatio, HIT_TYPE combatType, Vector3 hitPoint, float duration = 0f)
    {
        if (hitState == HIT_STATE.Invincible)
            return;

        DamageInformation damageInforamtion = TakeDamage(attacker, damageRatio);
        OnEnemyHit?.Invoke(this);

        if (Managers.SceneManagerCS.CurrentScene.RequestObject(Constants.Prefab_Floating_Damage_Text).TryGetComponent(out FloatingDamageText floatingDamageText))
            floatingDamageText.SetDamageText(damageInforamtion.isCritical, damageInforamtion.damage, hitPoint);

        if (Status.HitLevel > (int)combatType)
            return;

        switch (combatType)
        {
            case HIT_TYPE.LIGHT:
                OnLightHit();
                break;

            case HIT_TYPE.HEAVY:
                OnHeavyHit();
                break;

            case HIT_TYPE.STUN:
                if (this is IStunable stunable)
                    stunable.OnStun(duration);
                else
                    OnHeavyHit();
                break;
        }
        
        return;
    }

    #region Property
    public EnemyData Status { get { return status; } }
    public Vector3 SpawnPosition { get { return spawnPosition; } }
    public EnemySkill CurrentSkill { get { return currentSkill; } set { currentSkill = value; } }
    public StatusEffectController<BaseEnemy> StatusEffectControler { get { return statusEffectControler; } }

    public EnemyMoveController MoveController { get { return moveController; } }
    public float MoveDistance { get { return moveDistance; } }
    #endregion
}
