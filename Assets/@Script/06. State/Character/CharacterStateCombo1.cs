using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateCombo1 : ICharacterState
{
    private int stateWeight;

    public CharacterStateCombo1()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.Attack;
    }

    public void Enter(BaseCharacter character)
    {
        if (character.Animator.GetCurrentAnimatorStateInfo(0).IsName(Constants.ANIMATOR_STATE_NAME_MOVE_BLEND_TREE))
            character.Animator.SetTrigger(Constants.ANIMATOR_PARAMETERS_TRIGGER_ATTACK);

        character.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_COMBO_ATTACK, false);
        character.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_SMASH_ATTACK, false);

        character.transform.forward = new Vector3(character.PlayerCamera.transform.forward.x, 0, character.PlayerCamera.transform.forward.z);
    }

    public void Update(BaseCharacter character)
    {
        if (character.Animator.GetCurrentAnimatorStateInfo(0).IsName(Constants.ANIMATOR_STATE_NAME_MOVE_BLEND_TREE))        
            return;

        // Combo Attack
        if (Managers.InputManager.MouseLeftDown)
            character.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_COMBO_ATTACK, true);

        // Smash Attack
        if (Managers.InputManager.MouseRightDown)
            character.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_SMASH_ATTACK, true);

        if (character.Animator.GetNextAnimatorStateInfo(0).IsName(Constants.ANIMATOR_STATE_NAME_SMASH_1))
            character.SwitchState(CHARACTER_STATE.Smash_1);

        if (character.Animator.GetNextAnimatorStateInfo(0).IsName(Constants.ANIMATOR_STATE_NAME_COMBO_2))
            character.SwitchState(CHARACTER_STATE.Combo_2);

        if (character.Animator.GetNextAnimatorStateInfo(0).IsName(Constants.ANIMATOR_STATE_NAME_MOVE_BLEND_TREE))
            character.SwitchState(CHARACTER_STATE.Move);
    }

    public void Exit(BaseCharacter character)
    {
        character.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_COMBO_ATTACK, false);
        character.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_SMASH_ATTACK, false);
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
