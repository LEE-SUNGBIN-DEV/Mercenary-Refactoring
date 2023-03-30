using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalberdParryingAttack : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private int animationNameHash;

    public HalberdParryingAttack(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_PARRYING_ATTACK;
        animationNameHash = Constants.ANIMATION_NAME_HASH_HALBERD_PARRYING_ATTACK;
    }

    public void Enter()
    {
        character.Animator.Play(animationNameHash);
    }

    public void Update()
    {
        // !! When animation is over
        if (character.State.SetStateByAnimationTimeUpTo(animationNameHash, ACTION_STATE.PLAYER_HALBERD_IDLE, 0.9f))
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
