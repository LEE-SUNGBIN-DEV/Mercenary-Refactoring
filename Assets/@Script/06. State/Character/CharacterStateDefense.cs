using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateDefense : ICharacterState
{
    private int stateWeight;

    public CharacterStateDefense()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.Defense;
    }

    public void Enter(BaseCharacter character)
    {
        character.IsInvincible = true;
    }

    public void Update(BaseCharacter character)
    {
        // Input Control
        if (Managers.InputManager.MouseRightPress || Managers.InputManager.MouseRightDown)
            character.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_DEFENSE, true);

        else
            character.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_DEFENSE, false);

        // State Control
        if (character.Animator.GetNextAnimatorStateInfo(0).IsName(Constants.ANIMATOR_STATE_NAME_PARRYING))
            character.SetState(CHARACTER_STATE.Parrying);

        if (character.Animator.GetNextAnimatorStateInfo(0).IsName(Constants.ANIMATOR_STATE_NAME_MOVE_BLEND_TREE))
            character.SetState(CHARACTER_STATE.Walk);
    }
    public void Exit(BaseCharacter character)
    {
        character.IsInvincible = false;
        character.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_DEFENSE, false);
        character.Shield.OnDisableDefense();
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
