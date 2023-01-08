using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateParrying : ICharacterState
{
    private int stateWeight;

    public CharacterStateParrying()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.Parrying;
    }

    public void Enter(BaseCharacter character)
    {
        character.Animator.SetTrigger(Constants.ANIMATOR_PARAMETERS_TRIGGER_PARRYING);
    }

    public void Update(BaseCharacter character)
    {
        // Input Control
        if (Managers.InputManager.MouseRightDown)
            character.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_PARRYING_ATTACK, true);

        // State Control
        if (character.Animator.GetNextAnimatorStateInfo(0).IsName(Constants.ANIMATOR_STATE_NAME_PARRYING_ATTACK))
            character.SwitchState(CHARACTER_STATE.Parrying_Attack);

        if (character.Animator.GetNextAnimatorStateInfo(0).IsName(Constants.ANIMATOR_STATE_NAME_MOVE_BLEND_TREE))
            character.SwitchState(CHARACTER_STATE.Move);
    }
    public void Exit(BaseCharacter character)
    {
        character.Shield.OnDisableDefense();
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
