using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateStun : ICharacterState
{
    private int stateWeight;

    public CharacterStateStun()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.Stun;
    }

    public void Enter(BaseCharacter character)
    {
        character.Animator.SetTrigger("doStun");
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
