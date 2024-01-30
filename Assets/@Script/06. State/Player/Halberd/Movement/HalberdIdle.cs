using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalberdIdle : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;

    private AnimationClipInfo animationClipInformation;
    private Vector3 moveInput;

    public HalberdIdle(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_IDLE;

        animationClipInformation = character.AnimationClipTable["Halberd_Idle"];
        moveInput = Vector3.zero;
    }

    public void Enter()
    {
        character.Animator.CrossFadeInFixedTime(animationClipInformation.nameHash, 0.3f);
        moveInput = Vector3.zero;
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

        moveInput = Managers.InputManager.GetCharacterMoveVector();

        if (moveInput.sqrMagnitude > 0)
        {
            // -> Run
            if (Managers.InputManager.CharacterRunButton.IsPressed() && character.StatusData.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_RUN))
                character.State.SetState(ACTION_STATE.PLAYER_HALBERD_RUN, STATE_SWITCH_BY.WEIGHT);

            // -> Walk
            else
                character.State.SetState(ACTION_STATE.PLAYER_HALBERD_WALK, STATE_SWITCH_BY.WEIGHT);
        }

        character.StatusData.RecoverStaminaPerSec(Constants.PLAYER_STAMINA_IDLE_AUTO_RECOVERY_RATIO, VALUE_TYPE.PERCENTAGE);
    }

    public void Exit()
    {
        character.MoveController.SetMove(Vector3.zero, 0f);
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
