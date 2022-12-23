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
        berserker.Shield.OnDisableDefense();
    }

    #region Property
    public int StateWeight { get => stateWeight; }
    #endregion
}
