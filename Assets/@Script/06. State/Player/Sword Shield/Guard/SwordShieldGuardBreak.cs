using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordShieldGuardBreak : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private AnimationClipInfo animationClipInfo;

    public SwordShieldGuardBreak(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_GUARD_BREAK;
        animationClipInfo = character.AnimationClipTable["Sword_Shield_Guard_Break"];
    }

    public void Enter()
    {
        character.StatusData.ConsumeStamina(Constants.PLAYER_STAMINA_CONSUMPTION_GUARD_BREAK);
        character.HitState = HIT_STATE.HITTABLE;
        character.Animator.Play(animationClipInfo.nameHash);
        character.SFXPlayer.PlaySFX(Constants.Audio_Shield_Guard_Break);
    }

    public void Update()
    {
        // -> Idle
        if (character.State.SetStateByAnimationTimeUpTo(animationClipInfo.nameHash, ACTION_STATE.PLAYER_SWORD_SHIELD_IDLE, 0.9f))
            return;
    }

    public void Exit()
    {
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
