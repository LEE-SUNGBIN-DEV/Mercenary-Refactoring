using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateDefense : ICharacterState
{
    private int stateWeight;
    private int animationNameHash;

    public CharacterStateDefense()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.Defense;
        animationNameHash = Constants.ANIMATION_NAME_HASH_DEFENSE;
    }

    public void Enter(BaseCharacter character)
    {
        character.IsInvincible = true;
        character.Shield.OnEnableDefense(DEFENSE_TYPE.Parrying);
        character.Animator.CrossFade(animationNameHash, 0.1f);
    }

    public void Update(BaseCharacter character)
    {
        if(Input.GetMouseButton(1) && character.State.SetStateByUpperAnimationTime(animationNameHash, CHARACTER_STATE.Defense_Loop, 1.0f))
        {
            return;
        }

        // !! When animation is over
        if (character.State.SetStateByUpperAnimationTime(animationNameHash, CHARACTER_STATE.Defense_End, 1.0f))
        {
            return;
        }
    }

    public void Exit(BaseCharacter character)
    {
        character.IsInvincible = false;
        character.Shield.OnDisableDefense();
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
