using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateParrying : IActionState<BaseCharacter>
{
    private int stateWeight;
    private int animationNameHash;
    private bool mouseRightDown;

    public CharacterStateParrying()
    {
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_PARRYING;
        animationNameHash = Constants.ANIMATION_NAME_HASH_PARRYING;
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
        if (mouseRightDown && character.State.SetStateByUpperAnimationTime(animationNameHash, ACTION_STATE.PLAYER_PARRYING_ATTACK, 0.9f))
        {
            return;
        }

        // !! When animation is over
        if (character.State.SetStateByUpperAnimationTime(animationNameHash, ACTION_STATE.PLAYER_IDLE, 1f))
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
