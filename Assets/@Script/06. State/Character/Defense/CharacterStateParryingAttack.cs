using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateParryingAttack : ICharacterState
{
    private int stateWeight;
    private int animationNameHash;

    public CharacterStateParryingAttack()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.Parrying_Attack;
        animationNameHash = Constants.ANIMATION_NAME_HASH_PARRYING_ATTACK;
    }

    public void Enter(BaseCharacter character)
    {
        character.Animator.Play(animationNameHash);
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
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
