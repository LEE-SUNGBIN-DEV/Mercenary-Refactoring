using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateDie : ICharacterState
{
    private int stateWeight;

    public CharacterStateDie()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.Die;
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
