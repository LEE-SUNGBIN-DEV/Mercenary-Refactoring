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
        if (character.PlayerInput.IsMouseRightDown)
        {
            character.IsInvincible = true;
            isDefense = true;
            character.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_DEFENSE, true);
        }

        if (character.PlayerInput.IsMouseRightUp && isDefense)
        {
            character.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_DEFENSE, false);
        }

        if (isDefense)
        {
            if (character.PlayerInput.IsMouseRightDown || character.PlayerInput.IsMouseRightUp)
            {
                character.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_PARRYING_ATTACK, !character.PlayerInput.IsMouseRightUp);
            }
        }
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
    public int StateWeight
    {
        get => stateWeight;
    }
    #endregion
}
