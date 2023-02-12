using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateParryingAttack : ICharacterState
{
    private int stateWeight;

    public CharacterStateParryingAttack()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.Defense;
    }

    public void Enter(BaseCharacter character)
    {
    }

    public void Update(BaseCharacter character)
    {
        // State Control
        if (character.Animator.GetNextAnimatorStateInfo(0).IsName(Constants.ANIMATOR_STATE_NAME_MOVE_BLEND_TREE))
            character.SetState(CHARACTER_STATE.Walk);
    }

    public void Exit(BaseCharacter character)
    {
        character.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_PARRYING_ATTACK, false);
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
