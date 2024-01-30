using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateDie : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private AnimationClipInfo animationClipInformation;

    public PlayerStateDie(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_DIE;
        animationClipInformation = character.AnimationClipTable["Player_Die"];
    }

    public void Enter()
    {
        character.Animator.Play(animationClipInformation.nameHash);
    }

    public void Update()
    {
    }

    public void Exit()
    {
    }

    #region Property
    public int StateWeight { get => stateWeight; }
    #endregion
}
