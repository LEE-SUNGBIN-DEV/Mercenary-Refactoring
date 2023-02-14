using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateDefenseBreak : ICharacterState
{
    private int stateWeight;
    private int animationNameHash;

    public CharacterStateDefenseBreak()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.Defense_Break;
        animationNameHash = Constants.ANIMATION_NAME_DEFENSE_BREAK;
    }

    public void Enter(BaseCharacter character)
    {
        character.IsInvincible = true;
        character.Animator.Play(animationNameHash);
        character.StatusData.CurrentSP -= Constants.CHARACTER_STAMINA_CONSUMPTION_DEFENSE_BREAK;
    }

    public void Update(BaseCharacter character)
    {
        // !! When animation is over
        if (character.State.SetStateByUpperAnimationTime(animationNameHash, CHARACTER_STATE.Idle, 0.9f))
        {
            return;
        }
    }

    public void Exit(BaseCharacter character)
    {
        character.IsInvincible = false;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
