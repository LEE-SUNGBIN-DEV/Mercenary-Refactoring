using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateLanding : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private AnimationClipInfo animationClipInformation;

    public PlayerStateLanding(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_LANDING;
        animationClipInformation = character.AnimationClipTable["Player_Landing"];
    }

    public void Enter()
    {
        character.MoveController.SetMove(Vector3.zero, 0f);
        character.Animator.Play(animationClipInformation.nameHash);
        character.StatusData.ReduceHP(character.MoveController.GetFallDamageRate(), VALUE_TYPE.PERCENTAGE);
    }

    public void Update()
    {
        // !! When animation is over
        if (character.State.SetStateByAnimationTimeUpTo(animationClipInformation.nameHash, character.CurrentWeapon.IdleState, 0.9f))
            return;
    }

    public void Exit()
    {
        character.MoveController.SetMove(Vector3.zero, 0f);
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
