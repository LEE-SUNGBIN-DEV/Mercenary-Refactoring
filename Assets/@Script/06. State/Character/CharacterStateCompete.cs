using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateCompete : ICharacterState
{
    private int stateWeight;

    public CharacterStateCompete()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.Compete;
    }

    public void Enter(BaseCharacter character)
    {
        // Set Compete State
        character.IsInvincible = true;
        character.Animator.SetTrigger(Constants.ANIMATOR_PARAMETERS_TRIGGER_COMPETE);
        character.Animator.SetFloat(Constants.ANIMATOR_PARAMETERS_FLOAT_COMPETE, Managers.CompeteManager.CompetePower);
    }

    public void Update(BaseCharacter character)
    {
        character.Animator.SetFloat(Constants.ANIMATOR_PARAMETERS_FLOAT_COMPETE, Managers.CompeteManager.CompetePower);

        if (character.Animator.GetNextAnimatorStateInfo(0).IsName(Constants.ANIMATOR_STATE_NAME_MOVE_BLEND_TREE))
            character.SetState(CHARACTER_STATE.Walk);
    }

    public void Exit(BaseCharacter character)
    {
        character.IsInvincible = false;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
