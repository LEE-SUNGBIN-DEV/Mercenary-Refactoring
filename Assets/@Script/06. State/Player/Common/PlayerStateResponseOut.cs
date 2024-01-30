using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateResponseOut : IActionState
{
    private int stateWeight;
    private Animator animator;
    private StateController stateController;
    private UniqueEquipmentController weaponController;
    private AnimationClipInfo animationClipInformation;

    public PlayerStateResponseOut(PlayerCharacter character)
    {
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_RESPONSE_OUT;
        animator = character.Animator;
        stateController = character.State;
        weaponController = character.UniqueEquipmentController;
        animationClipInformation = character.AnimationClipTable["Player_Response_Out"];
    }

    public void Enter()
    {
        animator.CrossFadeInFixedTime(animationClipInformation.nameHash, 0.1f);
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
