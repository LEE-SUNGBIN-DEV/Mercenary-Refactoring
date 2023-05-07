using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateHeavyHit : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;

    private Animator animator;
    private AnimationClipInformation animationClipInformation;

    public PlayerStateHeavyHit(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_HIT_HEAVY;

        animator = character.Animator;
        animationClipInformation = character.AnimationClipTable["Player_Heavy_Hit"];
    }

    public void Enter()
    {
        animator.Play(animationClipInformation.nameHash);
    }

    public void Update()
    {
        // -> Hit Heavy Loop
        if (character.State.SetStateByAnimationTimeUpTo(animationClipInformation.nameHash, ACTION_STATE.PLAYER_HIT_HEAVY_LOOP, 1.0f))
            return;
    }

    public void Exit()
    {
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
