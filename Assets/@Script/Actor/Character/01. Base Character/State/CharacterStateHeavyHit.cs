using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateHeavyHit : ICharacterState
{
    private int stateWeight;

    public CharacterStateHeavyHit()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.HeavyHit;
    }

    public void Enter(BaseCharacter character)
    {
        character.Animator.SetTrigger("doHeavyHit");
    }

    public void Update(BaseCharacter character)
    {
    }

    public void Exit(BaseCharacter character)
    {
    }

    #region Property
    public int StateWeight
    {
        get => stateWeight;
    }
    #endregion
}
