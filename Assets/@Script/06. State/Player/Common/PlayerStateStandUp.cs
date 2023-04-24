using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateStandUp : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private AnimationClipInformation animationClipInformation;

    public PlayerStateStandUp(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_STAND_UP;
        animationClipInformation = character.AnimationClipTable["Player_Stand_Up"];
    }

    public void Enter()
    {
        character.Animator.Play(animationClipInformation.nameHash);
        //character.IsInvincible = true;
    }

    public void Update()
    {
        // -> Idle
        if (character.State.SetStateByAnimationTimeUpTo(animationClipInformation.nameHash, character.CurrentWeapon.IdleState, 1.0f))
            return;
    }

    public void Exit()
    {
        //character.IsInvincible = false;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
