using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateResonanceLoop : IActionState
{
    private int stateWeight;
    private Animator animator;
    private StateController stateController;
    private AnimationClipInformation animationClipInformation;

    public PlayerStateResonanceLoop(PlayerCharacter character)
    {
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_RESONANCE_LOOP;
        animator = character.Animator;
        stateController = character.State;
        animationClipInformation = character.AnimationClipTable["Player_Resonance_Loop"];
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
