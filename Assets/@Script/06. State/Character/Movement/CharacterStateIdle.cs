using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateIdle : IActionState<BaseCharacter>
{
    private int stateWeight;
    private int animationNameHash;
    private Vector3 moveInput;

    public CharacterStateIdle()
    {
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_IDLE;
        animationNameHash = Constants.ANIMATION_NAME_HASH_IDLE;
        moveInput = Vector3.zero;
    }

    public void Enter(BaseCharacter character)
    {
        moveInput = Vector3.zero;
        character.Animator.CrossFade(animationNameHash, 0.2f);
    }

    public void Update(BaseCharacter character)
    {
        if (Input.GetKeyDown(KeyCode.Space) && character.Status.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_ROLL))
        {
            character.State.TryStateSwitchingByWeight(ACTION_STATE.PLAYER_ROLL);
            return;
        }

        if (Input.GetKeyDown(KeyCode.R) && character.Status.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_SKILL_COUNTER))
        {
            character.State.TryStateSwitchingByWeight(ACTION_STATE.PLAYER_SKILL_COUNTER);
            return;
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            character.State.TryStateSwitchingByWeight(ACTION_STATE.PLAYER_ATTACK_LIGHT_01);
            return;
        }

        if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1))
        {
            character.State.TryStateSwitchingByWeight(ACTION_STATE.PLAYER_DEFENSE_START);
            return;
        }

        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if(character.IsGround)
        {
            character.CharacterData.StatusData.AutoRecoverStamina(Constants.PLAYER_STAMINA_IDLE_AUTO_RECOVERY);
            if (moveInput.sqrMagnitude > 0)
            {
                // Run
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    character.State.TryStateSwitchingByWeight(ACTION_STATE.PLAYER_RUN);
                    return;
                }
                // Walk
                else
                {
                    character.State.TryStateSwitchingByWeight(ACTION_STATE.PLAYER_WALK);
                    return;
                }
            }
        }
        // Fall
        else
        {

        }        
    }

    public void Exit(BaseCharacter character)
    {
    }

    #region Property
    public int StateWeight { get => stateWeight; }
    #endregion
}
