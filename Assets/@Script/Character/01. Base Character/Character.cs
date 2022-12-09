#define TEST

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Character : MonoBehaviour
{
    [SerializeField] private Vector3 cameraOffset;

    [Header("Character Component")]
    protected PlayerCamera playerCamera;
    protected CharacterController characterController;
    protected Animator animator;

    protected PlayerInput playerInput;
    [SerializeField] protected CharacterData characterData;
    protected CharacterStateController state;
    protected bool isInvincible;

    [Header("Object Pool")]
    [SerializeField] protected ObjectPoolController objectPoolController = new ObjectPoolController();

    protected virtual void Awake()
    {
        TryGetComponent(out characterController);
        TryGetComponent(out animator);

        isInvincible = false;
        playerInput = new PlayerInput();
#if TEST
        StatusData.OnCharacterStatusChanged -= SetAttackSpeed;
        StatusData.OnCharacterStatusChanged += SetAttackSpeed;

        StatusData.OnDie -= OnDie;
        StatusData.OnDie += OnDie;

        SetAttackSpeed(StatusData);
#else
        characterData = Managers.DataManager.SelectCharacterData;
        state = new CharacterState(this);

        StatusData.OnCharacterStatusChanged -= SetAttackSpeed;
        StatusData.OnCharacterStatusChanged += SetAttackSpeed;

        StatusData.OnDie -= Die;
        StatusData.OnDie += Die;

        SetAttackSpeed(StatusData);

        Managers.DataManager.SavePlayerData();
#endif
    }
    protected virtual void OnEnable()
    {
        Managers.UIManager.InteractPlayer -= SetInteract;
        Managers.UIManager.InteractPlayer += SetInteract;

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

    public virtual void OnHit()
    {
        SwitchCharacterState(CHARACTER_STATE.Hit);
    }
    public virtual void OnHeavyHit()
    {
        SwitchCharacterState(CHARACTER_STATE.HeavyHit);
    }
    public virtual void OnStun()
    {
        SwitchCharacterState(CHARACTER_STATE.Stun);
    }
    public virtual void OnCompete() { }
    public virtual void OnDie(StatusData characterStats) { }

    public abstract CHARACTER_STATE DetermineCharacterState();

    public void DamageProcess(Enemy enemy, float ratio)
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
            Managers.AudioManager.PlaySFX("Player Critical Attack");
        }
        else
        {
            isCritical = false;
            Managers.AudioManager.PlaySFX("Player Attack");
        }

        // Damage Ratio Process
        damage *= ratio;

        // Final Damage Process
        float damageRange = Random.Range(0.9f, 1.1f);
        damage *= damageRange;

        enemy.EnemyData.CurrentHP -= damage;
        FloatingDamageText floatingDamageText = Managers.ObjectPoolManager.RequestObject(Constants.RESOURCE_NAME_PREFAB_FLOATING_DAMAGE_TEXT).GetComponent<FloatingDamageText>();
        floatingDamageText.SetDamageText(isCritical, damage, enemy.transform.position);

        if (enemy.IsDie)
            Managers.EventManager.OnKillEnemy?.Invoke(this, enemy);
    }

    public void Rebirth()
    {

    }

    // !! 스태미나 자동 회복
    public void AutoRecoverStamina()
    {
        characterData.StatusData.CurrentSP += (characterData.StatusData.MaxSP * Constants.CHARACTER_STAMINA_AUTO_RECOVERY * 0.01f * Time.deltaTime);
    }
    public void SetInteract(bool isInteract)
    {
    }
    public void SetAttackSpeed(StatusData statusData)
    {
        animator.SetFloat("attackSpeed", statusData.AttackSpeed);
    }

    #region Animation Event
    public void SwitchCharacterState(CHARACTER_STATE targetState)
    {
        State.SwitchCharacterState(targetState);
    }
    #endregion

    #region Property
    public PlayerInput PlayerInput { get { return playerInput; } }
    public CharacterData CharacterData { get { return characterData; } }
    public StatusData StatusData { get { return characterData?.StatusData; } }
    public InventoryData InventoryData { get { return characterData?.InventoryData; } }
    public EquipmentSlotData EquipmentSlotData { get { return characterData?.EquipmentSlotData; } }
    public UserQuestData QuestData { get { return characterData?.QuestData; } }

    public CharacterStateController State { get { return state; } }
    public bool IsInvincible { get { return isInvincible; } set { isInvincible = value; } }

    public PlayerCamera PlayerCamera { get { return playerCamera; } set { playerCamera = value; } }
    public CharacterController CharacterController { get { return characterController; } }
    public Animator Animator { get { return animator; } }
    public ObjectPoolController ObjectPoolController { get { return objectPoolController; } }
    #endregion
}
