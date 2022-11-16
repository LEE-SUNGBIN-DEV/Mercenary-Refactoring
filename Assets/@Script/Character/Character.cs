using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Character : MonoBehaviour
{
    [SerializeField] private Vector3 cameraOffset;

    private PlayerCamera playerCamera;
    private CharacterController characterController;
    private Animator characterAnimator;
    private InventoryPanel characterInventory;

    private UserInput playerInput;
    [SerializeField] private CharacterData characterData;
    private CharacterStats characterStats;
    private CharacterState characterState;


    protected virtual void Awake()
    {
        playerCamera = Managers.GameManager.PlayerCamera;
        playerCamera.TargetTransform = transform;
        playerCamera.TargetOffset = cameraOffset;

        characterController = GetComponent<CharacterController>();
        characterAnimator = GetComponent<Animator>();
        characterInventory = GetComponent<InventoryPanel>();

        playerInput = new UserInput();
        characterData = Managers.DataManager.CurrentCharacterData;
        characterStats = new CharacterStats(this);
        characterState = new CharacterState(this);

        CharacterStats.OnDie -= Die;
        CharacterStats.OnDie += Die;

        Managers.DataManager.CurrentCharacter = this;
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

    private void OnDestroy()
    {
        Managers.DataManager.CurrentCharacter = null;
    }

    public abstract CHARACTER_STATE DetermineCharacterState();
    
    public void Die(CharacterStats characterStats)
    {
    }
    public void Rebirth()
    {

    }
    public void AutoRecoverStamina()
    {
        characterStats.CurrentStamina += (characterStats.MaxStamina * Constants.CHARACTER_STAMINA_AUTO_RECOVERY * 0.01f * Time.deltaTime);
    }
    public void SetInteract(bool isInteract)
    {
    }


    #region Animation Event
    public void SwitchCharacterState(CHARACTER_STATE targetState)
    {
        CharacterState.SwitchCharacterState(targetState);
    }
    #endregion

    #region Property
    public UserInput PlayerInput { get { return playerInput; } }
    public CharacterData CharacterData { get { return characterData; } }
    public CharacterStats CharacterStats { get { return characterStats; } }
    public CharacterState CharacterState { get { return characterState; } }
    
    public PlayerCamera PlayerCamera { get { return playerCamera; } set { playerCamera = value; } }
    public CharacterController CharacterController { get { return characterController; } }
    public Animator CharacterAnimator { get { return characterAnimator; } }
    public InventoryPanel CharacterInventory { get { return characterInventory; } }
    #endregion
}
