using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateHeavyHit : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;

    private Animator animator;
    private AnimationClipInformation animationClipInfo;

    public PlayerStateHeavyHit(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_HIT_HEAVY;

        animator = character.Animator;
        animationClipInfo = character.AnimationClipTable["Player_Heavy_Hit"];
    }

    public void Enter()
    {
        animator.Play(animationClipInfo.nameHash);
    }

    public void Update()
    {
        // -> Hit Heavy Loop
        if (character.Animator.IsAnimationFrameUpTo(animationClipInfo, animationClipInfo.maxFrame))
            character.State.SetState(ACTION_STATE.PLAYER_HIT_HEAVY_LOOP, STATE_SWITCH_BY.FORCED);

        // Movement
        if (character.Animator.IsAnimationFrameBetweenTo(animationClipInfo, 0, 20))
            character.MoveController.SetMoveOnly(-character.transform.forward, Constants.PLAYER_HEAVY_HIT_SPEED);
        else
            character.MoveController.SetMovementAndRotation(Vector3.zero, 0f);
    }

    public void Exit()
    {
        character.MoveController.SetMovementAndRotation(Vector3.zero, 0f);
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
