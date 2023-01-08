#define EDITOR_TEST

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseCharacter : BaseActor, ICompetable, IStunable, ILightHitable, IHeavyHitable
{
    [Header("Base Character")]
    [SerializeField] protected CharacterData characterData;
    [SerializeField] protected Vector3 cameraOffset;

    protected PlayerCamera playerCamera;
    protected CharacterController characterController;
    protected CharacterStateController state;

    protected PlayerAttackController weapon;
    protected PlayerDefenseController shield;

    public override void Awake()
    {
        base.Awake();
        TryGetComponent(out characterController);

#if EDITOR_TEST
#else
        characterData = Managers.DataManager.SelectCharacterData;
        Managers.DataManager.SavePlayerData();
#endif
        StatusData.OnCharacterStatusChanged -= AdjustAttackSpeed;
        StatusData.OnCharacterStatusChanged += AdjustAttackSpeed;

        StatusData.OnDie -= OnDie;
        StatusData.OnDie += OnDie;

        AdjustAttackSpeed(StatusData);
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
        AutoRecoverStamina();
    }

    public void Rebirth()
    {

    }

    public virtual void OnLightHit() { TrySwitchState(CHARACTER_STATE.LightHit); }
    public virtual void OnHeavyHit() { TrySwitchState(CHARACTER_STATE.HeavyHit); }
    public virtual void OnStun(float duration) { AddAbnormalState(ABNORMAL_TYPE.Stun, duration); }
    public virtual void OnCompete() { TrySwitchState(CHARACTER_STATE.Compete); }
    public virtual void OnDie(StatusData characterStats) { }

    public abstract CHARACTER_STATE NextState();

    public float DamageProcess(BaseEnemy enemy, float ratio, Vector3 hitPoint)
    {
        // Basic Damage Process
        float damage = (characterData.StatusData.AttackPower - enemy.EnemyData.DefensivePower * 0.5f) * 0.5f;
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

        enemy.EnemyData.CurrentHP -= damage;

        if(Managers.SceneManagerCS.CurrentScene.RequestObject(Constants.Prefab_Floating_Damage_Text).TryGetComponent(out FloatingDamageText floatingDamageText))
            floatingDamageText.SetDamageText(isCritical, damage, hitPoint);

        if (enemy.IsDie)
            Managers.GameEventManager.EventQueue.Enqueue(new GameEventMessage(GAME_EVENT_TYPE.OnKillEnemy, enemy));

        return damage;
    }

    public void AutoRecoverStamina()
    {
        characterData.StatusData.CurrentSP += (characterData.StatusData.MaxSP * Constants.CHARACTER_STAMINA_AUTO_RECOVERY * 0.01f * Time.deltaTime);
    }
    public void AdjustAttackSpeed(StatusData statusData)
    {
        animator.SetFloat(Constants.ANIMATOR_PARAMETERS_FLOAT_ATTACK_SPEED, statusData.AttackSpeed);
    }

    public void TrySwitchState(CHARACTER_STATE targetState)
    {
        state.TrySwitchState(targetState);
    }
    public void SwitchState(CHARACTER_STATE targetState)
    {
        Managers.InputManager?.Initialize();
        state.SwitchState(targetState);
    }
    public void OnCompetSuccess()
    {
        Managers.CompeteManager.OnEnemyFail();
    }

    #region Property
    public CharacterData CharacterData { get { return characterData; } }
    public StatusData StatusData { get { return characterData?.StatusData; } }
    public InventoryData InventoryData { get { return characterData?.InventoryData; } }
    public EquipmentSlotData EquipmentSlotData { get { return characterData?.EquipmentSlotData; } }
    public CharacterQuestData QuestData { get { return characterData?.QuestData; } }

    public CharacterStateController State { get { return state; } }

    public PlayerCamera PlayerCamera { get { return playerCamera; } set { playerCamera = value; } }
    public CharacterController CharacterController { get { return characterController; } }

    public PlayerAttackController Weapon { get { return weapon; } }
    public PlayerDefenseController Shield { get { return shield; } }
    #endregion
}
