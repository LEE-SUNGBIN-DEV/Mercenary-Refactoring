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

    public virtual void InitializeEnemy(int enemyID)
    {
        base.InitializeActor();

        status = new EnemyStatus(Managers.DataManager.EnemyTable[enemyID]);
        status.OnChangeEnemyData -= OnDie;
        status.OnChangeEnemyData += OnDie;
        status.StopDistance = Constants.ENEMY_MAX_STOP_DISTANCE;

        AddPreventPushObject();
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

    private void FixedUpdate()
    {
        moveController.UpdateGroundState();
        moveController.UpdatePosition();
    }

    public virtual void Update()
    {
        UpdateMoveInterval();
        UpdateTarget();
        state.Update();
    }

    public void AddPreventPushObject()
    {
        GameObject preventPushObject = new GameObject("Prevent Push Object");
        preventPushObject.transform.SetParent(transform);
        preventPushObject.layer = Constants.LAYER_PREVENT_PUSH;

        CapsuleCollider preventPushCollider = preventPushObject.AddComponent<CapsuleCollider>();
        preventPushCollider.height = capsuleCollider.height;
        preventPushCollider.radius = capsuleCollider.radius;
        preventPushCollider.center = capsuleCollider.center;

        Rigidbody preventPushRigidbody = preventPushObject.AddComponent<Rigidbody>();
        preventPushRigidbody.isKinematic = true;
        preventPushRigidbody.useGravity = false;
        preventPushRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        preventPushRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;

        Physics.IgnoreCollision(capsuleCollider, preventPushCollider);
    }

    #region Override Functions
    public virtual void Spawn(Vector3 spawnPosition)
    {
        materialController.SetDefaultMaterials();
        this.spawnPosition = spawnPosition;
        transform.position = spawnPosition;

        hitState = HIT_STATE.Hittable;
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
                hitBoxes[i].gameObject.layer = Constants.LAYER_DIE;

            targetTransform = null;
            gameObject.layer = Constants.LAYER_DIE;
            hitState = HIT_STATE.Invincible;
            IsDie = true;
            state?.SetState(ACTION_STATE.ENEMY_DIE, STATE_SWITCH_BY.WEIGHT);
            StartCoroutine(CoWaitForDisapear(Constants.TIME_NORMAL_MONSTER_DISAPEAR));

            Debug.Log("On Die");
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
        float reducedDefensivePower = status.DefensePower * (1 - attacker.Status.DefensePenetration);
        float damage = attacker.Status.AttackPower * (1 - (reducedDefensivePower / (100 + reducedDefensivePower)));

        // Critical Process
        bool isCritical;
        if (Random.Range(0.0f, 100.0f) <= attacker.Status.CriticalChance)
        {
            isCritical = true;
            damage *= (1 + attacker.Status.CriticalDamage * 0.01f);
        }
        else
            isCritical = false;

        // Damage Range Process
        damage *= damageRatio * Random.Range(0.9f, 1.1f);

        // Add Fixed Damage
        damage += attacker.Status.FixedDamage;

        status.CurrentHP -= damage;

        if (isDie)
            Managers.GameEventManager.EventQueue.Enqueue(new GameEventMessage(GAME_EVENT_TYPE.OnPlayerKillEnemy, this));

        return new DamageInformation(damage, isCritical);
    }

    public void TakeHit(PlayerCharacter attacker, float damageRatio, HIT_TYPE combatType, Vector3 hitPoint, float duration = 0f)
    {
        if (targetTransform == null)
            targetTransform = attacker.transform;

        if (hitState == HIT_STATE.Invincible)
            return;

        OnEnemyHit?.Invoke(this);

        DamageInformation damageInforamtion = TakeDamage(attacker, damageRatio);

        if (Managers.SceneManagerCS.CurrentScene.RequestObject(Constants.Prefab_Floating_Damage_Text).TryGetComponent(out FloatingDamageText floatingDamageText))
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
