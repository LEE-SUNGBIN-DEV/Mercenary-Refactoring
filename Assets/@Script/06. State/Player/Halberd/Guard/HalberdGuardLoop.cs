using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalberdGuardLoop : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private PlayerHalberd halberd;
    private AnimationClipInformation animationClipInformation;

    public HalberdGuardLoop(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_GUARD_LOOP;
        if(character != null)
        {
            halberd = character.WeaponController.GetWeapon<PlayerHalberd>(WEAPON_TYPE.HALBERD);
            animationClipInformation = character.AnimationClipTable["Halberd_Guard_Loop"];
        }
    }

    public void Enter()
    {
        character.IsInvincible = true;
        halberd.SetAndEnableHalberd(COMBAT_ACTION_TYPE.HALBERD_GUARD_LOOP);
        character.Animator.Play(animationClipInformation.nameHash);
    }

    public void Update()
    {
        if (!Input.GetMouseButton(1) && character.State.SetStateNotInTransition(animationClipInformation.nameHash, ACTION_STATE.PLAYER_HALBERD_GUARD_OUT))
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
