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
        character.Animator.CrossFadeInFixedTime(animationClipInformation.nameHash, 0.3f);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && character.TryEquipWeapon(WEAPON_TYPE.HALBERD))
        {
            character.State.SetState(character.CurrentWeapon.RunState, STATE_SWITCH_BY.FORCED);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && character.Status.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_ROLL))
        {
            character.State.SetState(ACTION_STATE.PLAYER_ROLL, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            character.State.SetState(ACTION_STATE.PLAYER_SWORD_SHIELD_ATTACK_LIGHT_01, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1))
        {
            character.State.SetState(ACTION_STATE.PLAYER_SWORD_SHIELD_GUARD_IN, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        Vector3 verticalDirection = new Vector3(character.PlayerCamera.transform.forward.x, 0, character.PlayerCamera.transform.forward.z) * Input.GetAxisRaw("Vertical");
        Vector3 horizontalDirection = new Vector3(character.PlayerCamera.transform.right.x, 0, character.PlayerCamera.transform.right.z) * Input.GetAxisRaw("Horizontal");
        moveDirection = (verticalDirection + horizontalDirection);

        if (moveDirection.sqrMagnitude > 0f)
        {
            // Run
            if (Input.GetKey(KeyCode.LeftShift))
            {
                character.CharacterData.StatusData.CurrentSP -= (Constants.PLAYER_STAMINA_CONSUMPTION_RUN * Time.deltaTime);
                character.MoveController.SetMovement(moveDirection, character.Status.MoveSpeed * Constants.PLAYER_RUN_SPEED_RATIO);
            }
            else
            {
                character.State.SetState(ACTION_STATE.PLAYER_SWORD_SHIELD_WALK, STATE_SWITCH_BY.FORCED);
            }
        }
        // Idle
        else
        {
            character.State.SetState(ACTION_STATE.PLAYER_SWORD_SHIELD_IDLE, STATE_SWITCH_BY.FORCED);
        }
    }

    public void Exit()
    {
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
