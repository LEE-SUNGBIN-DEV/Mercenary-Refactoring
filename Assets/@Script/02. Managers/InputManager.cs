using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[System.Serializable]
public class InputManager
{
    public event UnityAction<CHARACTER_INPUT_MODE> OnChangeInputMode;

    private PlayerInputs playerInputs;
    private Stack<CHARACTER_INPUT_MODE> inputStack;
    [SerializeField] private CHARACTER_INPUT_MODE currentInputState;
    
    public void Initialize()
    {
        playerInputs = new PlayerInputs();
        playerInputs.Character.Enable();
        playerInputs.UI.Enable();
        playerInputs.Interaction.Enable();

        inputStack = new Stack<CHARACTER_INPUT_MODE>();
        SwitchInputMode(CHARACTER_INPUT_MODE.ALL);
    }

    private void SwitchInputMode(CHARACTER_INPUT_MODE inputState)
    {
        currentInputState = inputState;
        switch (currentInputState)
        {
            case CHARACTER_INPUT_MODE.ALL:
                playerInputs.Character.Enable();
                playerInputs.UI.Enable();
                playerInputs.Interaction.Enable();
                break;
            case CHARACTER_INPUT_MODE.CHARACTER:
                playerInputs.Character.Enable();
                playerInputs.UI.Disable();
                playerInputs.Interaction.Disable();
                break;
            case CHARACTER_INPUT_MODE.UI:
                playerInputs.Character.Disable();
                playerInputs.UI.Enable();
                playerInputs.Interaction.Disable();
                break;
            case CHARACTER_INPUT_MODE.INTERACTION:
                playerInputs.Character.Disable();
                playerInputs.UI.Disable();
                playerInputs.Interaction.Enable();
                break;
            case CHARACTER_INPUT_MODE.COMBAT:
                playerInputs.Character.Enable();
                playerInputs.UI.Disable();
                playerInputs.Interaction.Disable();
                break;
            case CHARACTER_INPUT_MODE.DIE:
                playerInputs.Character.Disable();
                playerInputs.UI.Disable();
                playerInputs.Interaction.Disable();
                break;
            case CHARACTER_INPUT_MODE.EVENT_SCENE:
                playerInputs.Character.Disable();
                playerInputs.UI.Disable();
                playerInputs.Interaction.Disable();
                break;
        }
#if UNITY_EDITOR
        Debug.Log($"Input Mode: {currentInputState.GetEnumName()}");
#endif
    }
    public void InitializeInputMode(CHARACTER_INPUT_MODE inputState)
    {
        inputStack.Clear();
        SwitchInputMode(inputState);
    }
    public void PushInputMode(CHARACTER_INPUT_MODE inputState)
    {
        inputStack.Push(currentInputState);
        SwitchInputMode(inputState);
    }
    public void PopInputMode()
    {
        SwitchInputMode(inputStack.Pop());
    }

    #region Character Inputs
    public Vector2 GetCharacterMouseVector()
    {
        return playerInputs.Character.MouseVector.ReadValue<Vector2>();
    }
    public Vector3 GetCharacterMoveVector()
    {
        Vector2 moveInput = playerInputs.Character.MoveVector.ReadValue<Vector2>();
        return new Vector3(moveInput.x, 0, moveInput.y);
    }
    public InputAction CharacterRunButton { get { return playerInputs.Character.RunButton; } }
    public InputAction CharacterGuardButton { get { return playerInputs.Character.GuardButton; } }
    public InputAction CharacterLightAttackButton { get { return playerInputs.Character.LightAttackButton; } }
    public InputAction CharacterHeavyAttackButton { get { return playerInputs.Character.HeavyAttackButton; } }
    public InputAction CharacterParryingAttackButton { get { return playerInputs.Character.ParryingAttackButton; } }
    public InputAction CharacterCounterAttackButton { get { return playerInputs.Character.CounterAttackButton; } }
    public InputAction CharacterRollButton { get { return playerInputs.Character.RollButton; } }
    public InputAction CharacterDrinkButton { get { return playerInputs.Character.DrinkButton; } }
    public InputAction CharacterSwitchWeaponButton { get { return playerInputs.Character.SwitchWeaponButton; } }
    #endregion

    #region UI Inputs
    public InputAction UIEscButton { get { return playerInputs.UI.EscButton; } }
    public InputAction UIOptionPanelButton { get { return playerInputs.UI.OptionPanelButton; } }
    public InputAction UIInventoryPanelButton { get { return playerInputs.UI.InventoryPanelButton; } }
    public InputAction UIStatusPanelButton { get { return playerInputs.UI.StatusPanelButton; } }
    public InputAction UISkillPanelButton { get { return playerInputs.UI.SkillPanelButton; } }
    public InputAction UIQuestPanelButton { get { return playerInputs.UI.QuestPanelButton; } }
    public InputAction UIClosePanelButton { get { return playerInputs.UI.ClosePanelButton; } }
    public InputAction UIMouseLeftButton { get { return playerInputs.UI.MouseLeftButton; } }
    public InputAction UIMouseRightButton { get { return playerInputs.UI.MouseRightButton; } }
    public InputAction UIToggleCursorButton { get { return playerInputs.UI.ToggleCursorButton; } }
    public InputAction UIQuickSlot1Button { get { return playerInputs.UI.QuickSlot1Button; } }
    public InputAction UIQuickSlot2Button { get { return playerInputs.UI.QuickSlot2Button; } }
    public InputAction UIQuickSlot3Button { get { return playerInputs.UI.QuickSlot3Button; } }
    public InputAction UIQuickSlot4Button { get { return playerInputs.UI.QuickSlot4Button; } }
    #endregion

    #region Interaction Inputs
    public InputAction InteractionEnterButton { get { return playerInputs.Interaction.EnterInteractionButton; } }
    public InputAction InteractionUpdateButton { get { return playerInputs.Interaction.UpdateInteractionButton; } }
    public InputAction InteractionExitButton { get { return playerInputs.Interaction.ExitInteractionButton; } }
    #endregion

    #region Property
    public CHARACTER_INPUT_MODE CurrentInputState { get { return currentInputState; } }
    public PlayerInputs PlayerInputs { get { return playerInputs; } }
    #endregion
}