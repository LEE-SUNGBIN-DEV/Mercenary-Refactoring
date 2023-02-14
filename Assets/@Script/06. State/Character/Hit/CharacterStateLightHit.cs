using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateLightHit : ICharacterState
{
    private int stateWeight;
    private int animationNameHash;

    public CharacterStateLightHit()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.LightHit;
        animationNameHash = Constants.ANIMATION_NAME_LIGHT_HIT;
    }

    public void Enter(BaseCharacter character)
    {
        character.Animator.Play(animationNameHash);
    }

    public void Update(BaseCharacter character)
    {
        // !! When animation is over
        if (character.State.SetStateByUpperAnimationTime(animationNameHash, CHARACTER_STATE.Idle, 0.9f))
            return;
    }

    public void Exit(BaseCharacter character)
    {
    }

    #region Property
    public int StateWeight { get => stateWeight; }
    #endregion
}
