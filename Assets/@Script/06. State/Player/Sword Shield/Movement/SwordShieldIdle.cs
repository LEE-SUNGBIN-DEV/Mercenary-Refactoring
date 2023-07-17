using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordShieldIdle : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;

    private AnimationClipInformation animationClipInformation;
    private Vector3 moveInput;

    public SwordShieldIdle(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_IDLE;

        animationClipInformation = character.AnimationClipTable["Sword_Shield_Idle"];
        moveInput = Vector3.zero;
    }

    public void Enter()
    {
        character.Animator.CrossFadeInFixedTime(animationClipInformation.nameHash, 0.3f);
        moveInput = Vector3.zero;
    }

    public void Update()
    {
        if (character.GetInput().SwapDown && character.TryEquipWeapon(WEAPON_TYPE.HALBERD))
        {
            character.State.SetState(character.CurrentWeapon.IdleState, STATE_SWITCH_BY.FORCED);
            return;
        }

        if (character.GetInput().ResonanceWaterDown && character.InventoryData.TryDrinkResonanceWater())
        {
            character.State.SetSubState(ACTION_STATE.PLAYER_DRINK, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        if (character.GetInput().RollDown && character.Status.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_ROLL))
        {
            character.State.SetState(ACTION_STATE.PLAYER_ROLL, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        if (character.GetInput().LeftMouseDown && character.Status.CheckStamina(Constants.SWORD_SHIELD_STAMINA_CONSUMPTION_LIGHT_ATTACK_01))
        {
            character.State.SetState(ACTION_STATE.PLAYER_SWORD_SHIELD_ATTACK_LIGHT_01, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        if (character.GetInput().RightMouseDown || character.GetInput().RightMouseHold)
        {
            if (character.Status.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_GUARD_IN))
            {
                character.State.SetState(ACTION_STATE.PLAYER_SWORD_SHIELD_GUARD_IN, STATE_SWITCH_BY.WEIGHT);
                return;
            }
        }

        moveInput = character.GetInput().MoveInput;

        if (moveInput.sqrMagnitude > 0)
        {
            // -> Run
            if (character.GetInput().RunHold && character.Status.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_RUN))
                character.State.SetState(ACTION_STATE.PLAYER_SWORD_SHIELD_RUN, STATE_SWITCH_BY.WEIGHT);

            // -> Walk
            else
                character.State.SetState(ACTION_STATE.PLAYER_SWORD_SHIELD_WALK, STATE_SWITCH_BY.WEIGHT);
        }

        character.Status.RecoverStaminaPerSec(Constants.PLAYER_STAMINA_IDLE_AUTO_RECOVERY_RATIO, CALCULATE_MODE.Ratio);
    }

    public void Exit()
    {
        character.MoveController.SetMovementAndRotation(Vector3.zero, 0f);
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
