using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateDefenseLoop : ICharacterState
{
    private int stateWeight;
    private int animationNameHash;

    public CharacterStateDefenseLoop()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.Defense_Loop;
        animationNameHash = Constants.ANIMATION_NAME_HASH_DEFENSE_LOOP;
    }

    public void Enter(BaseCharacter character)
    {
        character.IsInvincible = true;
        character.Shield.OnEnableDefense(DEFENSE_TYPE.Defense);
        character.Animator.Play(animationNameHash);
    }

    public void Update(BaseCharacter character)
    {
        if (!Input.GetMouseButton(1) && character.State.SetStateNotInTransition(animationNameHash, CHARACTER_STATE.Defense_End))
        {
            return;
        }
    }

    public void Exit(BaseCharacter character)
    {
        character.IsInvincible = false;
        character.Shield.OnDisableDefense();
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
