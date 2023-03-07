using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateDefenseEnd : IActionState<BaseCharacter>
{
    private int stateWeight;
    private int animationNameHash;

    public CharacterStateDefenseEnd()
    {
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_DEFENSE_END;
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
        if (character.State.SetStateByAnimationTimeUpTo(animationNameHash, ACTION_STATE.PLAYER_IDLE, 0.9f))
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
