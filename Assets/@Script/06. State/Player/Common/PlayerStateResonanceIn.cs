using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateResonanceIn : IActionState
{
    private int stateWeight;
    private Animator animator;
    private StateController stateController;
    private AnimationClipInformation animationClipInformation;

    public PlayerStateResonanceIn(PlayerCharacter character)
    {
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_RESONANCE_IN;
        animator = character.Animator;
        stateController = character.State;
        animationClipInformation = character.AnimationClipTable["Player_Resonance_In"];
    }

    public void Enter()
    {
        animator.CrossFadeInFixedTime(animationClipInformation.nameHash, 0.2f);
    }

    public void Update()
    {
        // !! When animation is over
        if (stateController.SetStateByAnimationTimeUpTo(animationClipInformation.nameHash, ACTION_STATE.PLAYER_RESONANCE_LOOP, 0.9f))
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
