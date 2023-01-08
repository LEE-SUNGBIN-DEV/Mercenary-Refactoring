using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateLightHit : ICharacterState
{
    private int stateWeight;

    public CharacterStateLightHit()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.LightHit;
    }

    public void Enter(BaseCharacter character)
    {
        character.Animator.SetTrigger(Constants.ANIMATOR_PARAMETERS_TRIGGER_LIGHT_HIT);
    }

    public void Update(BaseCharacter character)
    {
        if (character.Animator.GetNextAnimatorStateInfo(0).IsName(Constants.ANIMATOR_STATE_NAME_MOVE_BLEND_TREE))
            character.SwitchState(CHARACTER_STATE.Move);
    }

    public void Exit(BaseCharacter character)
    {
    }

    #region Property
    public int StateWeight { get => stateWeight; }
    #endregion
}
