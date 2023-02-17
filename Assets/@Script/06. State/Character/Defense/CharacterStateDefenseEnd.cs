using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateDefenseEnd : ICharacterState
{
    private int stateWeight;
    private int animationNameHash;

    public CharacterStateDefenseEnd()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.Defense_End;
        animationNameHash = Constants.ANIMATION_NAME_HASH_DEFENSE_END;
    }

    public void Enter(BaseCharacter character)
    {
        character.IsInvincible = false;
        character.Animator.Play(animationNameHash);
    }

    public void Update(BaseCharacter character)
    {
        // !! When animation is over
        if (character.State.SetStateByUpperAnimationTime(animationNameHash, CHARACTER_STATE.Idle, 0.9f))
        {
            return;
        }
    }

    public void Exit(BaseCharacter character)
    {
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
