using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordShieldRun : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;

    private AnimationClipInformation animationClipInformation;
    private Vector3 moveDirection;

    public SwordShieldRun(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_RUN;

        animationClipInformation = character.AnimationClipTable["Sword_Shield_Run"];
    }

    public void Enter()
    {
        character.Animator.CrossFadeInFixedTime(animationClipInformation.nameHash, 0.2f);
    }

    public void Update()
    {
        if (character.GetInput().SwapDown && character.TryEquipWeapon(WEAPON_TYPE.HALBERD))
        {
            character.State.SetState(character.CurrentWeapon.RunState, STATE_SWITCH_BY.FORCED);
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

        Vector3 horizontalDirection = new Vector3(character.PlayerCamera.transform.right.x, 0, character.PlayerCamera.transform.right.z) * character.GetInput().MoveInput.x;
        Vector3 verticalDirection = new Vector3(character.PlayerCamera.transform.forward.x, 0, character.PlayerCamera.transform.forward.z) * character.GetInput().MoveInput.z;
        moveDirection = (verticalDirection + horizontalDirection);

        if (moveDirection.sqrMagnitude > 0f)
        {
            // -> Run
            if (character.GetInput().RunHold && character.Status.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_RUN))
            {
                character.Status.ConsumeStaminaPerSec(Constants.PLAYER_STAMINA_CONSUMPTION_RUN, CALCULATE_MODE.Absolute);
                character.MoveController.SetMovementAndRotation(moveDirection, character.Status.MoveSpeed * Constants.PLAYER_RUN_SPEED_RATIO);
            }
            // Current
            else
                character.State.SetState(ACTION_STATE.PLAYER_SWORD_SHIELD_WALK, STATE_SWITCH_BY.FORCED);
        }
        // -> Idle
        else
            character.State.SetState(ACTION_STATE.PLAYER_SWORD_SHIELD_IDLE, STATE_SWITCH_BY.FORCED);
    }

    public void Exit()
    {
        character.MoveController.SetMovementAndRotation(Vector3.zero, 0f);
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
