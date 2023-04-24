using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateResonanceOut : IActionState
{
    private int stateWeight;
    private Animator animator;
    private StateController stateController;
    private PlayerWeaponController weaponController;
    private AnimationClipInformation animationClipInformation;

    public PlayerStateResonanceOut(PlayerCharacter character)
    {
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_RESONANCE_OUT;
        animator = character.Animator;
        stateController = character.State;
        weaponController = character.WeaponController;
        animationClipInformation = character.AnimationClipTable["Player_Resonance_Out"];
    }

    public void Enter()
    {
        animator.CrossFadeInFixedTime(animationClipInformation.nameHash, 0.2f);
    }

    public void Update()
    {
        // !! When animation is over
        if (stateController.SetStateByAnimationTimeUpTo(animationClipInformation.nameHash, weaponController.CurrentWeapon.IdleState, 0.9f))
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
