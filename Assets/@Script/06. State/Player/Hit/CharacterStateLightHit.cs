using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateLightHit : IActionState<BaseCharacter>
{
    private int stateWeight;
    private int animationNameHash;

    public CharacterStateLightHit()
    {
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_HIT_LIGHT;
        animationNameHash = Constants.ANIMATION_NAME_HASH_LIGHT_HIT;
    }

    public void Enter(BaseCharacter character)
    {
        character.Animator.Play(animationNameHash);
    }

    public void Update(BaseCharacter character)
    {
        // !! When animation is over
        if (character.State.SetStateByAnimationTimeUpTo(animationNameHash, ACTION_STATE.PLAYER_IDLE, 0.9f))
            return;
    }

    public void Exit(BaseCharacter character)
    {
    }

    #region Property
    public int StateWeight { get => stateWeight; }
    #endregion
}
