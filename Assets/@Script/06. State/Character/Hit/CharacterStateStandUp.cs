using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateStandUp : ICharacterState
{
    private int stateWeight;
    private int animationNameHash;

    public CharacterStateStandUp()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.StandUp;
        animationNameHash = Constants.ANIMATION_NAME_STAND_UP;
    }

    public void Enter(BaseCharacter character)
    {
        character.Animator.Play(animationNameHash);
        //character.IsInvincible = true;
    }

    public void Update(BaseCharacter character)
    {
        // !! When animation is over
        if (character.State.SetStateByUpperAnimationTime(animationNameHash, CHARACTER_STATE.Idle, 1.0f))
            return;
    }

    public void Exit(BaseCharacter character)
    {
        //character.IsInvincible = false;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
