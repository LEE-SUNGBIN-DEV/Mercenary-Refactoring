using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordShieldParrying : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private AnimationClipInformation animationClipInformation;
    private bool mouseRightDown;

    public SwordShieldParrying(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_PARRYING;
        animationClipInformation = character.AnimationClipTable["Sword_Shield_Parrying"];
        mouseRightDown = false;
    }

    public void Enter()
    {
        mouseRightDown = false;
        character.IsInvincible = true;
        character.Animator.Play(animationClipInformation.nameHash);
    }

    public void Update()
    {
        if (!mouseRightDown)
            mouseRightDown = Input.GetMouseButtonDown(1);

        // Move State -> Parrying Attack
        if (mouseRightDown && character.State.SetStateByAnimationTimeUpTo(animationClipInformation.nameHash, ACTION_STATE.PLAYER_SWORD_SHIELD_PARRYING_ATTACK, 0.9f))
        {
            return;
        }

        // !! When animation is over
        if (character.State.SetStateByAnimationTimeUpTo(animationClipInformation.nameHash, ACTION_STATE.PLAYER_SWORD_SHIELD_IDLE, 1f))
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
