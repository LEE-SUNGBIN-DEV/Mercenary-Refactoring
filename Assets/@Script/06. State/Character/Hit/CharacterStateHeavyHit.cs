using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateHeavyHit : IActionState<BaseCharacter>
{
    private int stateWeight;
    private int animationNameHash;
    private Animator animator;

    public CharacterStateHeavyHit()
    {
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_HIT_HEAVY;
        animationNameHash = Constants.ANIMATION_NAME_HASH_HEAVY_HIT;
    }

    public void Enter(BaseCharacter character)
    {
        animator = character.Animator;
        animator.Play(animationNameHash);
    }

    public void Update(BaseCharacter character)
    {
        // !! When animation is over
        if (character.State.SetStateByUpperAnimationTime(animationNameHash, ACTION_STATE.PLAYER_HIT_HEAVY_LOOP, 1.0f))
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
