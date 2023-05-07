using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalberdWalk : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;

    private AnimationClipInformation animationClipInformation;
    private Vector3 moveDirection;

    public HalberdWalk(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_WALK;

        animationClipInformation = character.AnimationClipTable["Halberd_Walk"];
    }

    public void Enter()
    {
        character.Animator.CrossFadeInFixedTime(animationClipInformation.nameHash, 0.3f);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && character.TryEquipWeapon(WEAPON_TYPE.SWORD_SHIELD))
        {
            character.State.SetState(character.CurrentWeapon.WalkState, STATE_SWITCH_BY.FORCED);
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

        character.CharacterData.StatusData.AutoRecoverStamina(Constants.PLAYER_STAMINA_WALK_AUTO_RECOVERY);

        Vector3 verticalDirection = new Vector3(character.PlayerCamera.transform.forward.x, 0, character.PlayerCamera.transform.forward.z) * Input.GetAxisRaw("Vertical");
        Vector3 horizontalDirection = new Vector3(character.PlayerCamera.transform.right.x, 0, character.PlayerCamera.transform.right.z) * Input.GetAxisRaw("Horizontal");
        moveDirection = (verticalDirection + horizontalDirection);

        if (moveDirection.sqrMagnitude > 0f)
        {
            // Run
            if (Input.GetKey(KeyCode.LeftShift))
            {
                character.State.SetState(ACTION_STATE.PLAYER_HALBERD_RUN, STATE_SWITCH_BY.WEIGHT);
            }
            else
            {
                character.MoveController.SetMovement(moveDirection, character.Status.MoveSpeed);
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
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}