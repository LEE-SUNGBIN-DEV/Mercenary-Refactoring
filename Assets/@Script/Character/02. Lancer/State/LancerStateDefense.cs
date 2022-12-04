using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LancerStateDefense : ICharacterState
{
    private int stateWeight;
    private bool isDefense;
    private Lancer lancer;

    public LancerStateDefense()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.LancerDefense;
        isDefense = false;
        lancer = null;
    }

    public void Enter(Character character)
    {
        isDefense = false;
        lancer = character as Lancer;
    }
    public void Update(Character character)
    {
        if (character.PlayerInput.IsMouseRightDown)
        {
            if (!isDefense)
            {
                lancer.Shield?.gameObject.SetActive(true);
            }

            character.gameObject.tag = Constants.TAG_INVINCIBILITY;
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
                character.Animator.SetBool("isCounterAttack", !character.PlayerInput.IsMouseRightUp);
            }
        }
    }
    public void Exit(Character character)
    {
        isDefense = false;
        character.Animator.SetBool("isDefense", false);
        character.Animator.SetBool("isCounterAttack", false);
        lancer.Shield?.gameObject.SetActive(false);
        lancer = null;
    }

    #region Property
    public int StateWeight
    {
        get => stateWeight;
    }
    #endregion
}
