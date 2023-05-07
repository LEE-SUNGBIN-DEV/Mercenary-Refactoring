using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCharacter : BaseActor, ICompetable
{
    public event UnityAction<PlayerCharacter> OnPlayerSpawn;
    public event UnityAction<PlayerCharacter> OnPlayerDie;
    public event UnityAction<PlayerCharacter> OnPlayerHit;

    private PlayerCamera playerCamera;

    [Header("Player Character")]
    [SerializeField] private CharacterData characterData;

    [Header("Controllers")]
    [SerializeField] private PlayerMoveController moveController;
    [SerializeField] private PlayerWeaponController weaponController;
    [SerializeField] private PlayerInteractionController interactionController;
    private StatusEffectController<PlayerCharacter> statusEffectController;

    private bool isLockOn = false;

    protected override void Awake()
    {
        base.Awake();
        characterData = Managers.DataManager.CurrentCharacterData;

        OnPlayerHit += interactionController.DisableInteraction;
        OnPlayerDie += interactionController.DisableInteraction;

        Status.OnCharacterStatusChanged += SetAttackSpeed;
        Status.OnCharacterStatusChanged += OnDie;

        SetAttackSpeed(Status);

        moveController = new PlayerMoveController(this);

        // Initialize InteractionController
        interactionController = new PlayerInteractionController();
        interactionController.Initialize();
        interactionController.OnInteraction += Interaction;

        // Initialize Weapon
        weaponController = new PlayerWeaponController();
        weaponController.Initialize(this);

        // Initialize State
        AddDefaultState();
        if(TryEquipWeapon(CurrentWeapon.WeaponType))
            state.SetState(CurrentWeapon.IdleState, STATE_SWITCH_BY.FORCED);
    }
    
    private void OnEnable()
    {
        Spawn();
    }

    private void Update()
    {
        moveController?.UpdateGroundState();
        state?.Update();
    }

    public virtual void LateUpdate()
    {
        moveController?.UpdatePosition();
    }

    private void OnDestroy()
    {
        OnPlayerHit -= interactionController.DisableInteraction;
        OnPlayerDie -= interactionController.DisableInteraction;

        Status.OnCharacterStatusChanged -= SetAttackSpeed;
        Status.OnCharacterStatusChanged -= OnDie;

        interactionController.OnInteraction -= Interaction;
    }

    public void AddDefaultState()
    {
        state.StateDictionary.Add(ACTION_STATE.COMMON_UPPER_EMPTY, new CommonStateUpperEmpty(this));

        state.StateDictionary.Add(ACTION_STATE.PLAYER_SLIDE, new PlayerStateSlide(this));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_FALL, new PlayerStateFall(this));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_LANDING, new PlayerStateLanding(this));

        state.StateDictionary.Add(ACTION_STATE.PLAYER_RESONANCE_IN, new PlayerStateResonanceIn(this));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_RESONANCE_LOOP, new PlayerStateResonanceLoop(this));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_RESONANCE_OUT, new PlayerStateResonanceOut(this));

        state.StateDictionary.Add(ACTION_STATE.PLAYER_ROLL, new PlayerStateRoll(this));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_HIT_LIGHT, new PlayerStateLightHit(this));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_HIT_HEAVY, new PlayerStateHeavyHit(this));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_HIT_HEAVY_LOOP, new PlayerStateHeavyHitLoop(this));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_STAND_UP, new PlayerStateStandUp(this));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_STAND_ROLL, new PlayerStateStandRoll(this));

        state.StateDictionary.Add(ACTION_STATE.PLAYER_STUN, new PlayerStateStun(this));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_DIE, new PlayerStateDie(this));

        state.SetSubState(ACTION_STATE.COMMON_UPPER_EMPTY, STATE_SWITCH_BY.FORCED);
    }

    public void Spawn()
    {
        OnPlayerSpawn?.Invoke(this);

        gameObject.layer = Constants.LAYER_PLAYER;
        hitState = HIT_STATE.Hittable;
        IsDie = false;
        state?.SetState(CurrentWeapon.IdleState, STATE_SWITCH_BY.FORCED);
    }

    public void Interaction(bool isInteracting)
    {
        playerCamera.SetInteraction(isInteracting);

        switch (isInteracting)
        {
            case true:
                weaponController.HideWeapon(); 
                break;

            case false:
                weaponController.ShowWeapon();
                break;
        }
    }

    public bool TryEquipWeapon(WEAPON_TYPE targetWeapon)
    {
        switch (targetWeapon)
        {
            case WEAPON_TYPE.HALBERD:
                if (state.SetSubState(ACTION_STATE.PLAYER_HALBERD_EQUIP, STATE_SWITCH_BY.WEIGHT))
                {
                    weaponController.SwitchWeapon(targetWeapon);
                    return true;
                }
                break;

            case WEAPON_TYPE.SWORD_SHIELD:
                if (state.SetSubState(ACTION_STATE.PLAYER_SWORD_SHIELD_EQUIP, STATE_SWITCH_BY.WEIGHT))
                {
                    weaponController.SwitchWeapon(targetWeapon);
                    return true;
                }
                break;
        }
        return false;
    }

    public DamageInformation TakeDamage(BaseEnemy attacker, float damageRatio)
    {
        float damage = (attacker.Status.AttackPower - Status.DefensivePower * 0.5f) * 0.5f;

        if (damage < 0)
            damage = 0;

        damage += ((attacker.Status.AttackPower * 0.125f - Status.AttackPower * 0.0625f) + 1f);
        damage *= damageRatio;

        Status.CurrentHP -= damage;

        return new DamageInformation(damage, false);
    }

    public void TakeHit(BaseEnemy attacker, float damageRatio, HIT_TYPE combatType, float duration = 0f)
    {
        if (hitState == HIT_STATE.Invincible)
            return;

        DamageInformation damageInforamtion = TakeDamage(attacker, damageRatio);
        OnPlayerHit?.Invoke(this);

        if (Status.HitLevel > (int)combatType)
            return;

        switch(combatType)
        {
            case HIT_TYPE.LIGHT:
                state?.SetState(ACTION_STATE.PLAYER_HIT_LIGHT, STATE_SWITCH_BY.WEIGHT);
                break;

            case HIT_TYPE.HEAVY:
                state?.SetState(ACTION_STATE.PLAYER_HIT_HEAVY, STATE_SWITCH_BY.WEIGHT);
                break;

            case HIT_TYPE.STUN:
                state?.SetState(ACTION_STATE.PLAYER_STUN, STATE_SWITCH_BY.WEIGHT, duration);
                break;
        }

        return ;
    }

    public virtual void OnCompete() { state?.SetState(ACTION_STATE.PLAYER_COMPETE, STATE_SWITCH_BY.WEIGHT); }
    public void OnDie()
    {
        if (!isDie)
        {
            OnPlayerDie?.Invoke(this);

            moveController.SetMovement(Vector3.zero, 0f);
            gameObject.layer = Constants.LAYER_DIE;
            hitState = HIT_STATE.Invincible;
            IsDie = true;
            state?.SetState(ACTION_STATE.PLAYER_DIE, STATE_SWITCH_BY.WEIGHT);
            StartCoroutine(CoWaitForDisapear(Constants.TIME_NORMAL_MONSTER_DISAPEAR));
        }
    }
    public void OnDie(PlayerStatusData characterStatus)
    {
        if (characterStatus.CurrentHP <= 0)
        {
            OnDie();
        }
    }

    public void SetAttackSpeed(PlayerStatusData statusData)
    {
        animator.SetFloat(Constants.ANIMATOR_PARAMETERS_FLOAT_ATTACK_SPEED, statusData.AttackSpeed);
    }

    public void OnCompetSuccess()
    {
        Managers.CompeteManager.OnEnemyFail();
    }

    public void SetForwardDirection(Vector3 direction)
    {
        if (isLockOn)
        {
            return;
        }

        transform.forward = direction;
    }


    #region Property
    public PlayerCamera PlayerCamera { get { return playerCamera; } set { playerCamera = value; } }
    public PlayerMoveController MoveController { get { return moveController; } }
    public PlayerWeaponController WeaponController { get { return weaponController; } }
    public PlayerInteractionController InteractionController { get { return interactionController; } }
    public StatusEffectController<PlayerCharacter> StatusEffectController { get { return statusEffectController; } }

    public CharacterData CharacterData { get { return characterData; } }
    public PlayerStatusData Status { get { return characterData?.StatusData; } }
    public PlayerInventoryData InventoryData { get { return characterData?.InventoryData; } }
    public PlayerEquipmentSlotData EquipmentSlotData { get { return characterData?.EquipmentSlotData; } }
    public PlayerQuestData QuestData { get { return characterData?.QuestData; } }
    public PlayerLocationData LocationData { get { return characterData?.LocationData; } }

    public PlayerWeapon CurrentWeapon { get { return weaponController.CurrentWeapon; } }
    #endregion
}
