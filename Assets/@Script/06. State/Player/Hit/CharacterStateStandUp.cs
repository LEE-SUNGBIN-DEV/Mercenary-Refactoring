using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateStandUp : IActionState<BaseCharacter>
{
    private int stateWeight;
    private int animationNameHash;

    public CharacterStateStandUp()
    {
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_STAND_UP;
        animationNameHash = Constants.ANIMATION_NAME_HASH_STAND_UP;
    }

    public void Enter(BaseCharacter character)
    {
        character.Animator.Play(animationNameHash);
        //character.IsInvincible = true;
    }

    public void Update(BaseCharacter character)
    {
        // !! When animation is over
        if (character.State.SetStateByAnimationTimeUpTo(animationNameHash, ACTION_STATE.PLAYER_IDLE, 1.0f))
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
