using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalberdParrying : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private int animationNameHash;
    private bool mouseRightDown;

    public HalberdParrying(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_PARRYING;
        animationNameHash = Constants.ANIMATION_NAME_HASH_HALBERD_PARRYING;
        mouseRightDown = false;
    }

    public void Enter()
    {
        mouseRightDown = false;
        character.IsInvincible = true;
        character.Animator.Play(animationNameHash);
    }

    public void Update()
    {
        if (!mouseRightDown)
            mouseRightDown = Input.GetMouseButtonDown(1);

        // Move State -> Parrying Attack
        if (mouseRightDown && character.State.SetStateByAnimationTimeUpTo(animationNameHash, ACTION_STATE.PLAYER_HALBERD_PARRYING_ATTACK, 0.9f))
        {
            return;
        }

        // !! When animation is over
        if (character.State.SetStateByAnimationTimeUpTo(animationNameHash, ACTION_STATE.PLAYER_HALBERD_IDLE, 1f))
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
