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
    public event UnityAction<BaseEnemy> OnEnemyHit;

    [Header("Base Enemy")]
    [SerializeField] protected EnemyData status;
    [SerializeField] protected Vector3 spawnPosition;

    [Header("Status Effect")]
    [SerializeField] protected StatusEffectController<BaseEnemy> statusEffectControler;

    [Header("Skill")]
    [SerializeField] protected EnemySkill[] skillArray;
    [SerializeField] protected EnemySkill currentSkill;

    protected PathFinder pathFinder;

    [Header("Target")]
    [SerializeField] protected Transform targetTransform;
    [SerializeField] protected Vector3 targetDirection;
    [SerializeField] protected float targetDistance;

    [Header("Move")]
    [SerializeField] protected float moveDistance;


    protected override void Awake()
    {
        base.Awake();

        status.OnChanageEnemyData -= OnDie;
        status.OnChanageEnemyData += OnDie;

        if (TryGetComponent(out pathFinder))
        {
            pathFinder.Initialize(status.StopDistance);
        }

        skillArray = GetComponents<EnemySkill>();

        for (int i = 0; i < skillArray.Length; ++i)
        {
            skillArray[i].Initialize(this);
        }

        state.StateDictionary.Add(ACTION_STATE.ENEMY_IDLE, new EnemyStateIdle(this));
        state.StateDictionary.Add(ACTION_STATE.ENEMY_DIE, new EnemyStateDie(this));
        state.SetState(ACTION_STATE.ENEMY_IDLE, STATE_SWITCH_BY.FORCED);
    }

    public virtual void OnDisable()
    {
        Despawn();
    }

    public virtual void Update()
    {
        UpdateTargetInformation();
        state.Update();
        moveController?.Update();
    }

    public virtual void Spawn()
    {
        OnEnemySpawn?.Invoke(this);

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

    public void MoveTo(Vector3 destination, float speedRatio = 1f)
    {
        moveDistance = Vector3.Distance(destination, transform.position);
        moveController.SetMoveInformation(pathFinder.FindPath(destination, NavMesh.AllAreas), status.MoveSpeed * speedRatio);
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
        if (isInvincible)
            return;

        DamageInformation damageInforamtion = TakeDamage(attacker, damageRatio);
        OnEnemyHit?.Invoke(this);

        if (Managers.SceneManagerCS.CurrentScene.RequestObject(Constants.PREFAB_FLOATING_DAMAGE_TEXT).TryGetComponent(out FloatingDamageText floatingDamageText))
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

    public abstract void OnLightHit();
    public abstract void OnHeavyHit();
    public virtual void OnDie()
    {
        if (!isDie)
        {
            OnEnemySpawn?.Invoke(this);

            gameObject.layer = Constants.LAYER_ENEMY;
            isInvincible = true;
            IsDie = true;
            state?.SetState(ACTION_STATE.ENEMY_DIE, STATE_SWITCH_BY.WEIGHT);
            StartCoroutine(CoWaitForDisapear(Constants.TIME_NORMAL_MONSTER_DISAPEAR));
        }
    }
    public virtual void OnDie(EnemyData enemyData)
    {
        if (enemyData.CurrentHP <= 0)
        {
            OnDie();
        }
    }

    #region Property
    public EnemyData Status { get { return status; } }
    public Vector3 SpawnPosition { get { return spawnPosition; } }
    public EnemySkill CurrentSkill { get { return currentSkill; } set { currentSkill = value; } }
    public StatusEffectController<BaseEnemy> StatusEffectControler { get { return statusEffectControler; } }

    public Transform TargetTransform { get { return targetTransform; } set { targetTransform = value; } }
    public Vector3 TargetDirection { get { return targetDirection; } }
    public float TargetDistance { get { return targetDistance; } }
    public float MoveDistance { get { return moveDistance; } }
    #endregion
}
