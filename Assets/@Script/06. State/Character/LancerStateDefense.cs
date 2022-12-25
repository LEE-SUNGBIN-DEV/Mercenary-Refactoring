using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LancerStateDefense : ICharacterState
{
    private int stateWeight;
    private bool isDefense;
    private Lancer lancer;

    public LancerStateDefense(Lancer lancer)
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.Defense;
        isDefense = false;
        this.lancer = lancer;
    }

    public void Enter(BaseCharacter character)
    {
        isDefense = false;
    }
    public void Update(BaseCharacter character)
    {
        if (Managers.InputManager.MouseRightPress || Managers.InputManager.MouseRightDown)
        {
            character.IsInvincible = true;
            isDefense = true;
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
        lancer.Shield.OnDisableDefense();
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
