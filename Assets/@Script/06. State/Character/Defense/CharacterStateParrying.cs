using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateParrying : ICharacterState
{
    private int stateWeight;
    private int animationNameHash;
    private bool mouseRightDown;

    public CharacterStateParrying()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.Parrying;
        animationNameHash = Constants.ANIMATION_NAME_PARRYING;
        mouseRightDown = false;
    }

    public void Enter(BaseCharacter character)
    {
        mouseRightDown = false;
        character.IsInvincible = true;
        character.Animator.Play(animationNameHash);
    }

    public void Update(BaseCharacter character)
    {
        if (!mouseRightDown)
            mouseRightDown = Input.GetMouseButtonDown(1);

        // Move State -> Parrying Attack
        if (mouseRightDown && character.State.SetStateByUpperAnimationTime(animationNameHash, CHARACTER_STATE.Parrying_Attack, 0.9f))
        {
            return;
        }

        // !! When animation is over
        if (character.State.SetStateByUpperAnimationTime(animationNameHash, CHARACTER_STATE.Idle, 1f))
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
