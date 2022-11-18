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
    private InventoryPopup characterInventory;

    private UserInput playerInput;
    private CharacterData characterData;
    private CharacterStatus characterStatus;
    private CharacterState characterState;


    protected virtual void Awake()
    {
        playerCamera = Managers.GameManager.PlayerCamera;
        playerCamera.TargetTransform = transform;
        playerCamera.TargetOffset = cameraOffset;

        characterController = GetComponent<CharacterController>();
        characterAnimator = GetComponent<Animator>();
        characterInventory = GetComponent<InventoryPopup>();

        playerInput = new UserInput();
        characterData = Managers.DataManager.CurrentCharacterData;
        characterStatus = new CharacterStatus(this);
        characterState = new CharacterState(this);

        CharacterStatus.OnDie -= Die;
        CharacterStatus.OnDie += Die;

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
    
    public void Die(CharacterStatus characterStats)
    {
    }
    public void Rebirth()
    {

    }
    public void AutoRecoverStamina()
    {
        characterStatus.CurrentStamina += (characterStatus.MaxStamina * Constants.CHARACTER_STAMINA_AUTO_RECOVERY * 0.01f * Time.deltaTime);
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
    public CharacterStatus CharacterStatus { get { return characterStatus; } }
    public CharacterState CharacterState { get { return characterState; } }
    
    public PlayerCamera PlayerCamera { get { return playerCamera; } set { playerCamera = value; } }
    public CharacterController CharacterController { get { return characterController; } }
    public Animator CharacterAnimator { get { return characterAnimator; } }
    public InventoryPopup CharacterInventory { get { return characterInventory; } }
    #endregion
}
