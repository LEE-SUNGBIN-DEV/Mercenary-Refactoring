using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateIdle : ICharacterState
{
    private int stateWeight;
    private int animationNameHash;
    private Vector3 moveInput;

    public CharacterStateIdle()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.Idle;
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
        if (Input.GetKeyDown(KeyCode.Space) && character.StatusData.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_ROLL))
        {
            character.State.TryStateSwitchingByWeight(CHARACTER_STATE.Roll);
            return;
        }

        if (Input.GetKeyDown(KeyCode.R) && character.StatusData.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_SKILL_COUNTER))
        {
            character.State.TryStateSwitchingByWeight(CHARACTER_STATE.Skill);
            return;
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            character.State.TryStateSwitchingByWeight(CHARACTER_STATE.Light_Attack_01);
            return;
        }

        if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1))
        {
            character.State.TryStateSwitchingByWeight(CHARACTER_STATE.Defense);
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
                    character.State.TryStateSwitchingByWeight(CHARACTER_STATE.Run);
                    return;
                }
                // Walk
                else
                {
                    character.State.TryStateSwitchingByWeight(CHARACTER_STATE.Walk);
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
