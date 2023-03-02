using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateParryingAttack : IActionState<BaseCharacter>
{
    private int stateWeight;
    private int animationNameHash;

    public CharacterStateParryingAttack()
    {
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_PARRYING_ATTACK;
        animationNameHash = Constants.ANIMATION_NAME_HASH_PARRYING_ATTACK;
    }

    public void Enter(BaseCharacter character)
    {
        character.Animator.Play(animationNameHash);
    }

    public void Update(BaseCharacter character)
    {
        // !! When animation is over
        if (character.State.SetStateByUpperAnimationTime(animationNameHash, ACTION_STATE.PLAYER_IDLE, 0.9f))
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
