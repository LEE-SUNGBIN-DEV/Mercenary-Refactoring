using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateLanding : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private AnimationClipInformation animationClipInformation;

    public PlayerStateLanding(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_LANDING;
        animationClipInformation = character.AnimationClipTable["Player_Landing"];
    }

    public void Enter()
    {
        character.Animator.Play(animationClipInformation.nameHash);
        character.Status.CurrentHP -= character.Status.MaxHP * character.MoveController.GetFallDamage();
    }

    public void Update()
    {
        // !! When animation is over
        if (character.State.SetStateByAnimationTimeUpTo(animationClipInformation.nameHash, character.CurrentWeapon.IdleState, 0.9f))
            return;
    }

    public void Exit()
    {
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
