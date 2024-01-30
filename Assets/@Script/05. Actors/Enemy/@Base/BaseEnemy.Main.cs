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
    [SerializeField] protected EnemyStatus status;
    [SerializeField] protected Vector3 spawnPosition;
    protected EnemyHitbox[] hitBoxes;

    [Header("Controllers")]
    [SerializeField] protected StatusEffectController<BaseEnemy> statusEffectControler;

    #region Private
    protected override void Awake()
    {
        base.Awake();

        gameObject.name = gameObject.name.Replace("(Clone)", "").Trim();
        status = new EnemyStatus(Managers.DataManager.EnemyTable[gameObject.name]);
        status.OnChangeEnemyData -= OnDie;
        status.OnChangeEnemyData += OnDie;
        status.StopDistance = Constants.ENEMY_MAX_STOP_DISTANCE;

        InitializeMovement();
        InitializeSkill();

        // Initialize Hitbox
        hitBoxes = GetComponentsInChildren<EnemyHitbox>(true);
        for (int i = 0; i < hitBoxes.Length; ++i)
            hitBoxes[i].Initialize(this);

        // Initialize State
        state.StateDictionary.Add(ACTION_STATE.ENEMY_IDLE, new EnemyStateIdle(this));
        state.StateDictionary.Add(ACTION_STATE.ENEMY_DIE, new EnemyStateDie(this));
        state.SetState(ACTION_STATE.ENEMY_IDLE, STATE_SWITCH_BY.FORCED);
    }

    public virtual void Update()
    {
        UpdateMoveInterval();
        UpdateTarget();
        state.Update();
    }
    #endregion

    #region Override Functions
    public virtual void Spawn(Vector3 spawnPosition)
    {
        materialController.SetDefaultMaterials();
        this.spawnPosition = spawnPosition;
        transform.position = spawnPosition;

        hitState = HIT_STATE.HITTABLE;
        IsDie = false;
        status.CurrentHP = status.MaxHP;
        gameObject.layer = Constants.LAYER_ENEMY;

        for (int i = 0; i < hitBoxes.Length; ++i)
            hitBoxes[i].gameObject.layer = Constants.LAYER_HITBOX;

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
                hitBoxes[i].gameObject.layer = Constants.LAYER_NONE;

            if(targetTransform != null && targetTransform.TryGetComponent(out PlayerCharacter targetCharacter))
            {
                status.DropReward(targetCharacter.CharacterData);
            }
            targetTransform = null;
            gameObject.layer = Constants.LAYER_DIE;
            hitState = HIT_STATE.INVINCIBLE;
            IsDie = true;
            state?.SetState(ACTION_STATE.ENEMY_DIE, STATE_SWITCH_BY.WEIGHT);
            float disapearTime = Constants.TIME_NORMAL_ENEMY_DISAPEAR;
            switch (status.EnemyType)
            {
                case ENEMY_TYPE.Normal:
                    disapearTime = Constants.TIME_NORMAL_ENEMY_DISAPEAR;
                    break;
                case ENEMY_TYPE.Elite:
                    disapearTime = Constants.TIME_NAMED_ENEMY_DISAPEAR;
                    break;
                case ENEMY_TYPE.Boss:
                    disapearTime = Constants.TIME_BOSS_ENEMY_DISAPEAR;
                    break;
            }

            StartCoroutine(CoWaitForDisapear(disapearTime));

            OnEnemyDie?.Invoke(this);
        }
    }
    public virtual void OnDie(EnemyStatus enemyData)
    {
        if (enemyData.CurrentHP <= 0)
            OnDie();
    }
    #endregion

    public DamageInformation TakeDamage(PlayerCharacter attacker, float damageRatio)
    {
        // Basic Damage Process
        float reducedDefensivePower = status.DefensePower * (1 - attacker.StatusData.StatDict[STAT_TYPE.STAT_DEFENSE_PENETRATION_RATE].GetFinalValue());
        float damage = attacker.StatusData.StatDict[STAT_TYPE.STAT_ATTACK_POWER].GetFinalValue() * (1 - (reducedDefensivePower / (100 + reducedDefensivePower)));

        if (damage < 0)
            damage = 0;

        // Critical Process
        bool isCritical;
        if (Random.Range(0.0f, 100.0f) < attacker.StatusData.StatDict[STAT_TYPE.STAT_CRITICAL_CHANCE_RATE].GetFinalValue())
        {
            isCritical = true;
            damage *= (1 + attacker.StatusData.StatDict[STAT_TYPE.STAT_CRITICAL_DAMAGE_RATE].GetFinalValue() * 0.01f);
        }
        else
            isCritical = false;

        // Damage Random Range
        damage *= (damageRatio * Random.Range(0.9f, 1.1f));

        // Damage Reduction
        damage *= (1 - (status.DamageReduction * 0.01f));

        // Add Fixed Damage
        damage += attacker.StatusData.StatDict[STAT_TYPE.STAT_FIXED_DAMAGE].GetFinalValue();

        status.CurrentHP -= damage;

        if (isDie)
            Managers.GameManager.SendEventMessage(new GameEventMessage(GAME_EVENT_TYPE.PLAYER_KILL_ENEMY, this));

        return new DamageInformation(damage, isCritical);
    }

    public void TakeHit(PlayerCharacter attacker, float damageRatio, HIT_TYPE combatType, Vector3 hitPoint, float duration = 0f)
    {
        if (targetTransform == null)
            targetTransform = attacker.transform;

        if (hitState == HIT_STATE.INVINCIBLE)
            return;

        OnEnemyHit?.Invoke(this);

        DamageInformation damageInforamtion = TakeDamage(attacker, damageRatio);

        if (Managers.SceneManagerEX.CurrentScene.RequestObject(Constants.PREFAB_FLOATING_DAMAGE_TEXT).TryGetComponent(out FloatingDamageText floatingDamageText))
            floatingDamageText.SetDamageText(damageInforamtion.isCritical, damageInforamtion.damage, hitPoint);

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
    public EnemyStatus Status { get { return status; } }
    public Vector3 SpawnPosition { get { return spawnPosition; } }
    public StatusEffectController<BaseEnemy> StatusEffectControler { get { return statusEffectControler; } }
    #endregion
}
