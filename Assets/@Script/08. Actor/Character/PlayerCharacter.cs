#define EDITOR_TEST

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCharacter : BaseActor, ICompetable, IStunable
{
    public event UnityAction<PlayerCharacter> OnPlayerDie;

    [Header("Player Character")]
    [SerializeField] private CharacterData characterData;

    private PlayerCamera playerCamera;
    private StatusEffectController<PlayerCharacter> statusEffectController;

    [Header("Player Weapon")]
    private PlayerHalberd halberd;
    private PlayerSwordShield swordShield;
    private PlayerWeapon currentWeapon;
    private Dictionary<WEAPON_TYPE, PlayerWeapon> weaponDictionary;

    protected override void Awake()
    {
        base.Awake();

        // Data
#if EDITOR_TEST
#else
        characterData = Managers.DataManager.SelectCharacterData;
        Managers.DataManager.SavePlayerData();
#endif
        Status.OnCharacterStatusChanged -= ApplyAttackSpeed;
        Status.OnCharacterStatusChanged += ApplyAttackSpeed;

        Status.OnCharacterStatusChanged -= OnDie;
        Status.OnCharacterStatusChanged += OnDie;

        ApplyAttackSpeed(Status);

        // Initialize Weapon
        if (TryGetComponent<PlayerHalberd>(out halberd))
            halberd.InitializeWeapon(this);

        if (TryGetComponent<PlayerSwordShield>(out swordShield))
            swordShield.InitializeWeapon(this);

        weaponDictionary = new Dictionary<WEAPON_TYPE, PlayerWeapon>()
        {
            { WEAPON_TYPE.HALBERD, halberd },
            { WEAPON_TYPE.SWORD_SHIELD, swordShield }
        };

        currentWeapon = weaponDictionary[WEAPON_TYPE.HALBERD];

        // Initialize State
        InitializeCharacterState();
        if(TryEquipWeapon(currentWeapon.WeaponType))
        {
            state.SetState(currentWeapon.BasicStateInformation.idleState, STATE_SWITCH_BY.FORCED);
        }
    }
    
    private void OnEnable()
    {
        Rebirth();
    }

    private void Start()
    {
        playerCamera = Managers.GameManager.PlayerCamera;
        playerCamera.TargetTransform = transform;
    }

    private void Update()
    {
        state?.Update();
    }

    public void InitializeCharacterState()
    {
        // Common State
        state.StateDictionary.Add(ACTION_STATE.COMMON_UPPER_EMPTY, new CommonStateUpperEmpty(this));

        state.StateDictionary.Add(ACTION_STATE.PLAYER_FALL, new PlayerStateFall(this));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_LANDING, new PlayerStateLanding(this));

        state.StateDictionary.Add(ACTION_STATE.PLAYER_ROLL, new PlayerStateRoll(this));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_HIT_LIGHT, new PlayerStateLightHit(this));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_HIT_HEAVY, new PlayerStateHeavyHit(this));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_HIT_HEAVY_LOOP, new PlayerStateHeavyHitLoop(this));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_STAND_UP, new PlayerStateStandRoll(this));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_STAND_ROLL, new PlayerStateStandRoll(this));

        state.StateDictionary.Add(ACTION_STATE.PLAYER_STUN, new PlayerStateStun(this));
        state.StateDictionary.Add(ACTION_STATE.PLAYER_DIE, new PlayerStateDie(this));

        state.SetSubState(ACTION_STATE.COMMON_UPPER_EMPTY, STATE_SWITCH_BY.FORCED);
    }

    public void Rebirth()
    {

    }

    public bool TryEquipWeapon(WEAPON_TYPE weaponType)
    {
        bool isSucess = false;

        switch (weaponType)
        {
            case WEAPON_TYPE.HALBERD:
                if (state.SetSubState(ACTION_STATE.PLAYER_HALBERD_EQUIP, STATE_SWITCH_BY.WEIGHT))
                {
                    swordShield.UnequipWeapon();
                    halberd.EquipWeapon();
                    isSucess = true;
                }
                break;

            case WEAPON_TYPE.SWORD_SHIELD:
                if (state.SetSubState(ACTION_STATE.PLAYER_SWORD_SHIELD_EQUIP, STATE_SWITCH_BY.WEIGHT))
                {
                    halberd.UnequipWeapon();
                    swordShield.EquipWeapon();
                    isSucess = true;
                }
                break;
        }

        if (isSucess)
        {
            currentWeapon = weaponDictionary[weaponType];
            return true;
        }
        return false;
    }

    public virtual void OnHit() { state?.SetState(ACTION_STATE.PLAYER_HIT_LIGHT, STATE_SWITCH_BY.WEIGHT); }
    public virtual void OnLightHit() { state?.SetState(ACTION_STATE.PLAYER_HIT_LIGHT, STATE_SWITCH_BY.WEIGHT); }
    public virtual void OnHeavyHit() { state?.SetState(ACTION_STATE.PLAYER_HIT_HEAVY, STATE_SWITCH_BY.WEIGHT); }

    public virtual void OnStun(float duration) { state?.SetState(ACTION_STATE.PLAYER_STUN, STATE_SWITCH_BY.WEIGHT, duration); }

    public virtual void OnCompete() { state?.SetState(ACTION_STATE.PLAYER_COMPETE, STATE_SWITCH_BY.WEIGHT); }
    public void OnDie(StatusData characterStatus)
    {
        if (!isDie && characterStatus.CurrentHP <= 0)
        {
            OnPlayerDie?.Invoke(this);

            gameObject.layer = Constants.LAYER_DIE;
            isInvincible = true;
            IsDie = true;
            state?.SetState(ACTION_STATE.PLAYER_DIE, STATE_SWITCH_BY.WEIGHT);
            StartCoroutine(CoWaitForDisapear(Constants.TIME_NORMAL_MONSTER_DISAPEAR));
        }
    }

    public float DamageProcess(BaseEnemy enemy, float ratio, Vector3 hitPoint)
    {
        // Basic Damage Process
        float damage = (characterData.StatusData.AttackPower - enemy.Status.DefensivePower * 0.5f) * 0.5f;
        if (damage < 0) damage = 0;

        damage += ((characterData.StatusData.AttackPower / 8f - characterData.StatusData.AttackPower / 16f) + 1f);

        // Critical Process
        bool isCritical;
        float randomNumber = Random.Range(0.0f, 100.0f);
        if (randomNumber <= characterData.StatusData.CriticalChance)
        {
            isCritical = true;
            damage *= (1 + characterData.StatusData.CriticalDamage * 0.01f);
            //Managers.AudioManager.PlaySFX("Player Critical Attack");
        }
        else
        {
            isCritical = false;
            //Managers.AudioManager.PlaySFX("Player Attack");
        }

        // Damage Ratio Process
        damage *= ratio;

        // Final Damage Process
        float damageRange = Random.Range(0.9f, 1.1f);
        damage *= damageRange;

        enemy.Status.CurrentHP -= damage;

        if(Managers.SceneManagerCS.CurrentScene.RequestObject(Constants.Prefab_Floating_Damage_Text).TryGetComponent(out FloatingDamageText floatingDamageText))
            floatingDamageText.SetDamageText(isCritical, damage, hitPoint);

        if (enemy.IsDie)
            Managers.GameEventManager.EventQueue.Enqueue(new GameEventMessage(GAME_EVENT_TYPE.OnKillEnemy, enemy));

        return damage;
    }

    public void ApplyAttackSpeed(StatusData statusData)
    {
        animator.SetFloat(Constants.ANIMATOR_PARAMETERS_FLOAT_ATTACK_SPEED, statusData.AttackSpeed);
    }

    public void OnCompetSuccess()
    {
        Managers.CompeteManager.OnEnemyFail();
    }


    #region Property
    public CharacterData CharacterData { get { return characterData; } }
    public StatusData Status { get { return characterData?.StatusData; } }
    public InventoryData InventoryData { get { return characterData?.InventoryData; } }
    public EquipmentSlotData EquipmentSlotData { get { return characterData?.EquipmentSlotData; } }
    public CharacterQuestData QuestData { get { return characterData?.QuestData; } }

    public PlayerCamera PlayerCamera { get { return playerCamera; } set { playerCamera = value; } }

    public StatusEffectController<PlayerCharacter> StatusEffectController { get { return statusEffectController; } }

    public PlayerHalberd Halberd { get { return halberd; } }
    public PlayerSwordShield SwordShield { get { return swordShield; } }
    public PlayerWeapon CurrentWeapon { get { return currentWeapon; } }
    public Dictionary<WEAPON_TYPE, PlayerWeapon> WeaponDictionary { get { return weaponDictionary; } }
    #endregion
}
