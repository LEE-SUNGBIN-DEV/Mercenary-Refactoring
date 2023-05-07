using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalberdIdle : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;

    private AnimationClipInformation animationClipInformation;
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
        if (Input.GetKeyDown(KeyCode.Tab) && character.TryEquipWeapon(WEAPON_TYPE.SWORD_SHIELD))
        {
            character.State.SetState(character.CurrentWeapon.IdleState, STATE_SWITCH_BY.FORCED);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && character.Status.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_ROLL))
        {
            character.State.SetState(ACTION_STATE.PLAYER_ROLL, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        if (Input.GetKeyDown(KeyCode.R) && character.Status.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_SKILL_COUNTER))
        {
            character.State.SetState(ACTION_STATE.PLAYER_HALBERD_SKILL_COUNTER, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            character.State.SetState(ACTION_STATE.PLAYER_HALBERD_ATTACK_LIGHT_01, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1))
        {
            character.State.SetState(ACTION_STATE.PLAYER_HALBERD_GUARD_IN, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        character.CharacterData.StatusData.AutoRecoverStamina(Constants.PLAYER_STAMINA_IDLE_AUTO_RECOVERY);
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (moveInput.sqrMagnitude > 0)
        {
            // -> Run
            if (Input.GetKey(KeyCode.LeftShift))
                character.State.SetState(ACTION_STATE.PLAYER_HALBERD_RUN, STATE_SWITCH_BY.WEIGHT);

            // -> Walk
            else
                character.State.SetState(ACTION_STATE.PLAYER_HALBERD_WALK, STATE_SWITCH_BY.WEIGHT);
        }
    }

    public void Exit()
    {
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
