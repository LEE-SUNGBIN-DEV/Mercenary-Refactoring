using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;

public class PlayerCharacter : BaseActor, ICompetable
{
    public event UnityAction<PlayerCharacter> OnPlayerSpawn;
    public event UnityAction<PlayerCharacter> OnPlayerDie;
    public event UnityAction<PlayerCharacter> OnPlayerHit;

    private PlayerCamera playerCamera;

    [Header("Character Data")]
    [SerializeField] private CharacterData characterData;

    [Header("Character Controllers")]
    [SerializeField] private PlayerMoveController moveController;
    [SerializeField] private PlayerInteractionController interactionController;
    [SerializeField] private UniqueEquipmentController uniqueEquipmentController;
    [SerializeField] private StatusEffectController<PlayerCharacter> statusEffectController;

    [Header("Interaction")]
    [SerializeField] private bool isLockOn = false;

    protected override void Awake()
    {
        base.Awake();
        characterData = Managers.DataManager.CurrentCharacterData;

        // Move Controller
        TryGetComponent(out moveController);

        // Interaction Controller
        interactionController = new PlayerInteractionController();
        interactionController.Initialize();

        // Weapon Controller
        uniqueEquipmentController = new UniqueEquipmentController();
        uniqueEquipmentController.Initialize(this);

        // Add State
        AddDefaultState();

        // Add Audio Sources
        footstepAudioClipNames =
            new string[]
            {
                    "AUDIO_PLAYER_FOOTSTEP_01",
                    "AUDIO_PLAYER_FOOTSTEP_02",
                    "AUDIO_PLAYER_FOOTSTEP_03",
                    "AUDIO_PLAYER_FOOTSTEP_04",
                    "AUDIO_PLAYER_FOOTSTEP_05",
                    "AUDIO_PLAYER_FOOTSTEP_06",
            };

        ApplyAttackSpeed(StatusData);
        uniqueEquipmentController.SwitchWeapon(characterData.InventoryData.CurrentWeaponType);
        state.SetState(CurrentWeapon.IdleState, STATE_SWITCH_BY.FORCED);
    }

    #region Private
    private void OnEnable()
    {
        Spawn();
        StatusData.OnChangeStatusData += ApplyAttackSpeed;
        StatusData.OnChangeStatusData += OnDie;
        OnPlayerHit += interactionController.ExitInteraction;
        OnPlayerDie += interactionController.ExitInteraction;
    }
    private void OnDisable()
    {
        StatusData.OnChangeStatusData -= ApplyAttackSpeed;
        StatusData.OnChangeStatusData -= OnDie;
        OnPlayerHit -= interactionController.ExitInteraction;
        OnPlayerDie -= interactionController.ExitInteraction;
    }
    private void Update()
    {
        state?.Update();
    }
    private void OnAnimatorMove()
    {
        actorRigidbody.velocity += animator.velocity;
    }
    #endregion

    private void AddDefaultState()
    {
        // Sub
        state.StateDictionary.Add(ACTION_STATE.COMMON_UPPER_EMPTY, new CommonStateUpperEmpty(this));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_DRINK, new PlayerStateDrink(this));

