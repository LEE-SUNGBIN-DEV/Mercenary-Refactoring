using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateResponseLoop : IActionState
{
    private int stateWeight;
    private Animator animator;
    private AnimationClipInfo animationClipInformation;

    public PlayerStateResponseLoop(PlayerCharacter character)
    {
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_RESPONSE_LOOP;
        animator = character.Animator;
        animationClipInformation = character.AnimationClipTable["Player_Response_Loop"];
    }

    public void Enter()
    {
        animator.CrossFadeInFixedTime(animationClipInformation.nameHash, 0.2f);
    }

    public void Update()
    {
    }

    public void Exit()
    {
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
