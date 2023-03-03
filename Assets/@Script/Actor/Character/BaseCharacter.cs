#define EDITOR_TEST

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseCharacter : BaseActor, ICompetable, IStunable
{
    [Header("Base Character")]
    [SerializeField] protected CharacterData characterData;
    [SerializeField] protected Vector3 cameraOffset;

    protected CharacterController characterController;
    protected PlayerCamera playerCamera;
    protected CharacterFSM state;
    protected StatusEffectController<BaseCharacter> statusEffectController;

    protected PlayerAttackController weapon;
    protected PlayerDefenseController shield;
    protected bool isGround;

    public override void Awake()
    {
        base.Awake();
        TryGetComponent(out characterController);
        state = new CharacterFSM(this);

#if EDITOR_TEST
#else
        characterData = Managers.DataManager.SelectCharacterData;
        Managers.DataManager.SavePlayerData();
#endif
        Status.OnCharacterStatusChanged -= AdjustAttackSpeed;
        Status.OnCharacterStatusChanged += AdjustAttackSpeed;

        Status.OnDie -= OnDie;
        Status.OnDie += OnDie;

        AdjustAttackSpeed(Status);
    }
    protected virtual void OnEnable()
    {
        Rebirth();
    }
    protected virtual void Start()
    {
        playerCamera = Managers.GameManager.PlayerCamera;
        playerCamera.TargetTransform = transform;
        playerCamera.TargetOffset = cameraOffset;
    }

    protected virtual void Update()
    {
    }

    public void Rebirth()
    {

    }

    public virtual void OnHit() { state?.SetState(ACTION_STATE.PLAYER_HIT_LIGHT, STATE_SWITCH_BY.WEIGHT); }
    public virtual void OnLightHit() { state?.SetState(ACTION_STATE.PLAYER_HIT_LIGHT, STATE_SWITCH_BY.WEIGHT); }
    public virtual void OnHeavyHit() { state?.SetState(ACTION_STATE.PLAYER_HIT_HEAVY, STATE_SWITCH_BY.WEIGHT); }

    public virtual void OnStun(float duration) { state?.SetState(ACTION_STATE.PLAYER_STUN, STATE_SWITCH_BY.WEIGHT, duration); }

    public virtual void OnCompete() { state?.SetState(ACTION_STATE.PLAYER_COMPETE, STATE_SWITCH_BY.WEIGHT); }
    public virtual void OnDie(StatusData characterStats) { }

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

    public void AdjustAttackSpeed(StatusData statusData)
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
    public CharacterController CharacterController { get { return characterController; } }

    public CharacterFSM State { get { return state; } }
    public StatusEffectController<BaseCharacter> StatusEffectController { get { return statusEffectController; } }

    public PlayerAttackController Weapon { get { return weapon; } }
    public PlayerDefenseController Shield { get { return shield; } }

    public bool IsGround
    {
        get
        {
            if (characterController.isGrounded)
                isGround = true;

            else
            {
                var ray = new Ray(transform.position, Vector3.down);
                var rayDistance = 0.5f;
                isGround = Physics.Raycast(ray, rayDistance, LayerMask.GetMask("Terrain"));
            }
            return isGround;
        }
    }
    #endregion
}
