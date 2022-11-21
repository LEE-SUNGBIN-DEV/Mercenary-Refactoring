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
    protected CharacterStatus status;
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
        status = new CharacterStatus(this);
        state = new CharacterState(this);

        Status.OnDie -= Die;
        Status.OnDie += Die;
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
        float damage = (status.AttackPower - enemy.DefensivePower * 0.5f) * 0.5f;
        if (damage < 0)
        {
            damage = 0;
        }
        damage += ((status.AttackPower / 8f - status.AttackPower / 16f) + 1f);

        // Critical Process
        bool isCritical;
        float randomNumber = Random.Range(0.0f, 100.0f);
        if (randomNumber <= status.CriticalChance)
        {
            isCritical = true;
            damage *= (1 + status.CriticalDamage * 0.01f);
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

    public void Die(CharacterStatus characterStats)
    {
    }
    public void Rebirth()
    {

    }
    public void AutoRecoverStamina()
    {
        status.CurrentStamina += (status.MaxStamina * Constants.CHARACTER_STAMINA_AUTO_RECOVERY * 0.01f * Time.deltaTime);
    }
    public void SetInteract(bool isInteract)
    {
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
    public CharacterStatus Status { get { return status; } }
    public CharacterState State { get { return state; } }
    
    public PlayerCamera PlayerCamera { get { return playerCamera; } set { playerCamera = value; } }
    public CharacterController CharacterController { get { return characterController; } }
    public Animator Animator { get { return animator; } }
    #endregion
}
