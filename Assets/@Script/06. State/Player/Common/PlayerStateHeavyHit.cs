using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateHeavyHit : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private AnimationClipInformation animationClipInformation;
    private Animator animator;

    public PlayerStateHeavyHit(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_HIT_HEAVY;
        animationClipInformation = character.AnimationClipTable["Player_Heavy_Hit"];
    }

    public void Enter()
    {
        animator = character.Animator;
        animator.Play(animationClipInformation.nameHash);
    }

    public void Update()
    {
        // !! When animation is over
        if (character.State.SetStateByAnimationTimeUpTo(animationClipInformation.nameHash, ACTION_STATE.PLAYER_HIT_HEAVY_LOOP, 1.0f))
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
