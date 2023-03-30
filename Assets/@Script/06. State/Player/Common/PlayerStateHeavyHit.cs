using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateHeavyHit : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private int animationNameHash;
    private Animator animator;

    public PlayerStateHeavyHit(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_HIT_HEAVY;
        animationNameHash = Constants.ANIMATION_NAME_HASH_HEAVY_HIT;
    }

    public void Enter()
    {
        animator = character.Animator;
        animator.Play(animationNameHash);
    }

    public void Update()
    {
        // !! When animation is over
        if (character.State.SetStateByAnimationTimeUpTo(animationNameHash, ACTION_STATE.PLAYER_HIT_HEAVY_LOOP, 1.0f))
        {
            return;
        }
    }

    public void Exit()
    {
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
