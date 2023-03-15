using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateDefenseBreak : IActionState
{
    private BaseCharacter character;
    private int stateWeight;
    private int animationNameHash;

    public CharacterStateDefenseBreak(BaseCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_DEFENSE_BREAK;
        animationNameHash = Constants.ANIMATION_NAME_HASH_DEFENSE_BREAK;
    }

    public void Enter()
    {
        character.IsInvincible = true;
        character.Animator.Play(animationNameHash);
        character.Status.CurrentSP -= Constants.PLAYER_STAMINA_CONSUMPTION_DEFENSE_BREAK;
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
        character.IsInvincible = false;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
