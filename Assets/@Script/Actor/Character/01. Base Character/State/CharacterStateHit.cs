using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateHit : ICharacterState
{
    private int stateWeight;

    public CharacterStateHit()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.Hit;
    }

    public void Enter(BaseCharacter character)
    {
        character.Animator.SetTrigger("doHit");
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
