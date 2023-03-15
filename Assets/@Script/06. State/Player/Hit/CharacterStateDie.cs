using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateDie : IActionState
{
    private BaseCharacter character;
    private int stateWeight;

    public CharacterStateDie(BaseCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_DIE;
    }

    public void Enter()
    {
        character.Animator.SetTrigger(Constants.ANIMATOR_PARAMETERS_TRIGGER_DIE);
    }

    public void Update()
    {
    }

    public void Exit()
    {
    }

    #region Property
    public int StateWeight { get => stateWeight; }
    #endregion
}
