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
            if (!isDefense)
            {
                berserker.Weapon.OnSetWeapon(COMBAT_TYPE.PlayerDefense);
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
    public void Exit(BaseCharacter character)
    {
        character.IsInvincible = false;
        isDefense = false;
        character.Animator.SetBool("isDefense", false);
        character.Animator.SetBool("isParryingAttack", false);
        berserker.Weapon.OnReleaseWeapon();
    }

    #region Property
    public int StateWeight
    {
        get => stateWeight;
    }
    #endregion
}
