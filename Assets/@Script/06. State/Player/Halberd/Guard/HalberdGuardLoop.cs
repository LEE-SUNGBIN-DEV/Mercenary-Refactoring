using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalberdGuardLoop : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private AnimationClipInformation animationClipInformation;

    public HalberdGuardLoop(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_GUARD_LOOP;
        animationClipInformation = character.AnimationClipDictionary["Halberd_Guard_Loop"];
    }

    public void Enter()
    {
        character.IsInvincible = true;
        character.Halberd.SetAndEnableHalberd(COMBAT_ACTION_TYPE.HALBERD_GUARD_LOOP);
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
        character.Halberd.DisableHalberd();
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
