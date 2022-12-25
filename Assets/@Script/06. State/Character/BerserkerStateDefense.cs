using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerserkerStateDefense : ICharacterState
{
    private int stateWeight;
    private bool isDefense;
    private Berserker berserker;

    public BerserkerStateDefense(Berserker berserker)
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.Defense;
        isDefense = false;
        this.berserker = berserker;
    }

    public void Enter(BaseCharacter character)
    {
        isDefense = false;
    }
    public void Update(BaseCharacter character)
    {
        if (Managers.InputManager.MouseRightPress || Managers.InputManager.MouseRightDown)
        {
            isDefense = true;
            character.IsInvincible = true;
            character.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_DEFENSE, true);
        }

        if (isDefense && !Managers.InputManager.MouseRightDown && !Managers.InputManager.MouseRightPress)
            character.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_DEFENSE, false);

        if (isDefense && (Managers.InputManager.MouseRightDown || Managers.InputManager.MouseRightPress))
            character.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_PARRYING_ATTACK, true);

        if (character.Animator.GetNextAnimatorStateInfo(0).IsName(Constants.ANIMATOR_STATE_NAME_MOVE_BLEND_TREE))
            character.SwitchCharacterState(CHARACTER_STATE.Move);
    }
    public void Exit(BaseCharacter character)
    {
        character.IsInvincible = false;
        isDefense = false;
        character.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_DEFENSE, false);
        character.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_PARRYING_ATTACK, false);
        berserker.Shield.OnDisableDefense();
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