        // Main
        state.StateDictionary.Add(ACTION_STATE.PLAYER_SLIDE, new PlayerStateSlide(this));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_FALL, new PlayerStateFall(this));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_LANDING, new PlayerStateLanding(this));

        state.StateDictionary.Add(ACTION_STATE.PLAYER_RESPONSE_IN, new PlayerStateResponseIn(this));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_RESPONSE_LOOP, new PlayerStateResponseLoop(this));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_RESPONSE_OUT, new PlayerStateResponseOut(this));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_RESPONSE_SIMPLE, new PlayerStateResponseSimple(this));

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
        if(StatusData.CurrentHP <= 0)
            StatusData.Respawn();

        gameObject.layer = Constants.LAYER_PLAYER;
        hitState = HIT_STATE.HITTABLE;
        IsDie = false;
        state?.SetState(CurrentWeapon.IdleState, STATE_SWITCH_BY.FORCED);
        OnPlayerSpawn?.Invoke(this);
    }

    public bool TrySwitchWeapon(WEAPON_TYPE targetWeapon)
    {
        switch (targetWeapon)
        {
            case WEAPON_TYPE.HALBERD:
                if (state.SetSubState(ACTION_STATE.PLAYER_HALBERD_EQUIP, STATE_SWITCH_BY.WEIGHT))
                {
                    uniqueEquipmentController.SwitchWeapon(targetWeapon);
                    return true;
                }
                break;

            case WEAPON_TYPE.SWORD_SHIELD:
                if (state.SetSubState(ACTION_STATE.PLAYER_SWORD_SHIELD_EQUIP, STATE_SWITCH_BY.WEIGHT))
                {
                    uniqueEquipmentController.SwitchWeapon(targetWeapon);
                    return true;
                }
                break;
        }
        return false;
    }

    public DamageInformation TakeDamage(BaseEnemy attacker, float damageRatio)
    {
        // Basic Damage Process
        float reducedDefensivePower = StatusData.StatDict[STAT_TYPE.STAT_DEFENSE_POWER].GetFinalValue() * (1 - attacker.Status.DefensePenetration);
        float damage = attacker.Status.AttackPower * (1 - (reducedDefensivePower / (100 + reducedDefensivePower)));

        if (damage < 0)
            damage = 0;

        // Critical Process
        if (Random.Range(0.0f, 100.0f) < attacker.Status.CriticalChance)
        {
            damage *= (1 + attacker.Status.CriticalDamage * 0.01f);
        }

        // Damage Random Range
        damage *= (damageRatio * Random.Range(0.9f, 1.1f));

        // Damage Reduction
        damage *= (1 - (StatusData.StatDict[STAT_TYPE.STAT_DAMAGE_REDUCTION_RATE].GetFinalValue() * 0.01f));

        // Add Fixed Damage
        damage += attacker.Status.FixedDamage;

        StatusData.ReduceHP(damage);

        return new DamageInformation(damage, false);
    }

    public void TakeHit(BaseEnemy attacker, float damageRatio, HIT_TYPE combatType, float duration = 0f)
    {
        DamageInformation damageInforamtion = TakeDamage(attacker, damageRatio);

        OnPlayerHit?.Invoke(this);
        sfxPlayer.PlaySFX("Audio_Player_Hit");

        if (StatusData.HitLevel > (int)combatType)
            return;

        transform.forward = Functions.GetXZAxisDirection(transform.position, attacker.transform.position);
        playerCamera.ActiveCorrectionMode(attacker.transform.position);

        switch (combatType)
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

    public virtual void OnCompete()
    {
        state?.SetState(ACTION_STATE.PLAYER_COMPETE, STATE_SWITCH_BY.WEIGHT);
    }

    public void OnDie()
    {
        if (!isDie)
        {
            OnPlayerDie?.Invoke(this);
            IsDie = true;
            StatusData.CurrentHP = 0f;
            StatusData.CurrentSP = 0f;
            LocationData.SetLocationMode(LOCATION_MODE.SCENE_RESPONSE_POINT);

            gameObject.layer = Constants.LAYER_DIE;
            hitState = HIT_STATE.INVINCIBLE;
            moveController.SetMove(Vector3.zero, 0f);
            state?.SetState(ACTION_STATE.PLAYER_DIE, STATE_SWITCH_BY.WEIGHT);

            Managers.UIManager.UIInteractionPanelCanvas.CloseCurrentFocusPanel();
            Managers.UIManager.UIFixedPanelCanvas.DiePanel.OpenPanel();
            StartCoroutine(CoWaitForDisapear(Constants.TIME_NORMAL_ENEMY_DISAPEAR));
        }
    }
    public void OnDie(CharacterStatusData characterStatus)
    {
        if (characterStatus.CurrentHP <= 0)
        {
            OnDie();
        }
    }

    public void ApplyAttackSpeed(CharacterStatusData statusData)
    {
        animator.SetFloat(Constants.ANIMATOR_PARAMETERS_FLOAT_ATTACK_SPEED, statusData.FinalAttackSpeed);
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
    public PlayerInteractionController InteractionController { get { return interactionController; } }
    public UniqueEquipmentController UniqueEquipmentController { get { return uniqueEquipmentController; } }
    public StatusEffectController<PlayerCharacter> StatusEffectController { get { return statusEffectController; } }

    public CharacterData CharacterData { get { return characterData; } }
    public CharacterStatusData StatusData { get { return characterData?.StatusData; } }
    public CharacterInventoryData InventoryData { get { return characterData?.InventoryData; } }
    public CharacterQuestData QuestData { get { return characterData?.QuestData; } }
    public CharacterLocationData LocationData { get { return characterData?.LocationData; } }
    public CharacterSkillData SkillData { get { return characterData?.SkillData; } }
    public CharacterSceneData SceneData { get { return characterData?.SceneData; } }

    public PlayerWeapon CurrentWeapon { get { return uniqueEquipmentController.CurrentWeapon; } }
    #endregion
}
