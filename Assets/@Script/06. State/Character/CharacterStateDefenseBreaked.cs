using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateDefenseBreaked : ICharacterState
{
    private int stateWeight;

    public CharacterStateDefenseBreaked()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.Defense_Breaked;
    }

    public void Enter(BaseCharacter character)
    {
        character.Animator.SetTrigger(Constants.ANIMATOR_PARAMETERS_TRIGGER_BREAKED);
    }

    public void Update(BaseCharacter character)
    {
        // State Control
        if (character.Animator.GetNextAnimatorStateInfo(0).IsName(Constants.ANIMATOR_STATE_NAME_MOVE_BLEND_TREE))
            character.SetState(CHARACTER_STATE.Walk);
    }

    public void Exit(BaseCharacter character)
    {
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
