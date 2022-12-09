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

    public void Enter(Character character)
    {
        isDefense = false;
    }
    public void Update(Character character)
    {
        if (character.PlayerInput.IsMouseRightDown)
        {
            if (!isDefense)
            {
                lancer.Shield.OnSetWeapon(COMBAT_TYPE.PlayerDefense);
            }

            character.IsInvincible = true;
            isDefense = true;
            character.Animator.SetBool("isDefense", true);
        }

        if (character.PlayerInput.IsMouseRightUp && isDefense)
        {
            character.Animator.SetBool("isDefense", false);
        }

        if (isDefense)
        {
            if (character.PlayerInput.IsMouseRightDown || character.PlayerInput.IsMouseRightUp)
            {
                character.Animator.SetBool("isParryingAttack", !character.PlayerInput.IsMouseRightUp);
            }
        }
    }
    public void Exit(Character character)
    {
        character.IsInvincible = false;
        isDefense = false;
        character.Animator.SetBool("isDefense", false);
        character.Animator.SetBool("isParryingAttack", false);
        lancer.Shield.OnReleaseWeapon();
    }

    #region Property
    public int StateWeight
    {
        get => stateWeight;
    }
    #endregion
}
