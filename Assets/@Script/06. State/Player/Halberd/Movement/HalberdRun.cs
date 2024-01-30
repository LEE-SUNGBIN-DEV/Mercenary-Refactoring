using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalberdRun : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;

    private AnimationClipInfo animationClipInformation;
    private Vector3 moveDirection;

    public HalberdRun(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_RUN;

        animationClipInformation = character.AnimationClipTable["Halberd_Run"];
    }

    public void Enter()
    {
        character.Animator.CrossFadeInFixedTime(animationClipInformation.nameHash, 0.2f);
    }

    public void Update()
    {
        if (Managers.InputManager.CharacterSwitchWeaponButton.WasPressedThisFrame() && character.TrySwitchWeapon(WEAPON_TYPE.SWORD_SHIELD))
        {
            character.State.SetState(character.CurrentWeapon.IdleState, STATE_SWITCH_BY.FORCED);
            return;
        }

        if (Managers.InputManager.CharacterDrinkButton.WasPressedThisFrame() && character.InventoryData.TryConsumeResponseWater())
        {
            character.State.SetSubState(ACTION_STATE.PLAYER_DRINK, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        if (Managers.InputManager.CharacterRollButton.WasPressedThisFrame() && character.StatusData.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_ROLL))
        {
            character.State.SetState(ACTION_STATE.PLAYER_ROLL, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        if (Managers.InputManager.CharacterCounterAttackButton.WasPressedThisFrame() && character.StatusData.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_SKILL_COUNTER))
        {
            character.State.SetState(ACTION_STATE.PLAYER_HALBERD_SKILL_COUNTER, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        if (Managers.InputManager.CharacterLightAttackButton.WasPressedThisFrame() && character.StatusData.CheckStamina(Constants.HALBERD_STAMINA_CONSUMPTION_LIGHT_ATTACK_01))
        {
            character.State.SetState(ACTION_STATE.PLAYER_HALBERD_ATTACK_LIGHT_01, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        if (Managers.InputManager.CharacterGuardButton.WasPressedThisFrame() || Managers.InputManager.CharacterGuardButton.IsPressed())
        {
            if (character.StatusData.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_GUARD_IN))
            {
                character.State.SetState(ACTION_STATE.PLAYER_HALBERD_GUARD_IN, STATE_SWITCH_BY.WEIGHT);
                return;
            }
        }

        Vector3 verticalDirection = character.PlayerCamera.GetVerticalDirection() * Managers.InputManager.GetCharacterMoveVector().z;
        Vector3 horizontalDirection = character.PlayerCamera.GetHorizontalDirection() * Managers.InputManager.GetCharacterMoveVector().x;
        moveDirection = (verticalDirection + horizontalDirection);

        if (moveDirection.sqrMagnitude > 0f)
        {
            // Run
            if (Managers.InputManager.CharacterRunButton.IsPressed() && character.StatusData.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_RUN))
            {
                character.StatusData.ConsumeStaminaPerSec(Constants.PLAYER_STAMINA_CONSUMPTION_RUN, VALUE_TYPE.FIXED);
                character.MoveController.SetMove(moveDirection, character.StatusData.StatDict[STAT_TYPE.STAT_MOVE_SPEED].GetFinalValue() * Constants.PLAYER_RUN_SPEED_RATIO);
            }
            else
            {
                character.State.SetState(ACTION_STATE.PLAYER_HALBERD_WALK, STATE_SWITCH_BY.FORCED);
            }
        }
        // Idle
        else
        {
            character.State.SetState(ACTION_STATE.PLAYER_HALBERD_IDLE, STATE_SWITCH_BY.FORCED);
        }
    }

    public void Exit()
    {
        character.MoveController.SetMove(Vector3.zero, 0f);
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
