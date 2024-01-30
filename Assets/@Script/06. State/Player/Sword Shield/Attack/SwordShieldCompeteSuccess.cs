using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordShieldCompeteSuccess : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private AnimationClipInfo animationClipInfo;

    public SwordShieldCompeteSuccess(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_COMPETE_SUCCESS;
        animationClipInfo = character.AnimationClipTable["Compete_Success"];
    }

    public void Enter()
    {
        character.HitState = HIT_STATE.INVINCIBLE;
        character.StatusData.RecoverStamina(100, VALUE_TYPE.PERCENTAGE);
        character.Animator.Play(animationClipInfo.nameHash);
    }

    public void Update()
    {
        // -> Idle
        if (character.State.SetStateByAnimationTimeUpTo(animationClipInfo.nameHash, character.CurrentWeapon.IdleState, 0.9f))
            return;
    }

    public void Exit()
    {
        character.HitState = HIT_STATE.HITTABLE;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
