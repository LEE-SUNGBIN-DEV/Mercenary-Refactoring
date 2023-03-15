using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateDefenseEnd : IActionState
{
    private BaseCharacter character;
    private int stateWeight;
    private int animationNameHash;

    public CharacterStateDefenseEnd(BaseCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_DEFENSE_END;
        animationNameHash = Constants.ANIMATION_NAME_HASH_DEFENSE_END;
    }

    public void Enter()
    {
        character.IsInvincible = false;
        character.Animator.Play(animationNameHash);
    }

    public void Update()
    {
        // !! When animation is over
        if (character.State.SetStateByAnimationTimeUpTo(animationNameHash, ACTION_STATE.PLAYER_IDLE, 0.9f))
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
