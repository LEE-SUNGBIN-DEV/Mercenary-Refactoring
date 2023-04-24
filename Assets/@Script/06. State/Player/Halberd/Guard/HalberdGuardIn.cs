using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalberdGuardIn : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private PlayerHalberd halberd;
    private AnimationClipInformation animationClipInformation;

    public HalberdGuardIn(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_GUARD_IN;
        if (character != null)
        {
            halberd = character.WeaponController.GetWeapon<PlayerHalberd>(WEAPON_TYPE.HALBERD);
            animationClipInformation = character.AnimationClipTable["Halberd_Guard_In"];
        }
    }

    public void Enter()
    {
        character.IsInvincible = true;
        halberd.SetAndEnableHalberd(COMBAT_ACTION_TYPE.HALBERD_GUARD_IN);
        character.Animator.CrossFadeInFixedTime(animationClipInformation.nameHash, 0.02f);
    }

    public void Update()
    {
        if (!Input.GetMouseButton(1) && character.State.SetStateNotInTransition(animationClipInformation.nameHash, ACTION_STATE.PLAYER_HALBERD_GUARD_OUT))
        {
            return;
        }

        // !! When animation is over
        if (character.State.SetStateByAnimationTimeUpTo(animationClipInformation.nameHash, ACTION_STATE.PLAYER_HALBERD_GUARD_LOOP, 0.9f))
        {
            return;
        }
    }

    public void Exit()
    {
        character.IsInvincible = false;
        halberd.DisableHalberd();
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
