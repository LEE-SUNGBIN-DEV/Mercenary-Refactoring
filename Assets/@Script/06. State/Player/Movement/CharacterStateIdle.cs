using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateIdle : IActionState
{
    private BaseCharacter character;
    private int stateWeight;
    private int animationNameHash;
    private Vector3 moveInput;

    public CharacterStateIdle(BaseCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_IDLE;
        animationNameHash = Constants.ANIMATION_NAME_HASH_IDLE;
        moveInput = Vector3.zero;
    }

    public void Enter()
    {
        moveInput = Vector3.zero;
        character.Animator.CrossFade(animationNameHash, 0.2f);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && character.Status.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_ROLL))
        {
            character.State.SetState(ACTION_STATE.PLAYER_ROLL, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        if (Input.GetKeyDown(KeyCode.R) && character.Status.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_SKILL_COUNTER))
        {
            character.State.SetState(ACTION_STATE.PLAYER_SKILL_COUNTER, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            character.State.SetState(ACTION_STATE.PLAYER_ATTACK_LIGHT_01, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1))
        {
            character.State.SetState(ACTION_STATE.PLAYER_DEFENSE_START, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if(character.FallController.IsGround())
        {
            character.CharacterData.StatusData.AutoRecoverStamina(Constants.PLAYER_STAMINA_IDLE_AUTO_RECOVERY);
            if (moveInput.sqrMagnitude > 0)
            {
                // Run
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    character.State.SetState(ACTION_STATE.PLAYER_RUN, STATE_SWITCH_BY.WEIGHT);
                    return;
                }
                // Walk
                else
                {
                    character.State.SetState(ACTION_STATE.PLAYER_WALK, STATE_SWITCH_BY.WEIGHT);
                    return;
                }
            }
        }
        // Fall
        else
        {

        }        
    }

    public void Exit()
    {
    }

    #region Property
    public int StateWeight { get => stateWeight; }
    #endregion
}
