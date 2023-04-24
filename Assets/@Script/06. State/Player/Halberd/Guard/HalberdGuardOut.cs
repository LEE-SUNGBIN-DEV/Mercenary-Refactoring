using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalberdGuardOut : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private AnimationClipInformation animationClipInformation;

    public HalberdGuardOut(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_GUARD_OUT;
        animationClipInformation = character.AnimationClipTable["Halberd_Guard_Out"];
    }

    public void Enter()
    {
        character.IsInvincible = false;
        character.Animator.Play(animationClipInformation.nameHash);
    }

    public void Update()
    {
        // !! When animation is over
        if (character.State.SetStateByAnimationTimeUpTo(animationClipInformation.nameHash, ACTION_STATE.PLAYER_HALBERD_IDLE, 0.9f))
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
