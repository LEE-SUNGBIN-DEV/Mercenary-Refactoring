using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateIdle : ICharacterState
{
    private int stateWeight;
    private Vector3 moveInput;

    public CharacterStateIdle()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.Idle;
        moveInput = Vector3.zero;
    }

    public void Enter(BaseCharacter character)
    {
        moveInput = Vector3.zero;
        character.Animator.CrossFade(Constants.ANIMATION_NAME_IDLE, 0.2f);
    }

    public void Update(BaseCharacter character)
    {
        if (Input.GetKeyDown(KeyCode.Space) && character.StatusData.CheckStamina(Constants.CHARACTER_STAMINA_CONSUMPTION_ROLL))
        {
            character.TrySwitchState(CHARACTER_STATE.Roll);
            return;
        }

        if (Input.GetKeyDown(KeyCode.R) && character.StatusData.CheckStamina(Constants.CHARACTER_STAMINA_CONSUMPTION_COUNTER))
        {
            character.TrySwitchState(CHARACTER_STATE.Skill);
            return;
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            character.TrySwitchState(CHARACTER_STATE.Combo_1);
            return;
        }

        if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1))
        {
            character.TrySwitchState(CHARACTER_STATE.Defense);
            return;
        }

        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = 0;
        moveInput.z = Input.GetAxisRaw("Vertical");

        if(character.IsGround)
        {
            character.CharacterData.StatusData.AutoRecoverStamina(Constants.CHARACTER_STAMINA_IDLE_AUTO_RECOVERY);
            if (moveInput.sqrMagnitude > 0)
            {
                // Run
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    character.TrySwitchState(CHARACTER_STATE.Run);
                    return;
                }
                // Walk
                else
                {
                    character.TrySwitchState(CHARACTER_STATE.Walk);
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
