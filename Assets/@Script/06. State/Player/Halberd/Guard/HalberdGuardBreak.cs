using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalberdGuardBreak : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private AnimationClipInformation animationClipInformation;

    public HalberdGuardBreak(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_GUARD_BREAK;
        animationClipInformation = character.AnimationClipTable["Halberd_Guard_Break"];
    }

    public void Enter()
    {
        character.HitState = HIT_STATE.Hittable;
        character.Animator.Play(animationClipInformation.nameHash);
        character.SFXPlayer.PlaySFX(Constants.Audio_Halberd_Guard_Break);
        character.Status.CurrentSP -= Constants.PLAYER_STAMINA_CONSUMPTION_DEFENSE_BREAK;
    }

    public void Update()
    {
        // -> Idle
        if (character.State.SetStateByAnimationTimeUpTo(animationClipInformation.nameHash, ACTION_STATE.PLAYER_HALBERD_IDLE, 0.9f))
            return;
    }

    public void Exit()
    {
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
