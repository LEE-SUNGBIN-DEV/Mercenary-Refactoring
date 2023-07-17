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
    private PlayerMoveController moveController;
    [SerializeField] private PlayerResonanceWater resonanceWater;
    [SerializeField] private PlayerWeaponController weaponController;
    [SerializeField] private PlayerInteractionController interactionController;
    private StatusEffectController<PlayerCharacter> statusEffectController;

    [Header("Interaction")]
    [SerializeField] private bool isActiveUIInteraction;
    [SerializeField] private bool isActiveObjectInteraction;
    [SerializeField] private bool isLockOn = false;

    public void InitializeCharacter(CharacterData characterData)
    {
        base.InitializeActor();
        this.characterData = characterData;

        Managers.UIManager.OnActiveUserPanel += SetUIInteraction;

        Status.OnChangeStatusData += SetAttackSpeed;
        Status.OnChangeStatusData += OnDie;

        Status.Respawn();
        SetAttackSpeed(Status);

        resonanceWater = Functions.FindChild<PlayerResonanceWater>(gameObject, "Prefab_Resonance_Water", true);
        resonanceWater.Initialize(this);

        if (TryGetComponent(out moveController))
        {
            moveController.Initialize(this);
        }

        // Initialize InteractionController
        interactionController = new PlayerInteractionController();
        interactionController.Initialize();
        interactionController.OnActiveInteraction += SetObjectInteraction;
        OnPlayerHit += interactionController.InactiveInteraction;
        OnPlayerDie += interactionController.InactiveInteraction;

        // Initialize Weapon
        weaponController = new PlayerWeaponController();
        weaponController.Initialize(this);

        // Initialize State
        AddDefaultState();
        if (TryEquipWeapon(CurrentWeapon.WeaponType))
            state.SetState(CurrentWeapon.IdleState, STATE_SWITCH_BY.FORCED);

        // Initialize Audio Sources
        footstepAudioClipNames =
            new string[]
            {
                    "Audio_Player_Footstep_Dirt_01",
                    "Audio_Player_Footstep_Dirt_02",
                    "Audio_Player_Footstep_Dirt_03",
                    "Audio_Player_Footstep_Dirt_04",
                    "Audio_Player_Footstep_Dirt_05",
            };

        isActiveUIInteraction = false;
        isActiveObjectInteraction = false;
    }
    
    private void OnEnable()
    {
        Spawn();
    }

    private void FixedUpdate()
    {
        moveController.UpdateGroundState();
        moveController.UpdatePosition();
    }

    private void Update()
    {
        UpdateCharacterInputs();
        state?.Update();
    }

    private void OnDisable()
    {
        if (Managers.NullCheckInstance != null)
            Managers.UIManager.OnActiveUserPanel -= SetUIInteraction;

        interactionController.OnActiveInteraction -= SetObjectInteraction;

        OnPlayerHit -= interactionController.InactiveInteraction;
        OnPlayerDie -= interactionController.InactiveInteraction;

        Status.OnChangeStatusData -= SetAttackSpeed;
        Status.OnChangeStatusData -= OnDie;
    }

    public void AddDefaultState()
    {
        // Sub
        state.StateDictionary.Add(ACTION_STATE.COMMON_UPPER_EMPTY, new CommonStateUpperEmpty(this));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_DRINK, new PlayerStateDrink(this));

        // Main
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

    public void UpdateCharacterInputs()
    {
        if(isActiveUIInteraction || isActiveObjectInteraction)
            Managers.InputManager.CancelCharacterInputs();
        else
            Managers.InputManager.UpdateCharacterInputs();
    }

    public void SetUIInteraction(bool isInteraction)
    {
        isActiveUIInteraction = isInteraction;

        Managers.GameManager.SetCursorMode(isInteraction);
        if (!isActiveObjectInteraction)
            playerCamera.ActiveInteractionMode(isInteraction);
    }
    public void SetObjectInteraction(bool isInteraction)
    {
        isActiveObjectInteraction = isInteraction;

        Managers.UIManager.ActiveInteraction(isInteraction);
        playerCamera.ActiveInteractionMode(isInteraction);
        weaponController.HideWeapon(isInteraction);
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
        // Basic Damage Process
        float reducedDefensivePower = Status.DefensePower * (1 - attacker.Status.DefensePenetration);
        float damage = attacker.Status.AttackPower * (1 - (reducedDefensivePower / (100 + reducedDefensivePower)));

        if (damage < 0)
            damage = 0;

        damage += ((attacker.Status.AttackPower * 0.125f - Status.AttackPower * 0.0625f) + 1f);
        damage *= damageRatio;

        Status.ReduceHP(damage);

        return new DamageInformation(damage, false);
    }

    public void TakeHit(BaseEnemy attacker, float damageRatio, HIT_TYPE combatType, float duration = 0f)
    {
        DamageInformation damageInforamtion = TakeDamage(attacker, damageRatio);

        OnPlayerHit?.Invoke(this);
        sfxPlayer.PlaySFX("Audio_Player_Hit");

        if (Status.HitLevel > (int)combatType)
            return;

        transform.forward = Functions.GetZeroYDirection(transform.position, attacker.transform.position);
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
            Status.CurrentHP = 0f;
            Status.CurrentSP = 0f;
            LocationData.SetLocationMode(LOCATION_MODE.SCENE_RESONANCE_POINT);

            gameObject.layer = Constants.LAYER_DIE;
            hitState = HIT_STATE.Invincible;
            moveController.SetMovementAndRotation(Vector3.zero, 0f);
            state?.SetState(ACTION_STATE.PLAYER_DIE, STATE_SWITCH_BY.WEIGHT);

            Managers.UIManager.CloseActiveUserPanel();
            Managers.UIManager.OpenPanel(Managers.UIManager.GameSceneUI.DiePanel);
            StartCoroutine(CoWaitForDisapear(Constants.TIME_NORMAL_MONSTER_DISAPEAR));
        }
    }
    public void OnDie(CharacterStatusData characterStatus)
    {
        if (characterStatus.CurrentHP <= 0)
        {
            OnDie();
        }
    }

    public void SetAttackSpeed(CharacterStatusData statusData)
    {
        animator.SetFloat(Constants.ANIMATOR_PARAMETERS_FLOAT_ATTACK_SPEED, statusData.AttackSpeed);
    }

    public void SetForwardDirection(Vector3 direction)
    {
        if (isLockOn)
        {
            return;
        }

        transform.forward = direction;
    }

    public InputManager GetInput()
    {
        return Managers.InputManager;
    }

    #region Property
    public PlayerCamera PlayerCamera { get { return playerCamera; } set { playerCamera = value; } }
    public PlayerResonanceWater ResonanceWater { get { return resonanceWater; } }
    public PlayerMoveController MoveController { get { return moveController; } }
    public PlayerWeaponController WeaponController { get { return weaponController; } }
    public PlayerInteractionController InteractionController { get { return interactionController; } }
    public StatusEffectController<PlayerCharacter> StatusEffectController { get { return statusEffectController; } }

    public CharacterData CharacterData { get { return characterData; } }
    public CharacterStatusData Status { get { return characterData?.StatusData; } }
    public CharacterInventoryData InventoryData { get { return characterData?.InventoryData; } }
    public CharacterQuestData QuestData { get { return characterData?.QuestData; } }
    public CharacterLocationData LocationData { get { return characterData?.LocationData; } }
    public CharacterSkillData SkillData { get { return characterData?.SkillData; } }
    public CharacterSceneData SceneData { get { return characterData?.SceneData; } }

    public PlayerWeapon CurrentWeapon { get { return weaponController.CurrentWeapon; } }
    #endregion
}
