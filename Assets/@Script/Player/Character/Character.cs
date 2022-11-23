using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Character : MonoBehaviour
{
    [SerializeField] private Vector3 cameraOffset;

    protected PlayerCamera playerCamera;
    protected CharacterController characterController;
    protected Animator animator;

    protected PlayerInput playerInput;
    protected CharacterData characterData;
    protected CharacterState state;

    protected virtual void Awake()
    {
        playerCamera = Managers.GameManager.PlayerCamera;
        playerCamera.TargetTransform = transform;
        playerCamera.TargetOffset = cameraOffset;

        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        playerInput = new PlayerInput();
        characterData = Managers.DataManager.SelectCharacterData;
        characterData.StatusData = new StatusData(StatData);
        state = new CharacterState(this);

        StatusData.OnCharacterStatusChanged -= SetAttackSpeed;
        StatusData.OnCharacterStatusChanged += SetAttackSpeed;

        StatusData.OnDie -= Die;
        StatusData.OnDie += Die;

        SetAttackSpeed(StatusData);

        Managers.DataManager.SavePlayerData();
    }

    protected virtual void OnEnable()
    {
        Managers.UIManager.InteractPlayer -= SetInteract;
        Managers.UIManager.InteractPlayer += SetInteract;

        Rebirth();
    }

    protected virtual void Update()
    {
        AutoRecoverStamina();
    }

    public abstract CHARACTER_STATE DetermineCharacterState();
    // !!플레이어의 대미지를 계산하는 함수
    public void PlayerDamageProcess(Enemy enemy, float ratio)
    {
        // Basic Damage Process
        float damage = (characterData.StatusData.AttackPower - enemy.DefensivePower * 0.5f) * 0.5f;
        if (damage < 0)
        {
            damage = 0;
        }
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

        enemy.CurrentHitPoint -= damage;
        FloatingDamageText floatingDamageText = Managers.ObjectPoolManager.RequestObject(Constants.RESOURCE_NAME_PREFAB_FLOATING_DAMAGE_TEXT).GetComponent<FloatingDamageText>();
        floatingDamageText.SetDamageText(isCritical, damage, enemy.transform.position);

        if (enemy.IsDie)
        {
            Managers.EventManager.OnKillEnemy?.Invoke(this, enemy);
        }
    }

    public void Die(StatusData characterStats)
    {
    }
    public void Rebirth()
    {

    }
    public void AutoRecoverStamina()
    {
        characterData.StatusData.CurrentStamina += (characterData.StatusData.MaxStamina * Constants.CHARACTER_STAMINA_AUTO_RECOVERY * 0.01f * Time.deltaTime);
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
    public StatData StatData { get { return characterData?.StatData; } }
    public StatusData StatusData { get { return characterData?.StatusData; } }
    public InventoryData InventoryData { get { return characterData?.InventoryData; } }
    public EquipmentSlotData EquipmentSlotData { get { return characterData?.EquipmentSlotData; } }
    public QuestData QuestData { get { return characterData?.QuestData; } }

    public CharacterState State { get { return state; } }

    public PlayerCamera PlayerCamera { get { return playerCamera; } set { playerCamera = value; } }
    public CharacterController CharacterController { get { return characterController; } }
    public Animator Animator { get { return animator; } }
    #endregion
}
