using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateDie : IActionState<BaseCharacter>
{
    private int stateWeight;

    public CharacterStateDie()
    {
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_DIE;
    }

    public void Enter(BaseCharacter character)
    {
        character.Animator.SetTrigger(Constants.ANIMATOR_PARAMETERS_TRIGGER_DIE);
    }

    public void Update(BaseCharacter character)
    {
    }

    public void Exit(BaseCharacter character)
    {
    }

    #region Property
    public int StateWeight { get => stateWeight; }
    #endregion
}
